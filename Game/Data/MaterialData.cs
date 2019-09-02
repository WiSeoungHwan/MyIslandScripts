using System.Collections;
using System.Collections.Generic;


namespace MyIsland
{
    public enum MaterialState{
        WOOD,
        STONE,
        IRON,
        ADAM
    }
    public class MaterialData
    {
        public MaterialState state {get; set;}
        public int count {get; set;}
        public int amount {get; set;}
    }

}
