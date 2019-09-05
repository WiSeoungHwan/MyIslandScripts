using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MyIsland
{

    public class Bunker : Building
    {
        #region MonoBehaviour CallBack
        void Start()
        {
            this.buildingKind = BuildingKind.BUNKER;
        }
        #endregion
    }
}
