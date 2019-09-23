using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyIsland_InGame
{
    public class UnitData
    {
        public int unitLevel{get; set;}
        public int unitIndex{get; set;}
        public float unitHp{get; set;}
        public float unitMaxHp{get; set;}
        public int unitDemage{get;set;}
        public int maxStamina{get;set;}
        public int stamina{get; set;}
        public float staminaCoolTime{get;set;}
        public int tableCount{get;set;}
        public UnitMaterialData unitMaterial;
    }
}
