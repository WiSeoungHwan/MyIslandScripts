using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable] 
    public class GroundTile 
    { 
        public GameObject[] data; 
        public GameObject this[int index] { get {return data[index];} } 
        public int Length { get { return data.Length; } } 
    } 