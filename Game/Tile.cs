using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Tile: MonoBehaviour{
	
	// Material or building Ui
	[SerializeField]
	public Transform hpTransform;
	[SerializeField]
	private Text hpText;

	public TileData tileData;
	void Start(){
		// SetTilePrefab();
		hpText.transform.position = hpTransform.position;
		hpText.rectTransform.Rotate(Vector3.forward);
	}

	public void MaterialHit(int damage){
		this.tileData.hp -= damage;
		if(this.tileData.hp < 0){
			this.tileData.hp = 0;
		}
		hpText.text = this.tileData.hp.ToString();
	}
	public void TileSetup(TileData tile){
		// tile data 에 따라 프리펩 로드
		// 우선 기반 타일 로드
		this.tileData = tile;
		loadPrefabs(ConstData.normal, ConstData.normal);
		switch (tile.tileState){
			// 자원이나 건설물이 없을때
			case TileState.normal:
				break;
			// 자원일때 
			case TileState.material:
				// 어떤 자원을 불러올 것인지
				
				switch(tile.materialState){
					case MaterialState.none:
						break;
					case MaterialState.wood:
						loadPrefabs(ConstData.material + "/Wood", ConstData.material);
						break;
					case MaterialState.stone:
						break;
					case MaterialState.iron:
						break;
					case MaterialState.adamantium:
						break;
				}
				break;
			// 건설물 일때 
			case TileState.building:
				break;
		}
	}

	private void loadPrefabs(string forderName, string tag){
		if (Resources.LoadAll("Tile/"+forderName) == null){Debug.Log("Err: "+forderName+"is null");return;}
		Object[] prefabArr = Resources.LoadAll("Tile/"+forderName);
		if (prefabArr.Length == 0) {Debug.Log(forderName + " has " + prefabArr.Length + " prefab");return;}
		GameObject prefabObject = Instantiate(prefabArr[Random.Range(0,prefabArr.Length)],gameObject.transform.position,Quaternion.identity) as GameObject;
		prefabObject.transform.parent = gameObject.transform;
		gameObject.tag = tag;
		BoxCollider colider;
		if (gameObject.GetComponent<BoxCollider>() == null) {
			colider = gameObject.AddComponent<BoxCollider>();
		}
		colider = gameObject.GetComponent<BoxCollider>();
		if (tag != ConstData.normal){
			hpText.transform.gameObject.SetActive(true);
			colider.size = new Vector3(1,1.5f,1);
			colider.center = new Vector3(0,0.5f,0);
			prefabObject.transform.position = new Vector3(transform.position.x, -0.2f,transform.position.z);
		}else{
			hpText.transform.gameObject.SetActive(false);
			colider.size = Vector3.one;
			colider.center = Vector3.zero;
		}
		prefabObject.SetActive(true);
	}
}


