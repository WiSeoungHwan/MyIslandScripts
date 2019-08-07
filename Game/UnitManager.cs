using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    #region SerializeField
    
    [SerializeField]
    private bool isMine = true;

    #endregion
    #region Private Field
    private PlayerScript player;
    // Start is called before the first frame update
    #endregion


    #region MonoBehaviour Callbacks

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #endregion

    #region Public Methods
    public void PlayerInit(Ground ground){
        if(!Resources.Load("Player")){Debug.Log("Player prefab is not found");}
        var pos = isMine ? new Vector3(2,0,2) : new Vector3(-5,0,2);
        var playerObject = Instantiate((Resources.Load("Player") as GameObject),pos, Quaternion.identity);
        playerObject.transform.SetParent(transform);
        this.player = playerObject.GetComponent<PlayerScript>();
        this.player.PlayerInit(ground, isMine, ConstData.playerInstanceIndex);
    }
    #endregion
}
