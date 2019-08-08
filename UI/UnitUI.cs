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

    public void MaterialUIUpdate(PlayerData playerData)
    {
        woodText.text = "Wood: " + playerData.materialData.wood;

        stoneText.text = "Stone: " + playerData.materialData.stone;

        ironText.text = "Iron: " + playerData.materialData.iron;

        adamantiumText.text = "Adam: " + playerData.materialData.adamantium;

    }

    #endregion
}
