using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MyIsland_InGame
{
    public enum TablePoolList
    {
        TABLE_1, TABLE_2, TABLE_3, TABLE_4
    }

    public class TableObjectPool : SingletonMonoBehaviour<TableObjectPool>
    {

        #region Serialize Field
        [SerializeField]
        private GameObject[] tablePrefabs;
        
        #endregion

        #region Private Field
        private Dictionary<TablePoolList,GameObjectPool<GameObject>> tablePoolDictionary = new Dictionary<TablePoolList, GameObjectPool<GameObject>>();
        
        private List<GameObject> activeList = new List<GameObject>();
        #endregion


        #region MonoBehaviour Callbacks
        protected override void OnAwake(){
            base.OnAwake();
            for(int i = 0; i < tablePrefabs.Length; i++){
                GameObjectPool<GameObject> objPool = new GameObjectPool<GameObject>(10, () => {
                    GameObject obj = Instantiate(tablePrefabs[i]);
                    obj.transform.SetParent(transform);
                    obj.AddComponent<Table>();
                    obj.SetActive(false);
                    return obj;
                });
                tablePoolDictionary.Add((TablePoolList)i,objPool);
            }
        }
        #endregion

        #region Public Methods
        public GameObject Pop(TablePoolList key){
            GameObjectPool<GameObject> pool = tablePoolDictionary[key];
            GameObject obj = pool.pop();
            activeList.Add(obj);
            return obj;
        }

        public void Remove(TablePoolList key, GameObject obj){
            if(activeList.Remove(obj)){
                Reset(key,obj);        
            }
        }
        #endregion

        #region Private Methods
        private void Reset(TablePoolList key, GameObject obj){
            GameObjectPool<GameObject> pool = tablePoolDictionary[key];
            pool.push(obj);
            obj.SetActive(false);
        }
        #endregion
    }
}

