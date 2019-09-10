using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MyIsland
{
    public class UnitUI : MonoBehaviour
    {
        #region Serialize Field
        [SerializeField]
        private Text unitHpText;

        [SerializeField]
        private Image unitHpImage;
        #endregion


        #region Public Methods
        public void UnitUIUpdate(UnitData unitData)
        {
            unitHpText.text = unitData.unitHp.ToString();
            unitHpImage.fillAmount = (unitData.unitMaxHp * unitData.unitHp) * 0.01f;
        }
        #endregion
    }

}
