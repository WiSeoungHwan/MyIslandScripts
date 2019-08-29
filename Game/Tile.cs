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
	[SerializeField]
	private GameObject[] effects;
	[SerializeField]
	private Canvas tileCanvas;
	[SerializeField]
	private TextMove headPopup;
	public TileData tileData;

	private GameObject tower = null;
	void Start(){
		hpText.transform.position = hpTransform.position;
		hpText.text = tileData.hp.ToString();
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
	public void TileHeadPopUpActive(string text, Color color){
        TextMove instance = Instantiate(this.headPopup);
		instance.transform.gameObject.SetActive(true);
        instance.transform.SetParent(tileCanvas.transform, false);
        instance.SetText(text, color);
    }
	public void TileSetup(TileData tile, SkinData skinData){
		// tile data 에 따라 프리펩 로드
		// 우선 기반 타일 로드
		this.tileData = tile;
		TileLoad(skinData.ground, ConstData.normal);
		// loadPrefabs(ConstData.normal, ConstData.normal);
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
						TileLoad(skinData.wood, ConstData.material);
						// loadPrefabs(ConstData.material + "/Wood", ConstData.material);
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
	public void TileHit(int damage){
		this.tileData.hp -= damage;
		TileHeadPopUpActive(damage.ToString(),Color.red);
		if(this.tileData.hp <= 0){
			
			if(transform.childCount < 6){
				if(tileData.towerState == TowerState.table){
					GameManager.Instance.SendMessage("TableDestoried", tileData.isMine);
				}
				Destroy(this.transform.GetChild(4).gameObject);
				
			}
			this.hpText.gameObject.SetActive(false);
			this.tileData.hp = 10;
			BoxCollider colider = gameObject.GetComponent<BoxCollider>();
			colider.size = Vector3.one;
			colider.center = Vector3.one;
			tileData.tileState = TileState.normal;
			this.tag = ConstData.normal;
		}
		this.hpText.text = tileData.hp.ToString();
	}

	public void BuildTower(TowerState state, GroundTile towerList){
		
		switch(state){
            case TowerState.parabola:
				
				this.tower = Instantiate(towerList[0], new Vector3(transform.position.x,0.1f,transform.position.z), Quaternion.identity);
				var towerComponent = this.tower.AddComponent<Tower>();
				towerComponent.TowerInit(state,tileData.index,tileData.isMine,effects);
				towerComponent.transform.parent = transform;
				towerComponent.gameObject.SetActive(true);
				this.tileData.tileState = TileState.building;
                this.tag = ConstData.building;
                break;
            case TowerState.straight:
				
				this.tower = Instantiate(towerList[1], new Vector3(transform.position.x,0.1f,transform.position.z), Quaternion.identity);
				towerComponent = this.tower.AddComponent<Tower>();
				towerComponent.TowerInit(state,tileData.index,tileData.isMine,effects);
				towerComponent.transform.parent = transform;
				towerComponent.gameObject.SetActive(true);
				this.tileData.tileState = TileState.building;
                this.tag = ConstData.building;
                break;
            case TowerState.scope:
				
				this.tower = Instantiate(towerList[2], new Vector3(transform.position.x,0.1f,transform.position.z), Quaternion.identity);
				towerComponent = this.tower.AddComponent<Tower>();
				towerComponent.TowerInit(state,tileData.index,tileData.isMine,effects);
				towerComponent.transform.parent = transform;
				towerComponent.gameObject.SetActive(true);
				this.tileData.tileState = TileState.building;
                this.tag = ConstData.building;
                break;
        }
		hpText.transform.gameObject.SetActive(true);
		BoxCollider colider = gameObject.GetComponent<BoxCollider>();
		colider.size = new Vector3(1,1.5f,1);
		colider.center = new Vector3(0,0.5f,0);
		if(tileData.isMine){
			this.tower.transform.Rotate(0,0,0);
		}else{
			this.tower.transform.Rotate(0,180f,0);
		}
	}
	public void BuildTower(TowerState state, GameObject tower){
		
		switch(state){
            case TowerState.table:
				this.tower = Instantiate(tower, new Vector3(transform.position.x,0.1f,transform.position.z), Quaternion.identity);
				this.tower.transform.parent = transform;
				this.tower.gameObject.SetActive(true);
				this.tileData.tileState = TileState.building;
				this.tileData.towerState = TowerState.table;
                this.tag = ConstData.building;
                break;
			case TowerState.bunker:
				this.tower = Instantiate(tower, new Vector3(transform.position.x,0.1f,transform.position.z), Quaternion.identity);
				this.tower.transform.parent = transform;
				this.tower.gameObject.SetActive(true);
				this.tileData.tileState = TileState.building;
				this.tileData.hp = 100;
				this.hpText.text = tileData.hp.ToString();
				this.tileData.towerState = TowerState.bunker;
                this.tag = ConstData.building;
				break;
        }
		hpText.transform.gameObject.SetActive(true);
		BoxCollider colider = gameObject.GetComponent<BoxCollider>();
		colider.size = new Vector3(1,1.2f,1);
		colider.center = new Vector3(0,0.5f,0);
		if(tileData.isMine){
			this.tower.transform.Rotate(0,0,0);
		}else{
			this.tower.transform.Rotate(0,180f,0);
		}
	}

	private GameObject TileInstantiate(GroundTile groundTile){
		GameObject tileObject = Instantiate(groundTile.data[Random.Range(0,groundTile.Length)], transform.position,Quaternion.identity);
		tileObject.transform.parent = transform;
		tileObject.SetActive(true);
		return tileObject;
	}

	private GameObject TileLoad(GroundTile groundTile, string tag){
		GameObject prefabObject;
		BoxCollider colider;
		if (gameObject.GetComponent<BoxCollider>() == null) {
			colider = gameObject.AddComponent<BoxCollider>();
		}
		colider = gameObject.GetComponent<BoxCollider>();
		if (tag == ConstData.material){
			prefabObject = TileInstantiate(groundTile);
			hpText.transform.gameObject.SetActive(true);
			colider.size = new Vector3(1,1.5f,1);
			colider.center = new Vector3(0,0.5f,0);
			prefabObject.transform.position = new Vector3(transform.position.x, 0f,transform.position.z);
		}else if(tag == ConstData.normal){
			prefabObject = TileInstantiate(groundTile);
			hpText.transform.gameObject.SetActive(false);
			colider.size = Vector3.one;
			colider.center = Vector3.zero;
		}else if(tag == ConstData.building){
			prefabObject = TileInstantiate(groundTile);
			hpText.transform.gameObject.SetActive(true);
			colider.size = new Vector3(1,1.5f,1);
			colider.center = new Vector3(0,0.5f,0);
			prefabObject.transform.position = new Vector3(transform.position.x, 0.0f,transform.position.z);
		}else{
			Debug.Log("Tag problem");
			prefabObject = null;
		}
		colider.isTrigger = true;
		gameObject.tag = tag;
		return prefabObject;
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


