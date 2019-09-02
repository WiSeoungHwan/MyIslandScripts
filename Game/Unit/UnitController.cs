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

        #region Private Field
        private bool isPlayer;
        #endregion

        #region Public Method
        public void UnitControllerInit(bool isPlayer,GameObject selectedTheme, MaterialInitData materialInitData){
            this.isPlayer = isPlayer;
            ground.GroundInit(isPlayer,selectedTheme,materialInitData);
        }
        #endregion
    }

}
