using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TileState{
	normal,
	material,
	building
}
public class Tile: MonoBehaviour{
	[SerializeField]
	public TileState state = TileState.normal;

	public int index;
	void Start(){
		SetTilePrefab();
	}

	public void SetTilePrefab(){
		switch (state){
			case TileState.normal:
				loadPrefabs(ConstData.normal);
				break;
			case TileState.material:
				loadPrefabs(ConstData.material);
				break;
			case TileState.building:
				loadPrefabs(ConstData.building);
				break;
		}
	}

	public void ResetTilePrefab(TileState state){
		this.state = state;
		GameObject go = gameObject.transform.GetChild(0).gameObject; 
		Destroy(go);
		SetTilePrefab();
	}
	

	private void loadPrefabs(string forderName){
		if (Resources.LoadAll("Tile/"+forderName) == null){Debug.Log("Err: "+forderName+"is null");return;}
		Object[] prefabArr = Resources.LoadAll("Tile/"+forderName);
		if (prefabArr.Length == 0) {Debug.Log(forderName + " has " + prefabArr.Length + " prefab");return;}
		GameObject prefabObject = Instantiate(prefabArr[Random.Range(0,prefabArr.Length)],gameObject.transform.position,Quaternion.identity) as GameObject;
		prefabObject.transform.parent = gameObject.transform;
		gameObject.tag = forderName;
		BoxCollider colider;
		if (gameObject.GetComponent<BoxCollider>() == null) {
			colider = gameObject.AddComponent<BoxCollider>();
		}
		colider = gameObject.GetComponent<BoxCollider>();
		if (forderName == ConstData.material){
			colider.size = new Vector3(1,1.5f,1);
			colider.center = new Vector3(0,0.5f,0);
		}else{
			colider.size = Vector3.one;
			colider.center = Vector3.zero;
		}
		prefabObject.SetActive(true);
	}
}


