using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileEnum : MonoBehaviour
{
    public TileState tileState;
   
}

public class MaterialEnum: MonoBehaviour{
    public MaterialState materialState;

}
public class BuildingEnum: MonoBehaviour{
    public BuildingState buildingState;

}
public class TowerEnum: MonoBehaviour{
    public TowerState towerState;
}


public enum TileState
{
    normal,
    material,
    building
}
public enum MaterialState
{
    none,
    wood,
    stone,
    iron,
    adamantium
}
public enum BuildingState
{
    none,
    toolBox,
    bunker,
    tower
}

public enum TowerState
{
    none,
    parabola,
    straight,
    scope
}