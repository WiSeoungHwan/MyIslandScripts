using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


namespace MyIsland_InGame
{
    public enum TowerKind{
        PARABOLA,
        STRAIGHT,
        SCOPE
    }
    [Serializable]
    public class TowerData
    {
        public bool isPlayerGround;
        public int tileIndex;
        public TowerKind towerKind;
        public float towerDamage;
        public float towerHp;   
    }
}

