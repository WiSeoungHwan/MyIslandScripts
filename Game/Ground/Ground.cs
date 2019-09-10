using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyIsland
{
    public class Ground : MonoBehaviour
    {
        #region Serialize Fields
        // 테마 그라운드 
        [SerializeField]
        private Transform body;

        [SerializeField]
        private Tile tile;

        #endregion

        #region Private Fields
        // 타일 리스트 
        private bool isPlayerGround;
        private List<Tile> tileList = new List<Tile>();
        #endregion

        #region Public Methods
        public void GroundInit(bool isPlayerGround, GameObject[] selectedTheme, MaterialInitData materialInitData)
        {
            this.isPlayerGround = isPlayerGround;
            TileSort(materialInitData,selectedTheme);
            EventManager.Instance.on(EVENT_TYPE.TOWER_FIRE,TileTargeting);
        }

        public Tile GetTile(int index)
        {
            if (tileList[index])
            {
                return tileList[index];
            }
            Debug.Log("GetTile Fail");
            return null;
        }
        #endregion

        #region Private Methods

        private void TileTargeting(EVENT_TYPE eventType, Component sender, object param = null)
        {
            if ((Tower)sender)
            {
                Tower tower = (Tower)sender;
                if (tower.TowerData.isPlayerGround == isPlayerGround) { return; }
                RandomTargeting(tower);
            }
        }

        private void RandomTargeting(Tower tower)
        {
            var towerData = tower.TowerData;
            switch (towerData.towerKind)
            {
                case TowerKind.PARABOLA:
                    Tile tile = GetTile(RandomNum(towerData.tileIndex));
                    tower.Targeting(tile);
                    break; 
                case TowerKind.STRAIGHT:
                    tile = GetTile(isPlayerGround ? tower.TowerData.tileIndex % 5: tower.TowerData.tileIndex % 5 + 20);
                    tower.Targeting(tile);
                    List<Tile> routes = new List<Tile>();
                for (int i = 0; i < 5; ++i)
                {
                    Tile route;
                    if (isPlayerGround)
                    {
                        route = tileList[tile.tileData.index + 5 * i];
                    }
                    else
                    {
                        route = tileList[(tile.tileData.index % 5) + 5 * i];
                    }
                    route.TargetingSetActive(TowerKind.STRAIGHT,true);
                    routes.Add(route);
                    }
                    
                    break;
                case TowerKind.SCOPE:
                    break;
            }
        }
        private int RandomNum(int num)
        {
            List<int> numList = new List<int>();
            if (num == 0 || num % 5 == 0)
            {
                numList.Add(num + 1);
                numList.Add(num - 5);
                numList.Add(num + 5);
                numList.Add(num + 6);
                numList.Add(num - 4);
            }
            else if (num == 4 || (num - 4) % 5 == 0)
            {
                numList.Add(num - 1);
                numList.Add(num - 5);
                numList.Add(num + 5);
                numList.Add(num - 6);
                numList.Add(num + 4);
            }
            else
            {
                numList.Add(num - 1);
                numList.Add(num + 1);
                numList.Add(num - 5);
                numList.Add(num + 5);
                numList.Add(num - 6);
                numList.Add(num + 6);
                numList.Add(num - 4);
                numList.Add(num + 4);
            }
            numList.Add(num);
            for (int i = numList.Count - 1; i >= 0; i--)
            {
                if (numList[i] > 24 || numList[i] < 0)
                {
                    numList.RemoveAt(i);
                }
            }
            var randomNum = Random.Range(0, numList.Count);
            return numList[randomNum];
        }


        private void GroundInstance(GameObject selectedTheme,Tile tile)
        {
            // 그라운드 인스턴스 
            GameObject ground = Instantiate(selectedTheme,  tile.transform.position, Quaternion.identity);
            ground.transform.SetParent(tile.transform);
            ground.SetActive(true);
        }
        private void TileSort(MaterialInitData initData,GameObject[] selectedTheme)
        {
            int index = 0;
            List<int> tileSponeIndex = new List<int>() { 0, 1, 2, 4, 9, 14, 22, 23, 24, 10, 15, 20 };
            // 타일 인스턴스 
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    int ran = Random.Range(0,selectedTheme.Length);
                    Vector3 pos = new Vector3(i + transform.position.x, 0.35f, j);
                    Tile tile = TileLoad(pos);
                    GroundInstance(selectedTheme[ran], tile);
                    TileData tileData;
                    if (tileSponeIndex.Contains(index))
                    {
                        // 남은 자원 숫자 
                        tileData = RandomMaterial(index, tileSponeIndex.Count, initData);
                        tileSponeIndex.Remove(index);
                    }
                    else
                    {
                        tileData = new TileData(isPlayerGround, index, 100, TileState.NORMAL);
                    }
                    tile.TileInit(tileData);
                    tileList.Add(tile);
                    index++;
                }
            }
        }
        private TileData RandomMaterial(int index, int tileSponeIndexCount, MaterialInitData initData)
        {
            TileData tileData;
            int currentRemainingMaterial = initData.wood.count + initData.stone.count + initData.iron.count + initData.adam.count;
            // 남은 자원의 숫자가 들어갈 자원의 숫자와 같으면  
            if (currentRemainingMaterial == tileSponeIndexCount) // 12 보다 클 수 없음
            {

                if (initData.wood.count > 0)
                {
                    tileData = new TileData(isPlayerGround, index, initData.wood.amount, TileState.MATERIAL, MaterialState.WOOD);
                    initData.wood.count--;
                }
                else if (initData.stone.count > 0)
                {
                    tileData = new TileData(isPlayerGround, index, initData.stone.amount, TileState.MATERIAL, MaterialState.STONE);
                    initData.stone.count--;
                }
                else if (initData.iron.count > 0)
                {
                    tileData = new TileData(isPlayerGround, index, initData.iron.amount, TileState.MATERIAL, MaterialState.IRON);
                    initData.iron.count--;
                }
                else if (initData.adam.count > 0)
                {
                    tileData = new TileData(isPlayerGround, index, initData.adam.amount, TileState.MATERIAL, MaterialState.ADAM);
                    initData.adam.count--;
                }
                else
                {
                    tileData = new TileData(isPlayerGround, index, 100, TileState.NORMAL);
                }
            }
            else
            {
                int ran = Random.Range(0, 2); // Normal 인지 Material 인지 
                if (ran == 1)
                { // material 이면 
                    int materialRan = Random.Range(0, 4);
                    if (materialRan == 0 && initData.wood.count > 0)
                    {
                        tileData = new TileData(isPlayerGround, index, initData.wood.amount, TileState.MATERIAL, MaterialState.WOOD);
                        initData.wood.count--;
                    }
                    else if (materialRan == 1 && initData.stone.count > 0)
                    {
                        tileData = new TileData(isPlayerGround, index, initData.wood.amount, TileState.MATERIAL, MaterialState.STONE);
                        initData.stone.count--;
                    }
                    else if (materialRan == 2 && initData.iron.count > 0)
                    {
                        tileData = new TileData(isPlayerGround, index, initData.wood.amount, TileState.MATERIAL, MaterialState.IRON);
                        initData.iron.count--;
                    }
                    else if (materialRan == 3 && initData.adam.count > 0)
                    {
                        tileData = new TileData(isPlayerGround, index, initData.wood.amount, TileState.MATERIAL, MaterialState.ADAM);
                        initData.adam.count--;
                    }
                    else
                    {
                        tileData = new TileData(isPlayerGround, index, 100, TileState.NORMAL);
                    }

                }
                else
                {
                    tileData = new TileData(isPlayerGround, index, 100, TileState.NORMAL);
                }
            }

            return tileData;
        }
        private Tile TileLoad(Vector3 pos)
        {
            Tile tile = Instantiate(this.tile, pos, Quaternion.identity).GetComponent<Tile>();
            tile.transform.SetParent(transform);
            tile.gameObject.SetActive(true);
            return tile;
        }
        #endregion
    }
}

