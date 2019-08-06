using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class NetworkController : MonoBehaviourPunCallbacks {
	
	[SerializeField]
	private int gameVersion; // 포톤이 게임 버전에 따라 같은 유저 끼리 매치메이킹을 잡을 수있음.

	[SerializeField]
	private Text serverRegionText;

	public override void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);        
    }

    public override void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }

	void Start () {
		PhotonNetwork.GameVersion = gameVersion.ToString();
		PhotonNetwork.ConnectUsingSettings();
	}
	
	public override void OnConnectedToMaster(){
		string region = "Server region: " + PhotonNetwork.CloudRegion.ToString();
		Debug.Log(region);
		serverRegionText.text = region;
	}
}
