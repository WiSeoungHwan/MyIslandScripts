using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class StartSceneManager : MonoBehaviourPunCallbacks {

	[SerializeField]
	private int maxPlayers;

	[SerializeField]
	private bool isMultiplay = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	// PunCallbacks

	// MARK: - PunCallbacks
    public override void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);        
    }

    public override void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }

	public override void OnConnectedToMaster(){
		PhotonNetwork.AutomaticallySyncScene = true;
	}

	public override void OnJoinRandomFailed(short returnCode, string message){
		Debug.Log("OnJoinRandomFaild");
		CreateRoom();
	}
	public override void OnCreateRoomFailed(short returnCode, string message){
		Debug.Log("OnCreateRoomFaild");
	}

	public override void OnJoinedRoom(){
		Debug.Log("OnJoinedRoom");
		PhotonNetwork.LoadLevel(1);
	}


	// private
	void CreateRoom(){
		RoomOptions options = new RoomOptions(){IsVisible = true, IsOpen = true, MaxPlayers = (byte)maxPlayers };
		PhotonNetwork.CreateRoom(null,options);
	}

	// Public

	public void StartButtonTap(){
		if (isMultiplay){
			PhotonNetwork.JoinRandomRoom();
		}else{
			SceneManager.LoadScene(2);
		}
	}
}
