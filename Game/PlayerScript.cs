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
    Material normalMaterial;

    private Ground ground;

    private int playerIndex = 0;
    private int enemyIndex = 0;

    private bool isMine;
    public delegate void MaterialHit(MaterialState materialState, int num);
    public delegate bool MoveCount();

    private MaterialHit materialHit = null;
    private MoveCount moveCount = null;

    // MARK: - Public Fields

    // MARK: - MonoBehaviour

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        PlayerAction();
    }

    // MARK: - Public Methods 

    public void PlayerInit(Ground ground, bool isMine, int startIndex)
    {
        this.isMine = isMine;
        this.ground = ground;
        this.playerIndex = startIndex;
    }

    public void SetCallBacks(MaterialHit materialHit, MoveCount moveCount)
    {
        this.materialHit = materialHit;
        this.moveCount = moveCount;
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
                    if (hitInfo.transform.gameObject.GetComponent<Tile>())
                    {
                        Tile tile = hitInfo.transform.gameObject.GetComponent<Tile>();
                        if(!tile.tileData.isMine){return;} // 적의 타일을 클릭시 
                        if (tile.tileData.index == playerIndex + 5 || tile.tileData.index == playerIndex - 5 || tile.tileData.index == playerIndex + 1 || tile.tileData.index == playerIndex - 1)
                        {
                            if(!moveCount()){return;}
                            switch (tile.tileData.tileState){
                                case TileState.normal:
                                    Move(tile);
                                    break;
                                case TileState.material:
                                    Collect(tile);
                                    break;
                                case TileState.building:
                                    break;
                            }
                        }
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
        CheckAround();
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

    }

    private void EnemyAction(int inputIndex)
    {
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
                        CheckAround();
                    }
                }
                break;
            case TileState.building:
                Debug.Log("building");
                break;
        }
        CheckAround();
    }


    private void CheckAround()
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
                    i.SetActive(true);
                    i.GetComponent<MeshRenderer>().material = normalMaterial;
                    break;
                case TileState.material:
                    i.SetActive(true);
                    i.GetComponent<MeshRenderer>().material.color = Color.red;
                    break;
                case TileState.building:
                    i.SetActive(false);
                    break;
            }
        }
    }
}
