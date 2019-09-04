using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyIsland
{
    public class UnitData: MonoBehaviour
    {
        public int unitLevel{get; set;}
        public int unitIndex{get; set;}
        public int unitHp{get; set;}
        public int unitDemage{get;set;}
        public int stamina{get; set;}
        public int tableCount{
            get{return tableCount;}
            set{
                EventManager.Instance.emit(EVENT_TYPE.TABLE_COUNT_CHANGE, this, value);
                tableCount = value;
            } 

        }
        public UnitMaterialData unitMaterial;
    }
}
