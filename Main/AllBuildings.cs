using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyIsland_InGame;
public class AllBuildings : DontDestroy<AllBuildings>
{
    [SerializeField]
    private List<Tower> alltowers;
    [SerializeField]
    private TowerSheet towerSheet;
    private Dictionary<string,Tower> towerPrefabsDic = new Dictionary<string, Tower>();
    private Dictionary<string, TowerSheetData> towerDataDic = new Dictionary<string, TowerSheetData>();
    private List<string> selectedTowerIDs;
    
    #region MonoBehaviour
    void Start(){
        Debug.Log("Deck Setting");
        foreach(var tower in alltowers){
            towerPrefabsDic.Add(tower.TID,tower);
        }
        
        foreach(var data in towerSheet.dataArray){
            towerDataDic.Add(data.TID, data);
        }
        selectedTowerIDs = new List<string>(){
            "TWPa1_Lv3",
            "TWSt1_Lv1",
            "TWSc1_Lv1",
            "TRPa1_Lv1",
            "TRSt1_Lv1",
            "TRSc1_Lv1",
            "TIPa1_Lv1",
            "TISt1_Lv1",
            "TISc1_Lv1",
            "TAPa1_Lv1",
            "TASt1_Lv1",
            "TASc1_Lv1"
        };
    }
    #endregion

    #region Public Methods
    public GameObject[] GetSelectedTowerPrefabs(){
        List<GameObject> towerPrefabs = new List<GameObject>();
        for(int i = 0; i < selectedTowerIDs.Count; i++){
            var key = selectedTowerIDs[i].Split('_');
            if(towerPrefabsDic.ContainsKey(key[0])){
                Tower tower = towerPrefabsDic[key[0]];
                tower.TowerSheetDataInit(towerDataDic[selectedTowerIDs[i]]);
                towerPrefabs.Add(tower.gameObject);
            }
        }
        return towerPrefabs.ToArray();
    }
    #endregion
}
