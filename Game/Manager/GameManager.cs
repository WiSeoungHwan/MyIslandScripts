using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace MyIsland_InGame
{
    public class GameManager : SingletonMonoBehaviour<GameManager>
    {
        #region Serialize Field
        [SerializeField]
        private TowerSheet towerSheet;
        // 그라운드의 테마 
        [SerializeField]
        private GameObject[] selecetedTheme;

        // 자원 데이터 커스터마이징 
        [SerializeField]
        private MaterialInitCount materialInitCount;
        [SerializeField]
        private MaterialInitCount materialInitAmount;
        // Unit 시작 데이터 설정 
        [SerializeField]
        private UnitInitData unitInitData;
        // player 진영 
        [SerializeField]
        private UnitController playerController;
        // enemy 진영 
        [SerializeField]
        private UnitController enemyController;
        // gameOver 
        [SerializeField]
        private GameObject gameoverPanel;
        [SerializeField]
        private Text timeText;
        [SerializeField]
        private int playTime;
        [SerializeField]
        private Text fireTimeText;
        [SerializeField]
        private int fireTime;
        #endregion

        #region Private Field
        private int towerFireTime;
        private float curTime;
        private int min;
        private int sec;
        #endregion




        #region MonoBehaviour CallBacks
        protected override void OnStart()
        {
            GameInit();
            AudioManager.Instance.PlayBgm("Game_01");
            EventManager.Instance.on(EVENT_TYPE.GAMEOVER_UNIT_DIE, GameOverUnitDie);
            gameoverPanel.SetActive(false);
            towerFireTime = fireTime;
            curTime = playTime;
        }
        void OnDestroy(){
            EventManager.Instance.off(EVENT_TYPE.GAMEOVER_UNIT_DIE, GameOverUnitDie);
            AudioManager.Instance.StopBGM();
        }
        #endregion

        #region Public Methods
        public void GameInit()
        {
            MaterialInitData playerMaterialInitData = new MaterialInitData()
            {
                wood = new MaterialData()
                {
                    state = MaterialState.WOOD,
                    count = materialInitCount.woodCount,
                    amount = materialInitAmount.woodCount
                },
                stone = new MaterialData()
                {
                    state = MaterialState.STONE,
                    count = materialInitCount.stoneCount,
                    amount = materialInitAmount.stoneCount
                },
                iron = new MaterialData()
                {
                    state = MaterialState.IRON,
                    count = materialInitCount.ironCount,
                    amount = materialInitAmount.ironCount
                },
                adam = new MaterialData()
                {
                    state = MaterialState.ADAM,
                    count = materialInitCount.adamCount,
                    amount = materialInitAmount.adamCount
                },
            };
            MaterialInitData EnemyMaterialInitData = new MaterialInitData()
            {
                wood = new MaterialData()
                {
                    state = MaterialState.WOOD,
                    count = materialInitCount.woodCount,
                    amount = materialInitAmount.woodCount
                },
                stone = new MaterialData()
                {
                    state = MaterialState.STONE,
                    count = materialInitCount.stoneCount,
                    amount = materialInitAmount.stoneCount
                },
                iron = new MaterialData()
                {
                    state = MaterialState.IRON,
                    count = materialInitCount.ironCount,
                    amount = materialInitAmount.ironCount
                },
                adam = new MaterialData()
                {
                    state = MaterialState.ADAM,
                    count = materialInitCount.adamCount,
                    amount = materialInitAmount.adamCount
                },
            };
            playerController.UnitControllerInit(true, selecetedTheme, playerMaterialInitData, unitInitData);
            enemyController.UnitControllerInit(false, selecetedTheme, EnemyMaterialInitData, unitInitData);

            StartCoroutine("OneSecTimer");
        }

        IEnumerator OneSecTimer()
        {
            yield return new WaitForSeconds(1);
            towerFireTime--;
            curTime--;
            min = (int)curTime / 60;
            sec = (int)curTime % 60;
            timeText.text = min.ToString("00") + ":" + sec.ToString("00");
            fireTimeText.text = towerFireTime.ToString();
            if (towerFireTime <= 0)
            {
                towerFireTime = fireTime;
                EventManager.Instance.emit(EVENT_TYPE.GM_FIRE, this);
            }
            if(curTime < 1){
                gameoverPanel.SetActive(true);
            }
            StartCoroutine("OneSecTimer");
        }
        public void GameOverButtonTap(){
            SceneManager.LoadScene(2);
        }
        #endregion

        #region Private Method
        private void GameOverUnitDie(EVENT_TYPE eventType, Component sender, object param = null){
            bool isPlayer = (bool)param;
            gameoverPanel.SetActive(true);
        }
        #endregion
    }

}
