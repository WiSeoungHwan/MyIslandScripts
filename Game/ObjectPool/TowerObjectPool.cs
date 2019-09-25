using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyIsland_InGame
{   
    public enum BuildingKind{
        TABLE,
        BUNKER,
        TOWER
    }

    public enum TowerPoolList{
        WOOD_1,WOOD_2,WOOD_3,
        STONE_1,STONE_2,STONE_3,
        IRON_1,IRON_2,IRON_3,
        ADAM_1,ADAM_2,ADAM_3
    }
    
    
    

    public class TowerObjectPool : SingletonMonoBehaviour<TowerObjectPool>
    {
        #region Serialize Field
        private GameObject[] towerPrefabs;
        #endregion

        #region Private Field
        private Dictionary<TowerPoolList,GameObjectPool<GameObject>> towerPoolDictionary = new Dictionary<TowerPoolList, GameObjectPool<GameObject>>();
        
        private List<GameObject> activeList = new List<GameObject>();
        #endregion

        #region MonoBehaviour Callbacks
        protected override void OnAwake(){
            base.OnAwake();
            towerPrefabs = AllBuildings.Instance.GetSelectedTowerPrefabs();
            Debug.Log(towerPrefabs.Length + "towerPrefabs length");
            for(int i = 0; i < towerPrefabs.Length; i++){
                GameObjectPool<GameObject> objPool = new GameObjectPool<GameObject>(10, () => {
                    GameObject obj = Instantiate(towerPrefabs[i]);
                    obj.transform.SetParent(transform);
                    obj.AddComponent<Tower>();
                    obj.SetActive(false);
                    return obj;
                });
                towerPoolDictionary.Add((TowerPoolList)i,objPool);
            }
        }
        #endregion

        #region Public Methods
        public GameObject Pop(TowerPoolList key){
            GameObjectPool<GameObject> pool = towerPoolDictionary[key];
            GameObject obj = pool.pop();
            activeList.Add(obj);
            return obj;
        }

        public void Remove(TowerPoolList key, GameObject obj){
            if(activeList.Remove(obj)){
                Reset(key,obj);        
            }
        }
        #endregion

        #region Private Methods
        private void Reset(TowerPoolList key, GameObject obj){
            GameObjectPool<GameObject> pool = towerPoolDictionary[key];
            pool.push(obj);
            obj.SetActive(false);
        }
        #endregion
    }
}

