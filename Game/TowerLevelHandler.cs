using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerLevelHandler : MonoBehaviour
{
    #region SerializeField
    [SerializeField]
    List<GameObject> table = new List<GameObject>();
    [SerializeField]
    GroundTile bunker;
    [SerializeField]
    GroundTile woodTower;
    [SerializeField]
    GroundTile stoneTower;
    [SerializeField]
    GroundTile ironTower;
    [SerializeField]
    GroundTile adamTower;

    #endregion

    #region Public Methods

    public GroundTile GetTower(int level){
        switch (level){
            case 0:
                return woodTower;
            case 1:
                return stoneTower;
            case 2:
                return ironTower;
            case 3:
                return adamTower;
            default:
            return woodTower;
        }
    }

    public GameObject GetTable(int level){
        switch (level){
            case 0:
                return table[0];
            case 1:
                return table[1];
            case 2:
                return table[2];
            case 3:
                return table[3];
            default:
            return table[0];
        }
    }

    #endregion
}
