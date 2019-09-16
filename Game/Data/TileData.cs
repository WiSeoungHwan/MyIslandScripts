using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyIsland
{
    public class TileData
    {
        public bool isPlayerGround;
        public int index;
        public float hp;
        public TileState tileState;
        public MaterialState materialState;

        public TileData(bool isPlayerGround, int index, int hp, TileState tileState){
            this.isPlayerGround = isPlayerGround;
            this.index = index;
            this.hp = hp;
            this.tileState = tileState;
        }
        
        public TileData(bool isPlayerGround, int index, int hp, TileState tileState, MaterialState materialState){
            if (tileState != TileState.MATERIAL){return;}
            this.isPlayerGround = isPlayerGround;
            this.index = index;
            this.hp = hp;
            this.tileState = tileState;
            this.materialState = materialState;
        }

    }
}

