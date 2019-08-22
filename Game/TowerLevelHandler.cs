using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerLevelHandler : MonoBehaviour
{
    #region SerializeField
    [SerializeField]
    GroundTile table;
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

    #endregion
}
