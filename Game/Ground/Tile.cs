using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MyIsland
{
    public enum TileState
    {
        NORMAL,
        MATERIAL,
        BUILDING,
        WILL_BUILD
    }
    public class Tile : MonoBehaviour
    {
        #region Serialize Field
        [SerializeField]
        private TileUI tileUI;
        [SerializeField]
        private GameObject willBuildObject;
        private Building onBuildingObject;
        #endregion
        #region Property
        public TileData tileData { get; set; }

        #endregion

        #region Private Field
        private GameObject materialObj;
        private TileState beforeTileState;
        private bool buildingOn = false;
        private int key;
        #endregion

        #region MonoBehaviour Callbacks
        void OnMouseDown()
        {
            
        }
        #endregion

        #region Public Methods
        public void TileInit(TileData tileData)
        {
            this.tileData = tileData;
            beforeTileState = tileData.tileState;
            willBuildObject.SetActive(false);
            if (tileData.tileState == TileState.MATERIAL)
            {

                switch (tileData.materialState)
                {
                    case MaterialState.WOOD:
                        materialObj = MaterialObjectPool.Instance.Pop(MaterialState.WOOD);
                        break;
                    case MaterialState.STONE:
                        materialObj = MaterialObjectPool.Instance.Pop(MaterialState.STONE);
                        break;
                    case MaterialState.IRON:
                        materialObj = MaterialObjectPool.Instance.Pop(MaterialState.IRON);
                        break;
                    case MaterialState.ADAM:
                        materialObj = MaterialObjectPool.Instance.Pop(MaterialState.ADAM);
                        break;
                    default:
                        materialObj = MaterialObjectPool.Instance.Pop(MaterialState.WOOD);
                        break;
                }
                materialObj.transform.position = new Vector3(transform.position.x, 1f, transform.position.z);
                materialObj.SetActive(true);
                tileUI.TileUISetActive(true);
                tileUI.TileUIUpdate(tileData.hp);
            }
            else
            {
                tileUI.TileUISetActive(false);
            }
        }

        public void Build(){
            willBuildObject.SetActive(false);
            tileData.tileState = TileState.BUILDING;
            tileUI.TileUISetActive(true);
            tileData.hp = 10;
            tileUI.TileUIUpdate(tileData.hp);
        }

        public void BuildOff(){
            willBuildObject.SetActive(false);
            tileData.tileState = beforeTileState;
            if(onBuildingObject){
                switch(onBuildingObject.buildingKind){
                    case BuildingKind.TABLE:
                        TableObjectPool.Instance.Remove((TablePoolList)key, onBuildingObject.gameObject);
                        break;
                    case BuildingKind.TOWER:
                        TowerObjectPool.Instance.Remove((TowerPoolList)key, onBuildingObject.gameObject);
                        break;
                    case BuildingKind.BUNKER:
                        BunkerObjectPool.Instance.Remove((BunkerPoolList)key, onBuildingObject.gameObject);
                        break;
                }
            }
        }

        public void WillBuildTower(TowerPoolList towerKey)
        {
            var obj = TowerObjectPool.Instance.Pop(towerKey).GetComponent<Building>();
            WillBuildSetup((int)towerKey, obj);
        }

        public void WillBuildTable(TablePoolList towerKey)
        {
            var obj = TableObjectPool.Instance.Pop(towerKey).GetComponent<Building>();
            WillBuildSetup((int)towerKey, obj);
        }
        public void WillBuildBunker(BunkerPoolList towerKey)
        {
            var obj = BunkerObjectPool.Instance.Pop(towerKey).GetComponent<Building>();
            WillBuildSetup((int)towerKey, obj);
        }


        public bool TileHurt(int demage)
        {
            if (tileData.hp <= 0)
            {
                MaterialObjectPool.Instance.Remove(tileData.materialState, materialObj);
                tileData.tileState = TileState.NORMAL;
                tileUI.TileUISetActive(false);
                return false;
            }
            tileData.hp -= demage;
            tileUI.TileUIUpdate(tileData.hp);
            return true;
        }

        public BuildingKind GetBuildingKind(){
            return onBuildingObject.buildingKind;
        }
        #endregion

        #region Private Methods
        
        private void WillBuildSetup(int key, Building onBuildingObject){
            willBuildObject.SetActive(true);
            beforeTileState = tileData.tileState;
            this.onBuildingObject = onBuildingObject;
            this.onBuildingObject.gameObject.SetActive(true);
            this.onBuildingObject.transform.position = new Vector3(willBuildObject.transform.position.x, 1f, willBuildObject.transform.position.z);
            this.key = (int)key;
            tileData.tileState = TileState.WILL_BUILD;
        }
        #endregion
    }
}
