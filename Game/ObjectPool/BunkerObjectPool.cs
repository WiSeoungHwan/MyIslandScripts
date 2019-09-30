using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyIsland_InGame
{
    public enum BunkerPoolList{
        BUNKER_1,BUNKER_2,BUNKER_3,BUNKER_4
    }
    public class BunkerObjectPool : SingletonMonoBehaviour<BunkerObjectPool>
    {
        #region Serialize Field
        [SerializeField]
        private GameObject[] bunkerPrefabs;

        #endregion

        #region Private Field
        private Dictionary<BunkerPoolList,GameObjectPool<GameObject>> bunkerPoolDictionary = new Dictionary<BunkerPoolList, GameObjectPool<GameObject>>();
        private List<GameObject> activeList = new List<GameObject>();
        #endregion


        protected override void OnAwake(){
            base.OnAwake();
            for(int i = 0; i < bunkerPrefabs.Length; i++){
                GameObjectPool<GameObject> objPool = new GameObjectPool<GameObject>(10, () => {
                    GameObject obj = Instantiate(bunkerPrefabs[i]);
                    obj.transform.SetParent(transform);
                    obj.AddComponent<Bunker>();
                    obj.SetActive(false);
                    return obj;
                });
                bunkerPoolDictionary.Add((BunkerPoolList)i,objPool);
            }
        }

        #region Public Methods
        public GameObject Pop(BunkerPoolList key){
            GameObjectPool<GameObject> pool = bunkerPoolDictionary[key];
            GameObject obj = pool.pop();
            obj.SetActive(true);
            activeList.Add(obj);
            return obj;
        }

        public void Remove(BunkerPoolList key, GameObject obj){
            if(activeList.Remove(obj)){
                Debug.Log("Count " + activeList.Count + "key " + key);
                Reset(key,obj);        
            }
            
        }
        #endregion

        #region Private Methods
        private void Reset(BunkerPoolList key, GameObject obj){
            GameObjectPool<GameObject> pool = bunkerPoolDictionary[key];
            obj.SetActive(false);
            pool.push(obj);
            
        }
        #endregion
        
    }
}

