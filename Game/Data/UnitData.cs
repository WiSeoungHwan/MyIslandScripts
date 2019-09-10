using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyIsland
{
    public class UnitData
    {
        public int unitLevel{get; set;}
        public int unitIndex{get; set;}
        public float unitHp{get; set;}
        public float unitMaxHp{get; set;}
        public int unitDemage{get;set;}
        public int stamina{get; set;}
        public int tableCount{get;set;}
        public UnitMaterialData unitMaterial;
    }
}
