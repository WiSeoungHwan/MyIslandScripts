using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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


    }
    #endregion

    #region Public Methods
    #endregion

}
