using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerScript : MonoBehaviourPunCallbacks, IPunObservable
{

    // MARK: - Private Fields

    [SerializeField]
    GameObject[] practicableAreas;

    [SerializeField]
    Material normalMaterial;

	private Ground ground;
    private PhotonView _photonView;
    private int playerIndex = 0;
    private int enemyIndex = 0;

    // MARK: - Public Fields

    // MARK: - IPunObservable

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info){
        if (stream.IsWriting){  // 데이터를 보내는것
            stream.SendNext(playerIndex);
        }else{ // 받는것 
            enemyIndex = (int)stream.ReceiveNext();
            EnemyUpdate(enemyIndex);
        }
    }

    // MARK: - MonoBehaviour

    // Use this for initialization
    void Start()
    {
        _photonView = GetComponent<PhotonView>();
        Debug.Log("Is Master Client? " + PhotonNetwork.IsMasterClient);
        PlayerSetUp();
    }

    // Update is called once per frame
    void Update()
    {
		PlayerAction();
    }

    // MARK: - Pirvate Methods
    private void EnemyUpdate(int currentEnemyIndex){ // 적의 이동이벤트가 왔을때 호출 
        if (photonView.IsMine) {return;}
        if (!ground) {Debug.Log("Player script - EnemyUpdate: ground is null"); return;}
        var pos = ground.tileArr[currentEnemyIndex].transform.position;
        gameObject.transform.position = new Vector3(pos.x, transform.position.y, pos.z);
    }
    public void PlayerSetUp()
    {
        if (_photonView.IsMine)
        {
            transform.position = Vector3.zero;
            ground = GameObject.Find("MyGround").GetComponent<Ground>();
        }
        else
        {
            transform.position = new Vector3(ConstData.enemyGroundXPos, 0, 0);
            ground = GameObject.Find("EnemyGround").GetComponent<Ground>();
        }
        CheckAround();
    }

    private void PlayerAction()
    {
        if (!_photonView.IsMine){return;}
        // click the ground 
        if (Input.GetMouseButtonDown(0))
        {
            // get hitInfo
            RaycastHit hitInfo;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hitInfo))
            {
                Move(hitInfo);
                Collect(hitInfo);
            }
        }
    }

    private void Collect(RaycastHit hitInfo){
        if (hitInfo.transform.tag == ConstData.material){
            Tile tile = hitInfo.transform.gameObject.GetComponent<Tile>();
            if (tile.tileData.index == playerIndex + 5 || tile.tileData.index == playerIndex - 5 || tile.tileData.index == playerIndex + 1 || tile.tileData.index == playerIndex - 1){
                // tile.ResetTilePrefab(TileState.normal);
                CheckAround();
            }
        }
    }

    private void Move(RaycastHit hitInfo)
    {

        float rotateY = 90;
        switch (hitInfo.transform.name)
        {
            case "X1":
                rotateY = 90;
                playerIndex += 5; // down
                break;
            case "X-1":
                rotateY = -90;
                playerIndex -= 5; // up 
                break;
            case "Z1":
                rotateY = 0;
                playerIndex += 1; // left
                break;
            case "Z-1":
                rotateY = 180;
                playerIndex -= 1; // right
                break;
        }
        if (hitInfo.transform.tag == "PracticableArea")
        {
            transform.Find("Body").transform.rotation = Quaternion.Euler(new Vector3(0, rotateY, 0));
            var pos = hitInfo.transform.position;
            gameObject.transform.position = new Vector3(pos.x, transform.position.y, pos.z);
            CheckAround();
        }
    }

    private void CheckAround()
    {
        if(ground.tileArr.Count == 0){return;}
		if(!_photonView.IsMine){
			foreach (GameObject i in practicableAreas){
				i.SetActive(false);
			}
			return;
		}
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
            
			if(ground.tileArr[aroundIndex] == null) {continue;}
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
