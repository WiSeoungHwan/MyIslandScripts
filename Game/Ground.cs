using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class Ground : MonoBehaviour {
	// MARK: - Properties
	public List<Tile> tileArr = new List<Tile>();
	

	// MARK: - MonoBehaviour CallBacks
	void Awake() {
		
	}
	void Start(){
		
	}

	

	// MARK: - Public Methods
	public int[] CreateGround(){
		var index = 0;
		int[] tileStateArr = new int[ConstData.mapSize * ConstData.mapSize];
		for(int i = 0; i < ConstData.mapSize; i++){
			for(int j = 0; j < ConstData.mapSize; j++){
				if (Resources.Load("Tile/tile") == null) {Debug.Log("Err: tile is null"); return tileStateArr;}
				Vector3 pos = new Vector3(i + transform.position.x,gameObject.transform.position.y,j);
				GameObject tileObject = Instantiate(Resources.Load("Tile/tile"),pos,Quaternion.identity) as GameObject;
				var tile = tileObject.GetComponent<Tile>();
				tile.index = index;
				if (i == 0 && j == 0){ // player instance position
					tile.state = TileState.normal;
				}else {
					// .normal, .material 
					tile.state = (TileState)Random.Range(0,2);			
				}
				tileObject.transform.parent = gameObject.transform;
				tileObject.SetActive(true);
				tileArr.Add(tile);
				Debug.Log(index);
				tileStateArr[index] = (int)tile.state;
				index++;
			}
		}
		return tileStateArr;
	}

	public void CreateGroundForClient(int[] tileStateIndex){
		var index = 0;
		for(int i = 0; i < ConstData.mapSize; i++){
			for(int j = 0; j < ConstData.mapSize; j++){
				if (Resources.Load("Tile/tile") == null) {Debug.Log("Err: tile is null"); return;}
				Vector3 pos = new Vector3(i + transform.position.x,gameObject.transform.position.y,j);
				GameObject tileObject = Instantiate(Resources.Load("Tile/tile"),pos,Quaternion.identity) as GameObject;
				var tile = tileObject.GetComponent<Tile>();
				tile.index = index;
				tile.state = (TileState)tileStateIndex[index];			
				tileObject.transform.parent = gameObject.transform;
				tileObject.SetActive(true);
				tileArr.Add(tile);
				index++;
			}
		}
	}
}
