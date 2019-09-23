using System.Collections;
using System.Collections.Generic;
using UnityEngine;




namespace MyIsland_InGame
{
    public enum UnitState
    {
        IDLE,
        MOVE,
        COLLECT,
        BUILD,
        HURT
    }
    public enum EnemyBuildingIndex{
        NONE,
        TOWER_1,
        TOWER_2,
        TOWER_3,
        BUNKER,
        TABLE
    }
    public class UnitControl : MonoBehaviour
    {
        #region Delegete
        public delegate void Move(int clickTileIndex);
        public delegate void Collect(MaterialState materialState, int clickTileIndex);
        public delegate void Build(Tile tile);
        public delegate void BuildTap(Tile tile);
        public delegate void EnemyInput(int inputIndex);
        public delegate void EnemyBuildingInput(EnemyBuildingIndex buildingIndex);
        public delegate void EnemyLevelUp();

        private Move move = null;
        private Collect collect = null;
        private Build build = null;
        private BuildTap buildTap = null;
        private EnemyInput enemyInput = null;
        private EnemyBuildingInput enemyBuildingInput = null;
        private EnemyLevelUp enemyLevelUp = null;
        #endregion

        #region Serialize Field
        [SerializeField]
        private GameObject body;
        #endregion 

        #region Private Fields
        private UnitState unitState;
        private bool isPlayerGround;
        #endregion

        #region Public Methods
        public void SetDelegate(Move move, Collect collect, Build build, BuildTap buildTap, bool isPlayerGround, EnemyInput enemyInput, EnemyBuildingInput enemyBuildingInput,EnemyLevelUp enemyLevelUp)
        {
            this.move = move;
            this.collect = collect;
            this.build = build;
            this.buildTap = buildTap;
            this.isPlayerGround = isPlayerGround;
            this.enemyInput = enemyInput;
            this.enemyBuildingInput = enemyBuildingInput;
            this.enemyLevelUp = enemyLevelUp;
        }

        public void BodyActive(bool active)
        {
            body.SetActive(active);
        }
        #endregion

        #region Private Methods

        private void UnitAction()
        {
            if (isPlayerGround)
            { /// 1p
                if (Input.GetMouseButtonDown(0))
                {
                    // get hitInfo
                    RaycastHit hitInfo;
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    if (Physics.Raycast(ray, out hitInfo))
                    {
                        if (hitInfo.transform.gameObject.GetComponent<Tile>())
                        {// 타일 클릭했을때 
                            Tile tile = hitInfo.transform.gameObject.GetComponent<Tile>();
                            if (!tile.tileData.isPlayerGround) { return; }
                            body.transform.LookAt(new Vector3(tile.transform.position.x,1f,tile.transform.position.z));
                            switch (tile.tileData.tileState)
                            {
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
            else
            { // 2p
                EnemyInputIndex();
            }

        }

        private void BodyRotate(Vector3 pos){
            // if(pos.x < this.transform.lo)
        }

        #endregion

        #region Enemy Methods
        private void EnemyInputIndex()
        {
            int inputIndex = 0;
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                inputIndex = -5;
            }
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                inputIndex = 5;
            }
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                inputIndex = 1;
                
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                inputIndex = -1;
            }
            enemyInput(inputIndex);
            EnemyBuildingIndex buildingIndex = EnemyBuildingIndex.NONE;
            if (Input.GetKeyDown("`")){
                buildingIndex = EnemyBuildingIndex.NONE;
                enemyBuildingInput(buildingIndex);
            }
            else if (Input.GetKeyDown("1")){
                buildingIndex = EnemyBuildingIndex.TOWER_1;
                enemyBuildingInput(buildingIndex);
            }
                
            else if (Input.GetKeyDown("2")){
                buildingIndex = EnemyBuildingIndex.TOWER_2;
                enemyBuildingInput(buildingIndex);
            }
            else if (Input.GetKeyDown("3")){
                buildingIndex = EnemyBuildingIndex.TOWER_3;
                enemyBuildingInput(buildingIndex);
            }
            else if (Input.GetKeyDown("4")){
                buildingIndex = EnemyBuildingIndex.BUNKER;
                enemyBuildingInput(buildingIndex);
            }
            else if (Input.GetKeyDown("5")){
                buildingIndex = EnemyBuildingIndex.TABLE;
                enemyBuildingInput(buildingIndex);
            }
            if(Input.GetKeyDown(KeyCode.Q)){
                enemyLevelUp();
            }
                
        }
        #endregion

        #region MonoBehaviour CallBacks
        void Update()
        {
            UnitAction();
        }
        #endregion

    }
}

