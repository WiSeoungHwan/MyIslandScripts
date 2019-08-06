using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
public class GameSetUpManager : MonoBehaviourPunCallbacks, IPunObservable
{
    // MARK: - Private Fields

    [SerializeField]
    Ground myGround;
    [SerializeField]
    Ground enemyGround;
    private PhotonView _photonView;
    private Player localPlayer;
    int[][] tileStateArr = new int[2][];
    // MARK: - Public Fields

    // MARK: - PunCallbacks
    public override void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);        
    }

    public override void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer){
        Debug.Log("EnterRoom");
        _photonView.RPC("GroundSetUpForNotMasterClient", RpcTarget.All, tileStateArr);
    }
    

    // MARK: - MonoBehaviour Callbacks
    void Awake()
    {
        GameSceneSetup();
    }
    void Start()
    {
        _photonView = GetComponent<PhotonView>();
    }

    void Update()
    {
    }

    // MARK: - IPunObservable

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {

    }

    // MARK: - Private Methods
    void GameSceneSetup()
    {
        _photonView = GetComponent<PhotonView>();
        // Instantiate all player in current room
        if(PhotonNetwork.IsMasterClient){
            PhotonNetwork.Instantiate("Player", Vector3.zero, Quaternion.identity);
        }

        GroundSetUp();
    }

    void GroundSetUp()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            
            tileStateArr[0] = myGround.CreateGround();
            tileStateArr[1] = enemyGround.CreateGround(); 
        }
        
    }

    [PunRPC]
    void GroundSetUpForNotMasterClient(int[][] data)
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            Debug.Log(data[0].Length);

            myGround.CreateGroundForClient(data[1]);
            enemyGround.CreateGroundForClient(data[0]);
            var player = PhotonNetwork.Instantiate("Player", Vector3.zero, Quaternion.identity).GetComponent<PlayerScript>();
        }
    }

    

    

    // MARK: - Public Methods

}
