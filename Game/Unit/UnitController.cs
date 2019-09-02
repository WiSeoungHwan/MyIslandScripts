using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyIsland
{
    // Unit 관련 관리 객체 
    public class UnitController : MonoBehaviour
    {
        #region Serialize Field
        // Ground
        [SerializeField]
        private Ground ground;
        #endregion



        #region Public Method
        public void UnitControllerInit(GameObject selectedTheme, MaterialInitData materialInitData){
            ground.GroundInit(selectedTheme,materialInitData);
        }
        #endregion
    }

}
