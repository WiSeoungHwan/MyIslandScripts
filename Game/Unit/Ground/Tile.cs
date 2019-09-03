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
        TileUI tileUI;
        #endregion
        #region Property
        public TileData tileData{get; set;}

        #endregion

        #region Private Field
        private GameObject obj;
        #endregion


        #region Public Methods
        public void TileInit(TileData tileData)
        {
            this.tileData = tileData;
            
            if (tileData.tileState == TileState.MATERIAL)
            {
                
                switch (tileData.materialState)
                {
                    case MaterialState.WOOD:
                        obj = MaterialObjectPool.Instance.Pop(MaterialState.WOOD);
                        break;
                    case MaterialState.STONE:
                        obj = MaterialObjectPool.Instance.Pop(MaterialState.STONE);
                        break;
                    case MaterialState.IRON:
                        obj = MaterialObjectPool.Instance.Pop(MaterialState.IRON);
                        break;
                    case MaterialState.ADAM:
                        obj = MaterialObjectPool.Instance.Pop(MaterialState.ADAM);
                        break;
                    default:
                        obj = MaterialObjectPool.Instance.Pop(MaterialState.WOOD);
                        break;
                }
                obj.transform.position = new Vector3(transform.position.x, 0f,transform.position.z);
                obj.SetActive(true);
                tileUI.TileUISetActive(true);
                tileUI.TileUIUpdate(tileData.hp);
            }else{
                tileUI.TileUISetActive(false);
            }
        }

        public bool TileHurt(int demage){
            
            if(tileData.hp <= 0){
                MaterialObjectPool.Instance.Remove(tileData.materialState,obj);
                tileData.tileState = TileState.NORMAL;
                tileUI.TileUISetActive(false);
                return false;
            }
            tileData.hp -= demage;
            tileUI.TileUIUpdate(tileData.hp);
            return true;
        }

        
        #endregion
    }
}
