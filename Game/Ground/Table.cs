using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyIsland
{

    public class Table : Building
    {
        #region MonoBehaviour CallBack
        void Start()
        {
            this.buildingKind = BuildingKind.TABLE;
        }
        #endregion
    }
}

