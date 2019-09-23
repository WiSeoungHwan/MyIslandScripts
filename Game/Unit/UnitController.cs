using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyIsland_InGame
{
    // Unit 관련 관리 객체 
    public class UnitController : MonoBehaviour
    {
        #region Serialize Field
        // Ground
        [SerializeField]
        private Ground ground;
        [SerializeField]
        private UnitControl unitControl;
        [SerializeField]
        private GameObject[] unitNearArea;
        [SerializeField]
        private UnitUI unitUI;
        #endregion

        #region Private Field
        private bool isPlayer;
        private bool onBunker;
        private UnitData unitData;
        private bool buildingMode;
        #endregion

        #region Public Method
        public void UnitControllerInit(bool isPlayer, GameObject[] selectedTheme, MaterialInitData materialInitData, UnitInitData unitInitData)
        {
            //플레이어 진영인지
            this.isPlayer = isPlayer;
            // 그라운드 타일 및 자원 생성 
            ground.GroundInit(isPlayer, selectedTheme, materialInitData);
            // 유닛 데이터 생성 
            unitData = new UnitData()
            {
                unitLevel = unitInitData.unitLevel,
                unitIndex = unitInitData.unitIndex,
                unitHp = unitInitData.unitHp,
                unitMaxHp = unitInitData.unitHp,
                unitDemage = unitInitData.unitDemage,
                maxStamina = unitInitData.maxStamina,
                stamina = unitInitData.stamina,
                staminaCoolTime = unitInitData.staminaCoolTime,
                tableCount = 0,
                unitMaterial = new UnitMaterialData()
            };
            // 유닛 위치 초기화 
            unitControl.transform.position = ReturnMovePos(unitData.unitIndex);

            // 유닛 HUD 초기화
            unitUI.SetDelegate(UnitStaminaUp);
            unitUI.UnitUIUpdate(unitData);

            // 델리게이트 설정

            unitControl.SetDelegate(Move, Collect, Build, BuildTap, isPlayer, EnemyAction, EnemyBuildAction, EnemyLevelUp);
            // 이벤트 리스너 등록
            if (isPlayer)
            {
                EventManager.Instance.on(EVENT_TYPE_SINGLE.TOWER_WILL_BUILD, TowerButtonTap);
                EventManager.Instance.on(EVENT_TYPE_SINGLE.TABLE_WILL_BUILD, TableButtonTap);
                EventManager.Instance.on(EVENT_TYPE_SINGLE.BUNKER_WILL_BUILD, BunkerButtonTap);
                EventManager.Instance.on(EVENT_TYPE_SINGLE.WILL_BUILD_OFF, BuildOff);
            }

            EventManager.Instance.on(EVENT_TYPE_SINGLE.TILE_HIT, IsPlayerHit);
            EventManager.Instance.on(EVENT_TYPE_SINGLE.TABLE_BROKEN, TableBroken);
        }
        #endregion

        #region Private Methods
        private void UnitStaminaCount()
        {
            if (unitData.stamina <= 0)
            {
                unitData.stamina = 0;
                if(isPlayer)
                    unitUI.ShowMessege("스테미너가 부족합니다.");
            }
            else
            {
                unitData.stamina -= 1;
            }
            unitUI.UnitStaminaDiscount(unitData.stamina);
        }
        private int UnitStaminaUp()
        {
            if (unitData.stamina < 5)
            {
                unitData.stamina++;
            }
            else
            {
                unitData.stamina = 5;
            }
            return unitData.stamina;
        }
        private bool isHasStamina()
        {
            return unitData.stamina > 0 ? true : false;
        }

        // 행동 관련
        private void Move(int clickTileIndex)  // 플레이어가 노멀타일을 클릭하면 들어오는 함수 클릭한 타일의 인덱스가 들어옴 
        {
            if (!isNearTile(clickTileIndex)) { return; }
            if (!isHasStamina())
            {
                if(isPlayer)
                    unitUI.ShowMessege("스테미너가 부족합니다.");
                unitUI.UnitStaminaDiscount(unitData.stamina);
                return;
            }
            unitControl.BodyActive(true);
            onBunker = false;
            unitControl.transform.position = ReturnMovePos(clickTileIndex);
            unitData.unitIndex = clickTileIndex;
            UnitStaminaCount();
        }
        private void Collect(MaterialState materialState, int clickTileIndex)
        { // 플레이어가 자원 타일을 클릭하면 들어오는 함수 
            if (!isNearTile(clickTileIndex) || onBunker) { return; }
            if (!isHasStamina())
            {
                if(isPlayer)
                    unitUI.ShowMessege("스테미너가 부족합니다.");
                unitUI.UnitStaminaDiscount(unitData.stamina);
                return;
            }
            if (ground.GetTile(clickTileIndex).TileHurt(unitData.unitDemage) == false) { return; }
            switch (materialState)
            {
                case MaterialState.WOOD:
                    unitData.unitMaterial.wood += unitData.unitDemage;
                    break;
                case MaterialState.STONE:
                    unitData.unitMaterial.stone += unitData.unitDemage;
                    break;
                case MaterialState.IRON:
                    unitData.unitMaterial.iron += unitData.unitDemage;
                    break;
                case MaterialState.ADAM:
                    unitData.unitMaterial.adam += unitData.unitDemage;
                    break;
            }
            if (isPlayer)
            {
                EventManager.Instance.emit(EVENT_TYPE_SINGLE.MATERIAL_COLLECT, this, unitData.unitMaterial);
            }
            else
            {
                EventManager.Instance.emit(EVENT_TYPE_SINGLE.ENEMYMATERAIL_COLLECT, this, unitData.unitMaterial);
            }
            UnitStaminaCount();
        }
        private void Build(Tile tile)
        {
            if (onBunker) { return; }
            if (!isHasStamina())
            {
                unitUI.UnitStaminaDiscount(unitData.stamina);
                return;
            }
            bool hasMaterial = false;
            foreach (var i in NearTileList(unitData.unitIndex))
            {
                Tile nearTile = ground.GetTile(i);
                if (nearTile.tileData.tileState != TileState.WILL_BUILD) { continue; }
                if (tile.tileData.index == nearTile.tileData.index)
                {

                    switch (unitData.unitLevel)
                    {
                        case 0:
                            if (unitData.unitMaterial.wood >= 5)
                            {
                                hasMaterial = true;
                                unitData.unitMaterial.wood -= 5;
                            }
                            break;
                        case 1:
                            if (unitData.unitMaterial.wood >= 5)
                            {
                                hasMaterial = true;
                                unitData.unitMaterial.wood -= 5;
                            }
                            break;

                        case 2:
                            if (unitData.unitMaterial.stone >= 5)
                            {
                                hasMaterial = true;
                                unitData.unitMaterial.stone -= 5;
                            }
                            break;

                        case 3:
                            if (unitData.unitMaterial.iron >= 5)
                            {
                                hasMaterial = true;
                                unitData.unitMaterial.iron -= 5;
                            }
                            break;
                        case 4:
                            if (unitData.unitMaterial.adam >= 5)
                            {
                                hasMaterial = true;
                                unitData.unitMaterial.adam -= 5;
                            }
                            break;
                    }
                    if (!hasMaterial)
                    {
                        tile.BuildOff();
                        continue;
                    }
                    switch (tile.GetBuildingKind())
                    {
                        case BuildingKind.TABLE:
                            unitData.tableCount++;
                            if (unitData.unitLevel == 0)
                            {
                                unitData.unitLevel = 1;
                            }
                            if (isPlayer)
                                EventManager.Instance.emit(EVENT_TYPE_SINGLE.TABLE_COUNT_CHANGE, this, unitData.tableCount);
                            break;
                        case BuildingKind.BUNKER:
                            break;
                        case BuildingKind.TOWER:
                            break;
                    }

                    if (hasMaterial)
                    {
                        tile.Build();
                        if (isPlayer)
                            EventManager.Instance.emit(EVENT_TYPE_SINGLE.MATERIAL_COLLECT, this, unitData.unitMaterial);
                        else
                            EventManager.Instance.emit(EVENT_TYPE_SINGLE.ENEMYMATERAIL_COLLECT, this, unitData.unitMaterial);
                    }
                    continue;
                }
                nearTile.BuildOff();
            }
            if (hasMaterial)
                UnitStaminaCount();
            else
                unitUI.ShowMessege("자원이 부족합니다.");
        }

        private void BuildTap(Tile tile)
        {
            if (isNearTile(tile.tileData.index) == false) { return; }
            switch (tile.GetBuildingKind())
            {
                case BuildingKind.TABLE:
                    tile.ButtonUISetActive(() =>
                    {
                        bool hasMaterial = false;

                        switch (unitData.unitLevel)
                        {
                            case 1:
                                if (unitData.unitMaterial.wood >= 10)
                                {
                                    hasMaterial = true;
                                    unitData.unitMaterial.wood -= 10;
                                }
                                break;
                            case 2:
                                if (unitData.unitMaterial.stone >= 10)
                                {
                                    hasMaterial = true;
                                    unitData.unitMaterial.stone -= 10;
                                }
                                break;
                            case 3:
                                if (unitData.unitMaterial.iron >= 10)
                                {
                                    hasMaterial = true;
                                    unitData.unitMaterial.iron -= 10;
                                }
                                break;

                        }

                        if (hasMaterial)
                        {
                            tile.UpgradeTable(unitData.unitLevel);
                            unitData.unitLevel++;
                            EventManager.Instance.emit(EVENT_TYPE_SINGLE.MATERIAL_COLLECT, this, unitData.unitMaterial);

                        }
                        else
                        {
                            if(isPlayer)
                                unitUI.ShowMessege("업그레이드 자원부족");
                        }
                    });
                    break;
                case BuildingKind.BUNKER:
                    if (isNearTile(tile.tileData.index))
                    {
                        onBunker = true;
                        unitControl.BodyActive(false);
                        unitControl.transform.position = ReturnMovePos(tile.tileData.index);
                        unitData.unitIndex = tile.tileData.index;
                    }
                    break;
            }
        }

        // Tile 관련
        private bool isNearTile(int clickTileIndex)
        {
            if (clickTileIndex < 0 || clickTileIndex > 24)
            {
                return false;
            }
            bool isNearTile = false;
            if ((unitData.unitIndex + 5) % 5 == 0)
            { // 왼쪽 사이드 쪽에 있었을때 예외 처리
                if (clickTileIndex == unitData.unitIndex + 1 || clickTileIndex == unitData.unitIndex + 5 || clickTileIndex == unitData.unitIndex - 5)
                {
                    isNearTile = true;
                }
            }
            else if ((unitData.unitIndex + 1) % 5 == 0)
            { // 오른쪽 사이드에 있었을때 예외 처리
                if (clickTileIndex == unitData.unitIndex - 1 || clickTileIndex == unitData.unitIndex + 5 || clickTileIndex == unitData.unitIndex - 5)
                {
                    isNearTile = true;
                }
            }
            else
            {
                if (clickTileIndex == unitData.unitIndex + 1 || clickTileIndex == unitData.unitIndex - 1 || clickTileIndex == unitData.unitIndex + 5 || clickTileIndex == unitData.unitIndex - 5)
                {
                    isNearTile = true;
                }
            }
            return isNearTile;
        }

        private List<int> NearTileList(int unitIndex)
        {
            List<int> nearTileIndexs = new List<int>();
            if (isNearTile(unitIndex + 5))
            {
                nearTileIndexs.Add(unitIndex + 5);
            }
            if (isNearTile(unitIndex - 5))
            {
                nearTileIndexs.Add(unitIndex - 5);
            }
            if (isNearTile(unitIndex + 1))
            {
                nearTileIndexs.Add(unitIndex + 1);
            }
            if (isNearTile(unitIndex - 1))
            {
                nearTileIndexs.Add(unitIndex - 1);
            }

            return nearTileIndexs;
        }

        private void IsPlayerHit(EVENT_TYPE_SINGLE eventType, Component sender, object param = null)
        {
            if ((Tile)param)
            {
                Tile tile = (Tile)param;
                Bullet bullet = (Bullet)sender;
                if (tile.tileData.isPlayerGround != isPlayer) { return; }
                switch (bullet.TowerKind)
                {

                    case TowerKind.STRAIGHT:
                        PlayerHit(bullet.Damage);
                        break;

                    default:
                        if (unitData.unitIndex == tile.tileData.index)
                        {
                            PlayerHit(bullet.Damage);
                        }
                        break;
                }

            }
        }

        private void TableBroken(EVENT_TYPE_SINGLE eventType, Component sender, object param = null)
        {
            Tile tile = (Tile)sender;
            if (tile.tileData.isPlayerGround == isPlayer)
            {
                unitData.tableCount--;
                if (isPlayer)
                    EventManager.Instance.emit(EVENT_TYPE_SINGLE.TABLE_COUNT_CHANGE, this, unitData.tableCount);
            }
        }
        private void PlayerHit(float damage)
        {
            unitData.unitHp -= damage;
            if (unitData.unitHp <= 0)
            {
                unitData.unitHp = 0;
                EventManager.Instance.emit(EVENT_TYPE_SINGLE.GAMEOVER_UNIT_DIE, this, isPlayer);
            }
            unitUI.UnitUIUpdate(unitData);
        }

        private Vector3 ReturnMovePos(int index)
        {
            Vector3 tilePos = ground.GetTile(index).transform.position;
            return new Vector3(tilePos.x, 1f, tilePos.z);
        }

        private void TableButtonTap(EVENT_TYPE_SINGLE eventType, Component sender, object param = null)
        {
            if (onBunker) { return; }
            var nearTileIndexs = NearTileList(unitData.unitIndex);
            foreach (var i in nearTileIndexs)
            {
                Tile tile = ground.GetTile(i);
                if (tile.tileData.tileState == TileState.NORMAL || tile.tileData.tileState == TileState.WILL_BUILD)
                {
                    if (unitData.unitLevel == 4)
                    {
                        tile.WillBuildTable((TablePoolList)3);
                    }
                    if (unitData.unitLevel != 0)
                    {
                        tile.WillBuildTable((TablePoolList)unitData.unitLevel - 1);
                    }
                    else
                    {
                        tile.WillBuildTable((TablePoolList)unitData.unitLevel);
                    }
                }
            }
        }
        private void BunkerButtonTap(EVENT_TYPE_SINGLE eventType, Component sender, object param = null)
        {
            if (onBunker) { return; }
            var nearTileIndexs = NearTileList(unitData.unitIndex);
            foreach (var i in nearTileIndexs)
            {
                Tile tile = ground.GetTile(i);
                if (tile.tileData.tileState == TileState.NORMAL || tile.tileData.tileState == TileState.WILL_BUILD)
                {
                    if (unitData.unitLevel == 4)
                    {
                        tile.WillBuildBunker((BunkerPoolList)3);
                    }
                    tile.WillBuildBunker((BunkerPoolList)unitData.unitLevel - 1);
                }
            }
        }

        private void TowerButtonTap(EVENT_TYPE_SINGLE eventType, Component sender, object param = null)
        {
            if (onBunker) { return; }
            TowerEnum buildingEnum = (TowerEnum)param;
            var nearTileIndexs = NearTileList(unitData.unitIndex);
            var playerLevelTower = (0, 0, 0);
            switch (unitData.unitLevel)
            {
                case 0:
                    if(isPlayer)
                        unitUI.ShowMessege("작업대를 지어주세요.");
                    break;
                case 1:
                    playerLevelTower = (0, 1, 2);
                    break;
                case 2:
                    playerLevelTower = (3, 4, 5);
                    break;
                case 3:
                    playerLevelTower = (6, 7, 8);
                    break;
                case 4:
                    playerLevelTower = (9, 10, 11);
                    break;
            }
            TowerPoolList towerKey;
            switch (buildingEnum)
            {
                case TowerEnum.TOWER_1:
                    towerKey = (TowerPoolList)playerLevelTower.Item1;
                    break;
                case TowerEnum.TOWER_2:
                    towerKey = (TowerPoolList)playerLevelTower.Item2;
                    break;
                case TowerEnum.TOWER_3:
                    towerKey = (TowerPoolList)playerLevelTower.Item3;
                    break;
                default:
                    towerKey = TowerPoolList.WOOD_1;
                    break;
            }

            foreach (var i in nearTileIndexs)
            {
                Tile tile = ground.GetTile(i);
                if (tile.tileData.tileState == TileState.NORMAL || tile.tileData.tileState == TileState.WILL_BUILD)
                {
                    tile.WillBuildTower(towerKey);
                }
            }
        }

        private void BuildOff(EVENT_TYPE_SINGLE eventType, Component sender, object param = null)
        {
            var nearTileIndexs = NearTileList(unitData.unitIndex);
            foreach (var i in nearTileIndexs)
            {
                Tile tile = ground.GetTile(i);
                if (tile.tileData.tileState == TileState.WILL_BUILD)
                {
                    tile.BuildOff();
                }
            }
        }
        #endregion

        #region Enemy Action
        private void EnemyLevelUp()
        {
            var tiles = NearTileList(unitData.unitIndex);
            bool isLevelUp = false;
            foreach (var i in tiles)
            {
                var tile = ground.GetTile(i);
                if (tile.tileData.tileState == TileState.BUILDING)
                {
                    if (tile.GetBuildingKind() == BuildingKind.TABLE)
                    {
                        bool hasMaterial = false;

                        switch (unitData.unitLevel)
                        {
                            case 1:
                                if (unitData.unitMaterial.wood >= 10)
                                {
                                    hasMaterial = true;
                                    unitData.unitMaterial.wood -= 10;
                                }
                                break;
                            case 2:
                                if (unitData.unitMaterial.stone >= 10)
                                {
                                    hasMaterial = true;
                                    unitData.unitMaterial.stone -= 10;
                                }
                                break;
                            case 3:
                                if (unitData.unitMaterial.iron >= 10)
                                {
                                    hasMaterial = true;
                                    unitData.unitMaterial.iron -= 10;
                                }
                                break;

                        }
                        if(hasMaterial){
                            tile.UpgradeTable(unitData.unitLevel);
                            isLevelUp = true;
                        }
                        
                    }
                }

            }
            if (isLevelUp)
                unitData.unitLevel = unitData.unitLevel >= 3 ? 3 : unitData.unitLevel + 1;
        }
        private void EnemyAction(int inputIndex)
        {
            var curIndex = unitData.unitIndex + inputIndex;
            bool isInGround = (curIndex >= 0 && curIndex < 26);
            if (!isInGround) { return; }
            Tile tile = ground.GetTile(curIndex);
            switch (tile.tileData.tileState)
            {
                case TileState.NORMAL:
                    Move(curIndex);
                    break;
                case TileState.MATERIAL:
                    Collect(tile.tileData.materialState, curIndex);
                    break;
                case TileState.BUILDING:
                    break;
                case TileState.WILL_BUILD:
                    Build(tile);
                    break;
            }
        }
        private void EnemyBuildAction(EnemyBuildingIndex buildingIndex)
        {
            if (buildingIndex != EnemyBuildingIndex.TABLE && buildingIndex != EnemyBuildingIndex.NONE)
            {
                if (unitData.tableCount < 1)
                {
                    Debug.Log("No table");
                    return;
                }
            }
            switch (buildingIndex)
            {
                case EnemyBuildingIndex.TOWER_1:
                    TowerButtonTap(EVENT_TYPE_SINGLE.TOWER_WILL_BUILD, this, TowerEnum.TOWER_1);
                    break;
                case EnemyBuildingIndex.TOWER_2:
                    TowerButtonTap(EVENT_TYPE_SINGLE.TOWER_WILL_BUILD, this, TowerEnum.TOWER_2);
                    break;
                case EnemyBuildingIndex.TOWER_3:
                    TowerButtonTap(EVENT_TYPE_SINGLE.TOWER_WILL_BUILD, this, TowerEnum.TOWER_3);
                    break;
                case EnemyBuildingIndex.BUNKER:
                    BunkerButtonTap(EVENT_TYPE_SINGLE.BUNKER_WILL_BUILD, this, null);
                    break;
                case EnemyBuildingIndex.TABLE:
                    TableButtonTap(EVENT_TYPE_SINGLE.TABLE_WILL_BUILD, this, null);
                    break;
                case EnemyBuildingIndex.NONE:
                    BuildOff(EVENT_TYPE_SINGLE.WILL_BUILD_OFF, this, null);
                    break;
            }
        }
        #endregion
    }

}
