using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MyIsland
{
    public enum TowerEnum{
        TOWER_1,
        TOWER_2,
        TOWER_3
    }
    public class Tower : Building
    {
        #region MonoBehaviour CallBack
        void Start(){
            this.buildingKind = BuildingKind.TOWER;
        } 
        #endregion
    }

}
