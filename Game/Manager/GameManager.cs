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
        private MaterialInitData materialInitData;
        
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
            playerController.UnitControllerInit(selecetedTheme, materialInitData);
            enemyController.UnitControllerInit(selecetedTheme,materialInitData);
        }
        #endregion

        
    }

}
