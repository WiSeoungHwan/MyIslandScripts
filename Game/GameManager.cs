using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GameManager : SingletonMonoBehaviour<GameManager>
{

    #region SerializeField
    [SerializeField]
    private Ground playerGround;
    [SerializeField]
    private Ground enemyGround;
    [SerializeField]
    private UnitManager playerUnitManager;
    [SerializeField]
    private UnitManager enemyUnitManager;
    [SerializeField]
    private Text turnText;
    [SerializeField]
    private Text timeText;
    #endregion

    #region Private Field
    private int currentTurn = ConstData.totalTurn;
    private int currentTime = ConstData.playTime;
    #endregion

    #region MonoBehaviour Callbacks
    protected override void OnAwake(){
        GameInit();
    }
    #endregion

    
    #region private Methods
    private void GameInit(){
        // Ground Init
        playerGround.InitGround();
        enemyGround.InitGround();
        
        // Player Init
        playerUnitManager.PlayerInit(playerGround);
        enemyUnitManager.PlayerInit(enemyGround);

        StartCoroutine("OneSecTimer");
    }
    #endregion

    #region Public Methods

    IEnumerator OneSecTimer(){
        yield return new WaitForSeconds(1);
        currentTime--;
        if (currentTime <= 0){ // 턴이 끝나는 상황 
            currentTurn--;
            playerUnitManager.ResetPlayerActiveCount();
            enemyUnitManager.ResetPlayerActiveCount();
            currentTime = ConstData.playTime;
        }
        this.timeText.text = "Time: "  + currentTime;
        this.turnText.text = "Turn: "  + currentTurn;
        StartCoroutine("OneSecTimer");
    }

    #endregion

}
