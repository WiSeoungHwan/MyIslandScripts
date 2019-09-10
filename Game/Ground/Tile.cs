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
        [SerializeField]
        private GameObject[] targetingMark;
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
            foreach(var i in targetingMark){
                i.SetActive(false);
            }
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
        public void TargetingSetActive(TowerKind towerKind, bool active){
            switch(towerKind){
                case TowerKind.PARABOLA:
                    targetingMark[0].SetActive(active);
                    break;
                case TowerKind.STRAIGHT:
                    targetingMark[1].SetActive(active);
                    break;
                case TowerKind.SCOPE:
                    targetingMark[2].SetActive(active);
                    break;
            }
        }

        public void Build(){
            willBuildObject.SetActive(false);
            tileData.tileState = TileState.BUILDING;
            tileUI.TileUISetActive(true);
            tileData.hp = 10;
            tileUI.TileUIUpdate(tileData.hp);
            switch(onBuildingObject.buildingKind){
                case BuildingKind.TOWER:
                    // Get Tower Component
                    Tower tower = onBuildingObject.GetComponent<Tower>();
                    tower.TowerInit(tileData.isPlayerGround, tileData.index);
                    tower.BuildComplete = true;
                    break;
            }
        }

        public void BuildOff(){
            willBuildObject.SetActive(false);
            if(beforeTileState == TileState.WILL_BUILD){
                tileData.tileState = TileState.NORMAL;
            }else{
                tileData.tileState = beforeTileState;
            }
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
                onBuildingObject = null;

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

        public void UpgradeTable(int key){
            TableObjectPool.Instance.Remove((TablePoolList)key - 1, onBuildingObject.gameObject);
            var obj = TableObjectPool.Instance.Pop((TablePoolList)key).GetComponent<Building>();
            WillBuildSetup(key,obj);
            tileData.tileState = TileState.BUILDING;
            willBuildObject.SetActive(false);
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

        public void ButtonUISetActive(UpgradeButtonTap upgrade){
            tileUI.SetDelegate(upgrade);
            tileUI.TablePanelSetActive();
        }
        #endregion

        #region Private Methods
        
        private void WillBuildSetup(int key, Building onBuildingObject){
            OnBuildingClear();
            willBuildObject.SetActive(true);
            this.onBuildingObject = onBuildingObject;
            this.onBuildingObject.gameObject.SetActive(true);
            this.onBuildingObject.transform.position = new Vector3(willBuildObject.transform.position.x, 1f, willBuildObject.transform.position.z);
            this.key = (int)key;
            tileData.tileState = TileState.WILL_BUILD;
        }

        private void OnBuildingClear(){
            beforeTileState = tileData.tileState;
            if(onBuildingObject){
                switch(onBuildingObject.buildingKind){
                    case BuildingKind.TABLE:
                        TableObjectPool.Instance.Remove((TablePoolList)key,onBuildingObject.gameObject);
                        break;
                    case BuildingKind.TOWER:
                        TowerObjectPool.Instance.Remove((TowerPoolList)key,onBuildingObject.gameObject);
                        break;
                    case BuildingKind.BUNKER:
                        BunkerObjectPool.Instance.Remove((BunkerPoolList)key,onBuildingObject.gameObject);
                        break;
                }
                onBuildingObject = null;
                willBuildObject.SetActive(false);
            }
        }

        
        #endregion
    }
}
