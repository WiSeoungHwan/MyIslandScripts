using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerLevelHandler : MonoBehaviour
{
    #region SerializeField
    [SerializeField]
    List<GameObject> table = new List<GameObject>();
    [SerializeField]
    List<GameObject> bunker = new List<GameObject>();
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

    public GameObject GetBunker(int level){
        switch (level){
            case 0:
                return bunker[0];
            case 1:
                return bunker[1];
            case 2:
                return bunker[2];
            case 3:
                return bunker[3];
            default:
            return bunker[0];
        }
    }

    #endregion
}
