﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitUI : MonoBehaviour
{
    #region SerializeField

    [SerializeField]
    private Canvas unitCanvas;
    [SerializeField]
    private Text woodText;

    [SerializeField]
    private Text stoneText;

    [SerializeField]
    private Text ironText;

    [SerializeField]
    private Text adamantiumText;


    [SerializeField]
    private Text unitHP;
    [SerializeField]
    private GameObject practicableArea;

    [SerializeField]
    private Slider unitHpSlider;
    [SerializeField]
    private TextMove headPopup;
    [SerializeField]
    private GameObject towerUnActivePanel;
    
    private TowerLevelHandler towerLevelHandler;

    #endregion

    #region Private Field
    private Vector3 buildUIPosition;
    private PlayerScript player;
    #endregion

    #region Delegate
    public delegate void TowerSelected(TowerState towerState);
    private TowerSelected towerSelected = null;
    
    #endregion

    public TowerState state;

    #region MonoBehaviourCallBacks

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    #endregion

    #region Public Methods

    public void UnitUIInit(PlayerScript player, TowerLevelHandler towerLevelHandler){
        this.player = player;
        this.towerLevelHandler = towerLevelHandler;
    }

    public void SetCallbacks(TowerSelected towerSelected){
        this.towerSelected = towerSelected;
    }

    public void MaterialUIUpdate(PlayerData playerData)
    {
        woodText.text = "W " + playerData.materialData.wood;

        stoneText.text = "S " + playerData.materialData.stone;

        ironText.text = "I  " + playerData.materialData.iron;

        adamantiumText.text = "A " + playerData.materialData.adamantium;

    }
    
    public void UnitUIUpdate(PlayerData playerData){
        unitHP.text = playerData.hp.ToString();
        unitHpSlider.value = playerData.hp;
        if(playerData.tableCount > 0){
            towerUnActivePanel.SetActive(false);
        }else{
            towerUnActivePanel.SetActive(true);
        }
    }
    public void UnitHeadPopUpActive(string text, Color color){
        TextMove instance = Instantiate(this.headPopup);
        instance.transform.SetParent(unitCanvas.transform, false);
        instance.SetText(text, color);
    }


    // - Normal UI Button Func
    public void BuildModeToggle(string towerState){
        // TODO: - tower 레벨별 상황 분기하기 (현재는 우선 플레이어 레벨 1 상황)
        GameObject tower = towerLevelHandler.GetTower(0).data[0];
        
       
        switch(towerState){
            case "Parabola":
                state = TowerState.parabola;
                tower = towerLevelHandler.GetTower(0).data[0];
                break;
            case "Scope":
                state = TowerState.scope;
                tower = towerLevelHandler.GetTower(0).data[2];
                break;
            case "Straight":
                state = TowerState.straight;
                tower = towerLevelHandler.GetTower(0).data[1];
                break;
            case "Table":
                state = TowerState.table;
                tower = towerLevelHandler.GetTable(0);
                break;
            case "Bunker":
                state = TowerState.bunker;
                tower = towerLevelHandler.GetBunker(0);
                break;
            default:
                state = TowerState.none;
                break;
        }
        player.CheckAround(tower);
        practicableArea.SetActive(practicableArea.active ? false : true);
    }
    public void BuildModeOff(){
        practicableArea.SetActive(false);
    }



    // - Tower button func 

    
    public void TowerSelectButtonTap(string towerState){
        TowerState state;
        switch(towerState){
            case "Parabola":
                state = TowerState.parabola;
                break;
            case "Scope":
                state = TowerState.scope;
                break;
            case "Straight":
                state = TowerState.straight;
                break;
            case "Table":
                state = TowerState.table;
                break;
            case "Bunker":
                state = TowerState.bunker;
                break;
            default:
                state = TowerState.none;
                break;
        }
        // towerSelected(state);
    }


    #endregion
}
