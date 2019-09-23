using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MyIsland_InGame
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
