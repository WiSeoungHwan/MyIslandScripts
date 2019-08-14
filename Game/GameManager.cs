using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
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
    [SerializeField]
    private GameObject gameoverUIPanel;

    public bool isGameOver{ get; set; }

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

    
    #region Private Methods
    private void GameInit(){
        // Ground Init
        playerGround.InitGround(true);
        enemyGround.InitGround(false);
        
        // Player Init
        playerUnitManager.PlayerInit(playerGround);
        enemyUnitManager.PlayerInit(enemyGround);

        StartCoroutine("OneSecTimer");
    }

    private void IsPlayerHit(Tile tile){
        if (tile.tileData.isMine){ // 적이 쏜거 - 내 타일 
            var playerData = playerUnitManager.GetPlayerData();
            if (playerData.playerIndex == tile.tileData.index){
                playerUnitManager.UnitHit(5);
                Debug.Log("2P가 쏜 포탄에 1P가 맞았습니다.");
            }
        }else{ // 내가 쏜거 - 적타일 
            var playerData = enemyUnitManager.GetPlayerData();
            if (playerData.playerIndex == tile.tileData.index){
                enemyUnitManager.UnitHit(5);
                Debug.Log("1p가 쏜 포탄에 2P가 맞았습니다.");
            }
        }
    }

    

    private void NotiTest(TileData tileData){
        Debug.Log("NotiTest: " + tileData.isMine + tileData.index);
    }

    #endregion

    #region Public Methods

    public void GameOver(){
        gameoverUIPanel.SetActive(true);
        isGameOver = true;
    }
    
    public Ground GetGroundData(bool isMine){
        return isMine ? enemyGround : playerGround;
    }

    IEnumerator OneSecTimer(){
        yield return new WaitForSeconds(1);
        currentTime--;
        if (currentTime <= 0){ // 턴이 끝나는 상황 
            currentTurn--;
            playerUnitManager.ResetPlayerActiveCount();
            enemyUnitManager.ResetPlayerActiveCount();
            playerGround.BroadcastMessage("Fire",null,SendMessageOptions.DontRequireReceiver);
            enemyGround.BroadcastMessage("Fire",null,SendMessageOptions.DontRequireReceiver);
            currentTime = ConstData.playTime;
        }
        this.timeText.text = "Time: "  + currentTime;
        this.turnText.text = "Turn: "  + currentTurn;
        StartCoroutine("OneSecTimer");
    }

    public void ReGame(){
        SceneManager.LoadScene(2);
    }

    #endregion

}
