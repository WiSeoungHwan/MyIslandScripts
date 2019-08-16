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
    [SerializeField]
    private HealthBar stamina;

    #endregion
    #region Private Field
    private PlayerScript player;
    private PlayerData playerData;
    private Ground ground;
    private int willBuildIndex;

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

    public PlayerData GetPlayerData(){
        this.playerData.playerIndex =  player.GetPlayerIndex();
        return this.playerData;
    }
    public void PlayerInit(Ground ground)
    {
        // Ground set up 
        this.ground = ground;

        // Player Data set up 
        this.playerData = new PlayerData();
        this.playerData.hp = ConstData.playerHp;
        this.playerData.activeCount = ConstData.activeCount;
        this.playerData.materialData = new MaterialData();
        this.playerData.playerIndex = ConstData.playerInstanceIndex;
        unitUIController.UintUIUpdate(this.playerData);

        // Player Instance object
        var pos = isMine ? new Vector3(2, 0, 2) : new Vector3(-5, 0, 2);
        playerObject.transform.SetParent(transform);
        playerObject.transform.position = pos;
        this.player = playerObject.GetComponent<PlayerScript>();
        this.player.PlayerInit(ground, isMine, ConstData.playerInstanceIndex);
        this.player.SetCallBacks(SetUnitMatrialUI, UnitMoveCount, BuildingAreaTap,EnemyBuildTower);

        // Unit UI set up 
        unitUIController.UnitUIInit(this.player);
        unitUIController.SetCallbacks(TowerSelected);
        unitUIController.UintUIUpdate(playerData);

        // Stamina Bar set up 
        stamina.SetCallBacks(AddStamina);
    }


    public void ResetPlayerActiveCount()
    {
        // playerData.activeCount = ConstData.activeCount;
        // unitUIController.UintUIUpdate(this.playerData);
    }

    public void UnitHit(int damage){
        playerData.hp -= damage;
        if (playerData.hp <= 0){
            GameManager.Instance.GameOver();
            Debug.Log("GameOver");
        }
        unitUIController.UintUIUpdate(this.playerData);
        
    }


    #endregion

    #region Private Methods

    private void AddStamina()
    {
        if(playerData.activeCount   >= 5)
        {
            playerData.activeCount = 5;
            return;
        }
        playerData.activeCount++;
        Debug.Log("activeCount: " + playerData.activeCount);
    }

    private bool EnemyBuildTower(){
        if (playerData.activeCount > 0 && playerData.materialData.wood > 4) {
            playerData.materialData.wood -= 5;
            playerData.activeCount--;
            stamina.Hurt(1);
            unitUIController.UintUIUpdate(playerData);
            unitUIController.MaterialUIUpdate(playerData);
            return true;
        }else{
            return false;
        }
    }
    private bool UnitMoveCount()
    {
        if (playerData.activeCount <= 0)
        {
            // 카운트를 다 쓴 상황 
            unitUIController.UintUIUpdate(this.playerData);
            playerData.playerIndex = player.GetPlayerIndex();
            return false;
        }
        playerData.activeCount--;
        stamina.Hurt(1);
        playerData.playerIndex = player.GetPlayerIndex();
        unitUIController.UintUIUpdate(this.playerData);
        return true;
    }
    private void BuildingAreaTap(Vector3 clickPoint, int willBuildIndex)
    {
        this.willBuildIndex = willBuildIndex;
        unitUIController.BuildUIActive(clickPoint);
    }

    private void SetUnitMatrialUI(MaterialState materialKind, int num)
    {

        switch (materialKind)
        {
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

    private void TowerSelected(TowerState state){
        if(!ground.tileArr[willBuildIndex]) {Debug.Log("ground[willBuildIndex] tile is null"); return;}
        if(playerData.materialData.wood > 4){
            playerData.activeCount -= 1;
            stamina.Hurt(1);
            playerData.materialData.wood -= 5;
            unitUIController.MaterialUIUpdate(this.playerData);
            unitUIController.UintUIUpdate(this.playerData);
            Tile tile = ground.tileArr[willBuildIndex];
            tile.BuildTower(state);
        }else{
            Debug.Log("material is lack");
        }
        
    }
    #endregion
}
