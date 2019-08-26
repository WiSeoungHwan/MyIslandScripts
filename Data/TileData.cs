using System.Collections;
using System.Collections.Generic;




[System.Serializable]
public class TileData 
{
    public bool isMine = false;
    public int index = 0;
    public int hp = 10;
    public TileState tileState = TileState.normal;
    public MaterialState materialState = MaterialState.none;
    public BuildingState buildingState = BuildingState.none;
    public TowerState towerState = TowerState.none;
}
