using System.Collections;
using System.Collections.Generic;
using UnityEngine;




namespace MyIsland
{
    public enum UnitState{
        IDLE,
        MOVE,
        COLLECT,
        BUILD,
        HURT
    }
    public class UnitControl : MonoBehaviour
    {
        #region Delegete
        public delegate void Move(int clickTileIndex);
        public delegate void Collect(MaterialState materialState, int clickTileIndex);

        private Move move = null;
        private Collect collect = null;
        #endregion

        #region Serialize Field
        [SerializeField]
        private GameObject body;
        #endregion 

        #region Private Fields
        private UnitState unitState;
        #endregion

        #region Public Methods
        public void SetDelegate(Move move, Collect collect){
            this.move = move;
            this.collect = collect;
        }
        #endregion

        #region Private Methods

        private void UnitAction(){
             if (Input.GetMouseButtonDown(0))
            {
                // get hitInfo
                RaycastHit hitInfo;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hitInfo))
                {
                    if(hitInfo.transform.gameObject.GetComponent<Tile>()){// 타일 클릭했을때 
                        TileData tileData = hitInfo.transform.gameObject.GetComponent<Tile>().tileData;
                        if(!tileData.isPlayerGround){return;}
                        switch(tileData.tileState){
                            case TileState.NORMAL:
                                move(tileData.index);
                                break;
                            case TileState.MATERIAL:
                                collect(tileData.materialState, tileData.index);
                                break;
                            case TileState.BUILDING:
                                break;
                        }
                        
                    }
                }
            }
        }
        
        #endregion

        #region MonoBehaviour CallBacks
        void Update(){
            UnitAction();
        }
        #endregion

    }
}

