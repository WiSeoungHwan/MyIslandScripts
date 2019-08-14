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
	[SerializeField]
	private GameObject tileTargeting;

	public TileData tileData;

	private Tower tower = null;
	void Start(){
		// SetTilePrefab();
		hpText.transform.position = hpTransform.position;
		hpText.rectTransform.Rotate(Vector3.forward);
	}

	public void TileTargeting(bool isTargeting){
		tileTargeting.SetActive(isTargeting);
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

	public void BuildTower(TowerState state){
		switch(state){
            case TowerState.parabola:
				GameManager.Instance.SendMessage("NotiTest", tileData, SendMessageOptions.RequireReceiver);
				this.tower = loadPrefabs("Tile/Building/Wood/Wood_parabola_1", ConstData.building).AddComponent<Tower>();
				this.tower.TowerInit(state,tileData.index,tileData.isMine);
				this.tileData.tileState = TileState.building;
                break;
            case TowerState.straight:
                break;
            case TowerState.scope:
                break;
        }
	}

	private GameObject loadPrefabs(string path, string tag){
		GameObject prefabObject;
		BoxCollider colider;
		if (gameObject.GetComponent<BoxCollider>() == null) {
			colider = gameObject.AddComponent<BoxCollider>();
		}
		colider = gameObject.GetComponent<BoxCollider>();
		if (tag == ConstData.material){
			prefabObject = loadAll(path);
			hpText.transform.gameObject.SetActive(true);
			colider.size = new Vector3(1,1.5f,1);
			colider.center = new Vector3(0,0.5f,0);
			prefabObject.transform.position = new Vector3(transform.position.x, -0.2f,transform.position.z);
		}else if(tag == ConstData.normal){
			prefabObject = loadAll(path);
			hpText.transform.gameObject.SetActive(false);
			colider.size = Vector3.one;
			colider.center = Vector3.zero;
		}else if(tag == ConstData.building){
			prefabObject = loadOne(path);
			hpText.transform.gameObject.SetActive(true);
			colider.size = new Vector3(1,1.5f,1);
			colider.center = new Vector3(0,0.5f,0);
			prefabObject.transform.position = new Vector3(transform.position.x, 0.0f,transform.position.z);
		}else{
			Debug.Log("Tag problem");
			prefabObject = null;
		}
		
		prefabObject.transform.parent = gameObject.transform;
		gameObject.tag = tag;
		prefabObject.SetActive(true);
		return prefabObject;
	}

	private GameObject loadAll(string forderName){
		if (Resources.LoadAll("Tile/"+forderName) == null){Debug.Log("Err: "+forderName+"is null");return null;}
		Object[] prefabArr = Resources.LoadAll("Tile/"+forderName);
		if (prefabArr.Length == 0) {Debug.Log(forderName + " has " + prefabArr.Length + " prefab");return null;}
		GameObject prefabObject = Instantiate(prefabArr[Random.Range(0,prefabArr.Length)],gameObject.transform.position,Quaternion.identity) as GameObject;
		return prefabObject;
	}
	private GameObject loadOne(string path){
		if(Resources.Load(path) == null) {Debug.Log("Err: "+path+"is null");return null;}
		GameObject prefabObject = Instantiate(Resources.Load(path),gameObject.transform.position, Quaternion.identity) as GameObject;
		if(tileData.isMine){
			prefabObject.transform.Rotate(0,-90f,0);
		}else{
			prefabObject.transform.Rotate(0,90f,0);
		}
		return prefabObject;
	}
}


