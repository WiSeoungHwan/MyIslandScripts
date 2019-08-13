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
        playerGround.InitGround(true);
        enemyGround.InitGround(false);
        
        // Player Init
        playerUnitManager.PlayerInit(playerGround);
        enemyUnitManager.PlayerInit(enemyGround);

        StartCoroutine("OneSecTimer");
    }

    

    private void NotiTest(TileData tileData){
        Debug.Log("NotiTest: " + tileData.isMine + tileData.index);
    }
    #endregion

    #region Public Methods
    
    public Ground GetGroundData(bool isMine){
        return isMine ? playerGround : enemyGround;
    }

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
