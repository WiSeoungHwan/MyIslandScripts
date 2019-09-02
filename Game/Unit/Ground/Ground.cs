using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyIsland
{
    public class Ground : MonoBehaviour
    {
        #region Serialize Fields
        // 테마 타일 
        [SerializeField]
        private Transform body;

        #endregion


        #region Public Methods
        public void GroundInit(GameObject selectedTheme, MaterialInitData materialInitData)
        {
            // 타일 인스턴스 
            GameObject ground = Instantiate(selectedTheme,  body.position, Quaternion.identity);
            ground.transform.SetParent(body);
            ground.SetActive(true);
            
        }
        #endregion
    }
}

