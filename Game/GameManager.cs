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
    #endregion

    #region MonoBehaviour Callbacks
    protected override void OnAwake(){
        GameInit();
    }
    #endregion

    
    #region private Methods
    private void GameInit(){
        playerGround.InitGround();
        enemyGround.InitGround();
    }
    #endregion

    #region Public Methods
    #endregion

}
