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
        if(groundTile.Length > selectNum)
            skinData.ground = groundTile[selectNum];
        //임시 
        skinData.wood = materialWood[0];
        skinData.stone = materialStone[0];
        // if(materialWood.Length > selectNum)
        //     skinData.wood = materialWood[selectNum];
        // if(materialStone.Length > selectNum)
        //     skinData.stone = materialStone[selectNum];
        // if(materialIron.Length > selectNum)
        //     skinData.iron = materialIron[selectNum];
        // if(materialAdam.Length > selectNum)
        //     skinData.adam = materialAdam[selectNum];
        return skinData;
    }
    #endregion 


}
