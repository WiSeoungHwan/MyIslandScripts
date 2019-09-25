using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



namespace MyIsland_InGame
{
    public class MaterialInfoPanel : MonoBehaviour
    {
        #region Serialize Field
        [SerializeField]
        private Text woodText;
        [SerializeField]
        private Text stoneText;
        [SerializeField]
        private Text ironText;
        [SerializeField]
        private Text adamText;
        #endregion

        #region MonoBehaviour Methods
        void Start()
        {
            Debug.Log("Panel Start");
            EventManager.Instance.on(EVENT_TYPE.MATERIAL_COLLECT, UnitMaterialDataUpdate);
        }

        void OnDestroy()
        {
            EventManager.Instance.off(EVENT_TYPE.MATERIAL_COLLECT, UnitMaterialDataUpdate);
        }
        #endregion

        #region Public Methods
        public void UnitMaterialDataUpdate(EVENT_TYPE eventType, Component sender, object param = null)
        {
            UnitMaterialData data = (UnitMaterialData)param;
            woodText.text = data.wood.ToString();
            stoneText.text = data.stone.ToString();
            ironText.text = data.iron.ToString();
            adamText.text = data.adam.ToString();
        }
        #endregion
    }
}
