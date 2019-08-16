using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitUI : MonoBehaviour
{
    #region SerializeField

    [SerializeField]
    private Text woodText;

    [SerializeField]
    private Text stoneText;

    [SerializeField]
    private Text ironText;

    [SerializeField]
    private Text adamantiumText;

    [SerializeField]
    private Text unitActiveCount;

    [SerializeField]
    private Text unitHP;
    [SerializeField]
    private GameObject practicableArea;
    [SerializeField]
    private PiUIManager piUIManager;
    [SerializeField]
    private Slider unitHpSlider;

    #endregion

    #region Private Field
    private Vector3 buildUIPosition;
    private PlayerScript player;
    #endregion

    #region Delegate
    public delegate void TowerSelected(TowerState towerState);
    private TowerSelected towerSelected = null;
    
    #endregion

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

    public void UnitUIInit(PlayerScript player){
        this.player = player;
        
    }

    public void SetCallbacks(TowerSelected towerSelected){
        this.towerSelected = towerSelected;
    }

    public void MaterialUIUpdate(PlayerData playerData)
    {
        woodText.text = "Wood: " + playerData.materialData.wood;

        stoneText.text = "Stone: " + playerData.materialData.stone;

        ironText.text = "Iron: " + playerData.materialData.iron;

        adamantiumText.text = "Adam: " + playerData.materialData.adamantium;

    }
    
    public void UintUIUpdate(PlayerData playerData){
        unitActiveCount.text = playerData.activeCount.ToString();
        unitHP.text = playerData.hp.ToString();
        unitHpSlider.value = playerData.hp;
    }


    // - Normal UI Button Func
    public void BuildModeToggle(){
        player.CheckAround();
        practicableArea.SetActive(practicableArea.active ? false : true);
    }

    public void BuildUIActive(Vector3 clickPoint){
        buildUIPosition = Camera.main.WorldToScreenPoint(clickPoint);
		piUIManager.ChangeMenuState("NormalMenu",buildUIPosition);
    }

    public void BuildUIUnActive(){
        if(piUIManager.PiOpened("NormalMenu"))
            piUIManager.ChangeMenuState("NormalMenu");
        if(piUIManager.PiOpened("TowerMenu"))
            piUIManager.ChangeMenuState("TowerMenu");
    }

    public void TowerUITap(){
        piUIManager.ChangeMenuState("NormalMenu");
        piUIManager.ChangeMenuState("TowerMenu",buildUIPosition);
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
            default:
                state = TowerState.none;
                break;
        }
        towerSelected(state);
        BuildUIUnActive();
        BuildModeToggle();
    }


    #endregion
}
