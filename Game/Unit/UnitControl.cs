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
        public delegate void Build(Tile tile);
        public delegate void BuildTap(Tile tile);

        private Move move = null;
        private Collect collect = null;
        private Build build = null;
        private BuildTap buildTap = null;
        #endregion

        #region Serialize Field
        [SerializeField]
        private GameObject body;
        #endregion 

        #region Private Fields
        private UnitState unitState;
        #endregion

        #region Public Methods
        public void SetDelegate(Move move, Collect collect, Build build, BuildTap buildTap){
            this.move = move;
            this.collect = collect;
            this.build = build;
            this.buildTap = buildTap;
        }

        public void BodyActive(bool active){
            body.SetActive(active);
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
                        Tile tile = hitInfo.transform.gameObject.GetComponent<Tile>();
                        if(!tile.tileData.isPlayerGround){return;}
                        switch(tile.tileData.tileState){
                            case TileState.NORMAL:
                                move(tile.tileData.index);
                                break;
                            case TileState.MATERIAL:
                                collect(tile.tileData.materialState, tile.tileData.index);
                                break;
                            case TileState.BUILDING:
                                buildTap(tile);
                                break;
                            case TileState.WILL_BUILD:
                                build(tile);
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

