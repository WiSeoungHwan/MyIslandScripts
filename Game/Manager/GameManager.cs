using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyIsland
{
    public class GameManager : SingletonMonoBehaviour<GameManager>
    {
        #region Serialize Field
        // 그라운드의 테마 
        [SerializeField]
        private GameObject selecetedTheme;

        // 자원 데이터 커스터마이징 
        [SerializeField]
        private MaterialInitCount materialInitCount;
        [SerializeField]
        private MaterialInitCount materialInitAmount;
        
        // player 진영 
        [SerializeField]
        private UnitController playerController;
        // enemy 진영 
        [SerializeField]
        private UnitController enemyController;
        #endregion


        #region MonoBehaviour CallBacks
        void Start(){
            GameInit();
        }
        #endregion

        #region Public Methods
        public void GameInit(){
            MaterialInitData playerMaterialInitData = new MaterialInitData(){
                wood = new MaterialData(){
                    state = MaterialState.WOOD,
                    count = materialInitCount.woodCount,
                    amount = materialInitAmount.woodCount
                },
                stone = new MaterialData(){
                    state = MaterialState.STONE,
                    count = materialInitCount.stoneCount,
                    amount = materialInitAmount.stoneCount
                },
                iron = new MaterialData(){
                    state = MaterialState.IRON,
                    count = materialInitCount.ironCount,
                    amount = materialInitAmount.ironCount
                },
                adam = new MaterialData(){
                    state = MaterialState.ADAM,
                    count = materialInitCount.adamCount,
                    amount = materialInitAmount.adamCount
                },
            };
            MaterialInitData EnemyMaterialInitData = new MaterialInitData(){
                wood = new MaterialData(){
                    state = MaterialState.WOOD,
                    count = materialInitCount.woodCount,
                    amount = materialInitAmount.woodCount
                },
                stone = new MaterialData(){
                    state = MaterialState.STONE,
                    count = materialInitCount.stoneCount,
                    amount = materialInitAmount.stoneCount
                },
                iron = new MaterialData(){
                    state = MaterialState.IRON,
                    count = materialInitCount.ironCount,
                    amount = materialInitAmount.ironCount
                },
                adam = new MaterialData(){
                    state = MaterialState.ADAM,
                    count = materialInitCount.adamCount,
                    amount = materialInitAmount.adamCount
                },
            };
            playerController.UnitControllerInit(true, selecetedTheme, playerMaterialInitData);
            enemyController.UnitControllerInit(false, selecetedTheme,EnemyMaterialInitData);
        }
        #endregion

        
    }

}
