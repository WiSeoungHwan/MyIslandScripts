using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MyIsland
{
    public enum TileState
    {
        NORMAL,
        MATERIAL,
        BUILDING
    }
    public class Tile : MonoBehaviour
    {
        #region Private Field
        private TileData tileData;

        #endregion



        #region Public Methods
        public void TileInit(TileData tileData)
        {
            this.tileData = tileData;
            if (tileData.tileState == TileState.MATERIAL)
            {
                GameObject obj;
                switch (tileData.materialState)
                {
                    case MaterialState.WOOD:
                        obj = MaterialObjectPool.Instance.Pop(MaterialState.WOOD);
                        break;
                    case MaterialState.STONE:
                        obj = MaterialObjectPool.Instance.Pop(MaterialState.STONE);
                        break;
                    case MaterialState.IRON:
                        obj = MaterialObjectPool.Instance.Pop(MaterialState.IRON);
                        break;
                    case MaterialState.ADAM:
                        obj = MaterialObjectPool.Instance.Pop(MaterialState.ADAM);
                        break;
                    default:
                        obj = MaterialObjectPool.Instance.Pop(MaterialState.WOOD);
                        break;
                }
                obj.transform.position = new Vector3(transform.position.x, 0f,transform.position.z);
                obj.SetActive(true);
            }
        }
        #endregion
    }
}
