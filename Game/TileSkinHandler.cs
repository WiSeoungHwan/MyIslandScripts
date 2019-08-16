using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSkinHandler : MonoBehaviour
{

    #region SerializeField

    [SerializeField]
    int selectNum = 0;
    
    [SerializeField]
    GroundTile[] groundTile;
    [SerializeField]
    GroundTile[] materialWood;
    [SerializeField]
    GroundTile[] materialStone;
    [SerializeField]
    GroundTile[] materialIron;
    [SerializeField]
    GroundTile[] materialAdam;

    #endregion

    #region MonoBehaviour

    #endregion

    #region Public Methods

    public SkinData GetSkinData(){
        var skinData = new SkinData();
        
        skinData.ground = groundTile[selectNum];
        skinData.wood = materialWood[selectNum];
        skinData.stone = materialStone[selectNum];
        skinData.iron = materialIron[selectNum];
        skinData.adam = materialAdam[selectNum];
        return skinData;
    }
    #endregion 


}
