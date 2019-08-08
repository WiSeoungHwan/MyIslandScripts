using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    #region SerializeField
    
    [SerializeField]
    private bool isMine = true;

    [SerializeField]
    private UnitUI unitUIController;
    [SerializeField]
    private GameObject playerObject;

    #endregion
    #region Private Field
    private PlayerScript player;
    private PlayerData playerData;

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
        // Player Data set up 
        this.playerData = new PlayerData();
        this.playerData.hp = ConstData.playerHp;
        this.playerData.activeCount = ConstData.activeCount;
        this.playerData.materialData = new MaterialData();
        unitUIController.UintUIUpdate(this.playerData);

        // Player Object Instantiate
        // if(!Resources.Load("Player")){Debug.Log("Player prefab is not found");}
        var pos = isMine ? new Vector3(2,0,2) : new Vector3(-5,0,2);
        // var playerObject = Instantiate((Resources.Load("Player") as GameObject),pos, Quaternion.identity);
        playerObject.transform.SetParent(transform);
        playerObject.transform.position = pos;
        this.player = playerObject.GetComponent<PlayerScript>();
        this.player.PlayerInit(ground, isMine, ConstData.playerInstanceIndex);
        this.player.SetCallBacks(SetUnitMatrialUI,UnitMoveCount);
    }

    public void ResetPlayerActiveCount(){
        playerData.activeCount = ConstData.activeCount;
        unitUIController.UintUIUpdate(this.playerData);
    }
    #endregion

    #region Private Methods

    private bool UnitMoveCount(){
        if(playerData.activeCount <= 0){
            // 카운트를 다 쓴 상황 
            unitUIController.UintUIUpdate(this.playerData);
            return false;
        }
        playerData.activeCount--;
        unitUIController.UintUIUpdate(this.playerData);
        return true;
    }

    private void SetUnitMatrialUI(MaterialState materialKind, int num){

        switch(materialKind){
            case MaterialState.wood:
                playerData.materialData.wood += num;
                break;
            case MaterialState.stone:
                playerData.materialData.stone += num;
                break;
            case MaterialState.iron:
                playerData.materialData.iron += num;
                break;
            case MaterialState.adamantium:
                playerData.materialData.adamantium += num;
                break;
            default:
                break;
        }
        unitUIController.MaterialUIUpdate(this.playerData);
    }
    #endregion
}
