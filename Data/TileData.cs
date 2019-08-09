using System.Collections;
using System.Collections.Generic;


public enum TileState{
	normal,
	material,
	building
}
public enum MaterialState{
    none,
    wood,
    stone,
    iron,
    adamantium
}
public enum BuildingState{
    none,
    toolBox,
    bunker,
    tower
}

public enum TowerState{
    none,
    parabola,
    straight,
    scope
}

[System.Serializable]
public class TileData 
{
    public bool isMine = false;
    public int index = 0;
    public int hp = 0;
    public TileState tileState = TileState.normal;
    public MaterialState materialState = MaterialState.none;
    public BuildingState buildingState = BuildingState.none;
    public TowerState towerState = TowerState.none;
}
