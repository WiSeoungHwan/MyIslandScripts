using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyIsland
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
        #endregion

        #region Private Field
        private bool isPlayer;
        private UnitData unitData;
        #endregion

        #region Public Method
        public void UnitControllerInit(bool isPlayer, GameObject selectedTheme, MaterialInitData materialInitData, UnitInitData unitInitData)
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
                unitDemage = unitInitData.unitDemage,
                tableCount = 0,
                unitMaterial = new UnitMaterialData()
            };
            // 유닛 위치 초기화 
            if (isPlayer)
            {
                unitControl.transform.position = ReturnMovePos(unitData.unitIndex);
            }
            // 델리게이트 설정
            if (isPlayer)
            {
                unitControl.SetDelegate(Move, Collect);
            }
            // 이벤트 리스너 등록
            if (isPlayer)
            {
                EventManager.Instance.on(EVENT_TYPE.TOWER_WILL_BUILD, TowerButtonTap);
                EventManager.Instance.on(EVENT_TYPE.TABLE_WILL_BUILD, TableButtonTap);
                EventManager.Instance.on(EVENT_TYPE.BUNKER_WILL_BUILD, BunkerButtonTap);
            }

        }
        #endregion

        #region Private Methods

        // 행동 관련
        private void Move(int clickTileIndex)  // 플레이어가 노멀타일을 클릭하면 들어오는 함수 클릭한 타일의 인덱스가 들어옴 
        {
            if (!isNearTile(clickTileIndex)) { return; }
            unitControl.transform.position = ReturnMovePos(clickTileIndex);
            unitData.unitIndex = clickTileIndex;

        }
        private void Collect(MaterialState materialState, int clickTileIndex)
        { // 플레이어가 자원 타일을 클릭하면 들어오는 함수 
            if (!isNearTile(clickTileIndex)) { return; }
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
                EventManager.Instance.emit(EVENT_TYPE.MATERIAL_COLLECT, this, unitData.unitMaterial);
            }


        }

        // Tile 관련
        private bool isNearTile(int clickTileIndex)
        {
            if(clickTileIndex < 0 || clickTileIndex > 24){
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

        private Vector3 ReturnMovePos(int index)
        {
            Vector3 tilePos = ground.GetTile(index).transform.position;
            return new Vector3(tilePos.x, 1f, tilePos.z);
        }

        private void TableButtonTap(EVENT_TYPE eventType, Component sender, object param = null)
        {
            var nearTileIndexs = NearTileList(unitData.unitIndex);
            foreach (var i in nearTileIndexs)
            {
                Tile tile = ground.GetTile(i);
                if (tile.tileData.tileState == TileState.NORMAL)
                {
                    tile.WillBuildTable((TablePoolList)unitData.unitLevel);
                }
            }
        }
        private void BunkerButtonTap(EVENT_TYPE eventType, Component sender, object param = null)
        {
            var nearTileIndexs = NearTileList(unitData.unitIndex);
            foreach (var i in nearTileIndexs)
            {
                Tile tile = ground.GetTile(i);
                if (tile.tileData.tileState == TileState.NORMAL)
                {
                    tile.WillBuildBunker((BunkerPoolList)unitData.unitLevel);
                }
            }
        }

        private void TowerButtonTap(EVENT_TYPE eventType, Component sender, object param = null)
        {
            TowerEnum buildingEnum = (TowerEnum)param;
            var nearTileIndexs = NearTileList(unitData.unitIndex);
            var playerLevelTower = (0, 0, 0);
            switch (unitData.unitLevel)
            {
                case 0:
                    Debug.Log("Player Level is 0");
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
            TowerPoolList towerKey = TowerPoolList.WOOD_1;
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
            }

            foreach (var i in nearTileIndexs)
            {
                Tile tile = ground.GetTile(i);
                if (tile.tileData.tileState == TileState.NORMAL)
                {
                    tile.WillBuildTower(towerKey);
                }
            }
        }
        #endregion
    }

}
