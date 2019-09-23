using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyIsland_InGame
{
    public class MaterialObjectPool : SingletonMonoBehaviour<MaterialObjectPool>
    {
        #region Serialize Field
        [SerializeField]
        private GameObject woodPrefab;
        [SerializeField]
        private GameObject stonePrefab;
        [SerializeField]
        private GameObject ironPrefab;
        [SerializeField]
        private GameObject adamPrefab;
        #endregion

        #region Private Field
        private GameObjectPool<GameObject> woodPool;
        private GameObjectPool<GameObject> stonePool;
        private GameObjectPool<GameObject> ironPool;
        private GameObjectPool<GameObject> adamPool;

        private List<GameObject> activeList;
        #endregion

        #region MonoBehaviour Callbacks
        protected override void OnAwake()
        {
            base.OnAwake();
            woodPool = new GameObjectPool<GameObject>(8, () =>
            {
                GameObject obj = Instantiate(woodPrefab);
                obj.transform.SetParent(transform);
                obj.SetActive(false);
                return obj;
            });
            stonePool = new GameObjectPool<GameObject>(4, () =>
            {
                GameObject obj = Instantiate(stonePrefab);
                obj.transform.SetParent(transform);
                obj.SetActive(false);
                return obj;
            });
            ironPool = new GameObjectPool<GameObject>(4, () =>
            {
                GameObject obj = Instantiate(ironPrefab);
                obj.transform.SetParent(transform);
                obj.SetActive(false);
                return obj;
            });
            adamPool = new GameObjectPool<GameObject>(2, () =>
            {
                GameObject obj = Instantiate(adamPrefab);
                obj.transform.SetParent(transform);
                obj.SetActive(false);
                return obj;
            });
            activeList = new List<GameObject>();
        }
        #endregion

        #region Public Methods

        public void Remove(MaterialState state, GameObject obj)
        {
            if (activeList.Remove(obj))
            {
                Reset(state, obj);
            }
        }

        public GameObject Pop(MaterialState state)
        {
            GameObject obj;
            switch (state)
            {
                case MaterialState.WOOD:
                    obj = woodPool.pop();
                    break;
                case MaterialState.STONE:
                    obj = stonePool.pop();
                    break;
                case MaterialState.IRON:
                    obj = ironPool.pop();
                    break;
                case MaterialState.ADAM:
                    obj = adamPool.pop();
                    break;
                default:
                    obj = woodPool.pop();
                    break;
            }
            activeList.Add(obj);
            return obj;
        }
        #endregion

        #region Private Methods
        private void Reset(MaterialState state, GameObject obj)
        {
            switch (state)
            {
                case MaterialState.WOOD:
                    woodPool.push(obj);
                    break;
                case MaterialState.STONE:
                    stonePool.push(obj);
                    break;
                case MaterialState.IRON:
                    ironPool.push(obj);
                    break;
                case MaterialState.ADAM:
                    adamPool.push(obj);
                    break;
            }
            obj.SetActive(false);
        }
        #endregion
    }
}
