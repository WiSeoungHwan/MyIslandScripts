using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MyIsland
{
    public enum TileState
    {
        NORMAL,
        MATERIAL,
        BUILDING
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
        public TileData tileData{get; set;}

        #endregion

        #region Private Field
        private GameObject materialObj;
        private TileState beforeTileState;
        #endregion

        #region MonoBehaviour Callbacks
        void OnMouseDown(){
            if(willBuildObject.activeInHierarchy){
                EventManager.Instance.emit(EVENT_TYPE.WILL_BUILD_OFF,this);
                
                willBuildObject.SetActive(false);
                tileData.tileState = TileState.BUILDING;
            }
        }
        #endregion

        #region Public Methods
        public void TileInit(TileData tileData)
        {
            this.tileData = tileData;
            beforeTileState = tileData.tileState;
            willBuildObject.SetActive(false);
            EventManager.Instance.on(EVENT_TYPE.WILL_BUILD_OFF, WillBuildObjectOff);
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
                materialObj.transform.position = new Vector3(transform.position.x, 0f,transform.position.z);
                materialObj.SetActive(true);
                tileUI.TileUISetActive(true);
                tileUI.TileUIUpdate(tileData.hp);
            }else{
                tileUI.TileUISetActive(false);
            }
        }

        public void WillBuildTower(TowerPoolList towerKey){
            willBuildObject.SetActive(true);
            beforeTileState = tileData.tileState;
            tileData.tileState = TileState.BUILDING;
        }
        
        public void WillBuildTable(TablePoolList towerKey){
            willBuildObject.SetActive(true);
            beforeTileState = tileData.tileState;
            onBuildingObject = TableObjectPool.Instance.Pop(towerKey).GetComponent<Building>();
            onBuildingObject.gameObject.SetActive(true);
            onBuildingObject.transform.position = Vector3.zero;
            tileData.tileState = TileState.BUILDING;
        }
        public void WillBuildBunker(BunkerPoolList towerKey){
            willBuildObject.SetActive(true);
            beforeTileState = tileData.tileState;
            tileData.tileState = TileState.BUILDING;
        }
        

        public bool TileHurt(int demage){
            if(tileData.hp <= 0){
                MaterialObjectPool.Instance.Remove(tileData.materialState,materialObj);
                tileData.tileState = TileState.NORMAL;
                tileUI.TileUISetActive(false);
                return false;
            }
            tileData.hp -= demage;
            tileUI.TileUIUpdate(tileData.hp);
            return true;
        }
        #endregion

        #region Private Methods
        private void WillBuildObjectOff(EVENT_TYPE eventType, Component sender, object param = null){
            willBuildObject.SetActive(false);

            tileData.tileState = beforeTileState;
        }
        #endregion
    }
}
