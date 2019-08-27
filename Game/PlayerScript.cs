using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerScript : MonoBehaviour
{

    // MARK: - Private Fields

    [SerializeField]
    GameObject[] practicableAreas;
    [SerializeField]
    GameObject[] moveArea;

    [SerializeField]
    Material normalMaterial;
    [SerializeField]
    Material moveMaterial;
    [SerializeField]
    GameObject body;

    private Ground ground;

    private int playerIndex = 0;
    private int enemyIndex = 0;

    private bool isMine;
    private bool isEnemyBuildMode;
    private int enemyCurrentBuilding = 1;// 기본 포물선 타워 
    private TowerLevelHandler towerLevelHandler;
    private Tile bunker;
    
    #region Delegate

    public delegate void MaterialHit(MaterialState materialState, int num);
    public delegate bool MoveCount();
    public delegate void BuildingAreaTap(Vector3 clickPoint, int groundIndex);
    public delegate bool EnemyBuildTower(bool isTable);

    private MaterialHit materialHit = null;
    private MoveCount moveCount = null;
    private BuildingAreaTap buildingAreaTap = null;
    private EnemyBuildTower enemyBuildTower = null;
    #endregion
    // MARK: - Public Fields

    // MARK: - MonoBehaviour

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.Instance.isGameOver){return;}
        PlayerAction();
    }

    // MARK: - Public Methods 

    public void PlayerInit(Ground ground, bool isMine, int startIndex, TowerLevelHandler towerLevelHandler)
    {
        this.isMine = isMine;
        this.ground = ground;
        this.playerIndex = startIndex;
        this.towerLevelHandler = towerLevelHandler;
        CheckAroundMove();
    }

    public void SetCallBacks(MaterialHit materialHit, MoveCount moveCount, BuildingAreaTap buildingAreaTap, EnemyBuildTower enemyBuildTower)
    {
        this.materialHit = materialHit;
        this.moveCount = moveCount;
        this.buildingAreaTap = buildingAreaTap;
        this.enemyBuildTower = enemyBuildTower;
    }



    // MARK: - Pirvate Methods

    private void PlayerAction()
    {
        if (isMine)
        {
            // 1P
            // click the ground 
            if (Input.GetMouseButtonDown(0))
            {
                // get hitInfo
                RaycastHit hitInfo;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hitInfo))
                {
                    if (hitInfo.transform.gameObject.GetComponent<Tile>()) // 타일을 클릭했을때 
                    {
                        Tile tile = hitInfo.transform.gameObject.GetComponent<Tile>();
                        if (!tile.tileData.isMine) { return; } // 적의 타일을 클릭시 
                        if (tile.tileData.index == playerIndex + 5 || tile.tileData.index == playerIndex - 5 || tile.tileData.index == playerIndex + 1 || tile.tileData.index == playerIndex - 1)
                        {
                            
                            switch (tile.tileData.tileState)
                            {
                                case TileState.normal:
                                    if (!moveCount()) { return; }
                                    bunker = null;
                                    Move(tile);
                                    body.SetActive(true);
                                    break;
                                case TileState.material:
                                    if(bunker != null){return;}
                                    if (!moveCount()) {return;}
                                    Collect(tile);
                                    break;
                                case TileState.building:
                                    if(tile.tileData.towerState == TowerState.bunker){
                                        if(!moveCount()){return;}
                                        bunker = tile;
                                        Move(tile);
                                        body.SetActive(false);
                                    }
                                    
                                // 건물 눌렀을때
                                    break;
                            }
                            CheckAroundMove();
                        }
                    }
                    else if (hitInfo.transform.tag == ConstData.practicableAreaTag)
                    {
                        var willBuildIndex = 0;
                        switch (hitInfo.transform.name)
                        {
                            case "X1":
                                willBuildIndex = playerIndex + 5;
                                break;
                            case "X-1":
                                willBuildIndex = playerIndex - 5;
                                break;
                            case "Z1":
                                willBuildIndex = playerIndex + 1;
                                break;
                            case "Z-1":
                                willBuildIndex = playerIndex - 1;
                                break;
                        }
                        buildingAreaTap(hitInfo.transform.position, willBuildIndex);
                    }
                }
            }
        }
        else
        {
            //2P
            EnemyInput();
        }
    }

    

    private void Collect(Tile tile)
    {
        tile.MaterialHit(ConstData.materialDamage);
        materialHit(tile.tileData.materialState, ConstData.materialDamage);
        //CheckAround();
    }
    private void Move(Tile tile)
    {
        var pos = tile.transform.position;
        gameObject.transform.position = new Vector3(pos.x, transform.position.y, pos.z);
        playerIndex = tile.tileData.index;

    }


    private void EnemyInput()
    {
        int inputIndex = 0;
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            inputIndex = -5;
            EnemyAction(inputIndex);
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            inputIndex = 5;
            EnemyAction(inputIndex);
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            inputIndex = 1;
            if ((playerIndex + inputIndex) % 5 == 0) { return; }
            EnemyAction(inputIndex);
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (playerIndex % 5 == 0) { return; }
            inputIndex = -1;
            EnemyAction(inputIndex);
        }
        EnemyBuilding();

    }
    private void EnemyBuilding()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            if (!isEnemyBuildMode)
            {
                isEnemyBuildMode = true;
                // CheckAround();
            }
            else
            {
                isEnemyBuildMode = false;
                BuildModeOff();
            }
        }
        if (isEnemyBuildMode)
        {
            if (Input.GetKeyDown("1"))
            {
                enemyCurrentBuilding = 1;
            }
            if (Input.GetKeyDown("2"))
            {
                enemyCurrentBuilding = 2;
            }
            if (Input.GetKeyDown("3"))
            {
                enemyCurrentBuilding = 3;
            }
            if(Input.GetKeyDown("4")){
                enemyCurrentBuilding = 4;
            }
            if(Input.GetKeyDown("5")){
                enemyCurrentBuilding = 5;
            }
            
            int inputIndex = 0;
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                inputIndex = -5;
                BuildingEnemy(inputIndex);
            }
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                inputIndex = 5;
                BuildingEnemy(inputIndex);
            }
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                inputIndex = 1;
                if ((playerIndex + inputIndex) % 5 == 0) { return; }
                BuildingEnemy(inputIndex);
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                if (playerIndex % 5 == 0) { return; }
                inputIndex = -1;
                BuildingEnemy(inputIndex);
            }
        }

    }

    private void EnemyAction(int inputIndex)
    {
        if (isEnemyBuildMode) { return; }
        if (playerIndex + inputIndex < 0 || playerIndex + inputIndex > 24) { return; }
        Tile tile = ground.tileArr[playerIndex + inputIndex];
        switch (tile.tileData.tileState)
        {
            case TileState.normal:
                if (moveCount())
                {
                    playerIndex += inputIndex;
                    gameObject.transform.position = new Vector3(tile.transform.position.x, transform.position.y, tile.transform.position.z);
                }
                break;
            case TileState.material:
                if (tile.tileData.index == playerIndex + 5 || tile.tileData.index == playerIndex - 5 || tile.tileData.index == playerIndex + 1 || tile.tileData.index == playerIndex - 1)
                {
                    if (moveCount())
                    {
                        tile.MaterialHit(ConstData.materialDamage);
                        materialHit(tile.tileData.materialState, ConstData.materialDamage);
                    }
                }
                break;
            case TileState.building:
                Debug.Log("building");
                break;
        }
    }
    private void BuildingEnemy(int inputIndex){
        if (playerIndex + inputIndex < 0 || playerIndex + inputIndex > 24) { return; }
            Tile tile = ground.tileArr[playerIndex + inputIndex];
        
            if(tile.tileData.tileState == TileState.normal){
                if(enemyCurrentBuilding == 4 && enemyBuildTower(true)){
                    tile.BuildTower(TowerState.table, towerLevelHandler.GetTable(0));
                }else if(enemyBuildTower(false)){
                    tile.BuildTower((TowerState)enemyCurrentBuilding, towerLevelHandler.GetTower(0));
                }
                this.BuildModeOff();
                isEnemyBuildMode = false;
            }
            
            
    }

    public int GetPlayerIndex(){
        return playerIndex;
    }

    public void BuildModeOff()
    {
        foreach (GameObject i in practicableAreas)
        {
            i.SetActive(false);
        }
    }

    public void CheckAroundMove(){
        foreach (GameObject i in moveArea)
        {
            int aroundIndex = 0;
            switch (i.transform.name)
            {
                case "X1":
                    aroundIndex = playerIndex + 5;
                    break;
                case "X-1":
                    aroundIndex = playerIndex - 5;
                    break;
                case "Z1":
                    if (playerIndex == 4 || (playerIndex - 4) % 5 == 0)
                    { // right side
                        i.SetActive(false);
                        continue;
                    }
                    aroundIndex = playerIndex + 1;
                    break;
                case "Z-1":
                    if (playerIndex == 0 || playerIndex % 5 == 0)
                    { // left side
                        i.SetActive(false);
                        continue;
                    }
                    aroundIndex = playerIndex - 1;
                    break;
                default:
                    Debug.Log("Tile not found");
                    break;
            }
            if (aroundIndex < 0 || aroundIndex > 24)
            {  // start tile or end tile
                i.SetActive(false);
                continue;
            }

            if (ground.tileArr[aroundIndex] == null) { continue; }
            Tile tile = ground.tileArr[aroundIndex];
            switch (tile.tileData.tileState)
            {
                case TileState.normal:
                    i.SetActive(true);
                    i.GetComponent<MeshRenderer>().material = moveMaterial;
                    break;
                case TileState.material:
                    i.SetActive(true);
                    i.GetComponent<MeshRenderer>().material.color = Color.green;
                    break;
                case TileState.building:
                    if(tile.tileData.towerState == TowerState.bunker){
                        i.GetComponent<MeshRenderer>().material.color = Color.blue;
                        i.SetActive(true);
                        break;
                    }
                    i.SetActive(false);
                    break;
            }
        }
    }


    public void CheckAround(GameObject willBuild)
    {
        foreach (GameObject i in practicableAreas)
        {
            int aroundIndex = 0;
            switch (i.transform.name)
            {
                case "X1":
                    aroundIndex = playerIndex + 5;
                    break;
                case "X-1":
                    aroundIndex = playerIndex - 5;
                    break;
                case "Z1":
                    if (playerIndex == 4 || (playerIndex - 4) % 5 == 0)
                    { // right side
                        i.SetActive(false);
                        continue;
                    }
                    aroundIndex = playerIndex + 1;
                    break;
                case "Z-1":
                    if (playerIndex == 0 || playerIndex % 5 == 0)
                    { // left side
                        i.SetActive(false);
                        continue;
                    }
                    aroundIndex = playerIndex - 1;
                    break;
                default:
                    Debug.Log("Tile not found");
                    break;
            }
            if (aroundIndex < 0 || aroundIndex > 24)
            {  // start tile or end tile
                i.SetActive(false);
                continue;
            }

            if (ground.tileArr[aroundIndex] == null) { continue; }
            Tile tile = ground.tileArr[aroundIndex];
            switch (tile.tileData.tileState)
            {
                case TileState.normal:
                    for(int j = 0; j < i.transform.childCount; j++){
                        if(i.transform.GetChild(j)){
                            Destroy(i.transform.GetChild(j).gameObject);
                        }
                    }
                    i.SetActive(true);
                    var instance = Instantiate(willBuild, i.transform.position, Quaternion.identity);
                    instance.transform.parent = i.transform;
                    instance.SetActive(true);
                    i.GetComponent<MeshRenderer>().material = normalMaterial;
                    break;
                case TileState.material:
                    for(int j = 0; j < i.transform.childCount; j++){
                        if(i.transform.GetChild(j)){
                            Destroy(i.transform.GetChild(j).gameObject);
                        }
                    }
                    i.SetActive(true);
                    i.GetComponent<MeshRenderer>().material.color = Color.red;
                    break;
                case TileState.building:
                    for(int j = 0; j < i.transform.childCount; j++){
                        if(i.transform.GetChild(j)){
                            Destroy(i.transform.GetChild(j).gameObject);
                        }
                    }
                    i.SetActive(false);
                    break;
            }
        }
    }
}
