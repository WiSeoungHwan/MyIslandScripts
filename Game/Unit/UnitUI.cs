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
        [SerializeField]
        private Image unitStaminaImage;
        [SerializeField]
        private Image unitStaminaCoolTimeImage;
        #endregion

        #region Delegate
        public delegate int StaminaUp();

        private StaminaUp staminaUp;
        #endregion

        #region Private Field
        private int curStamina;
        private float coolTime;
        #endregion

        #region Public Methods
        public void SetDelegate(StaminaUp staminaUp){
            this.staminaUp = staminaUp;
        }
        public void UnitUIUpdate(UnitData unitData)
        {
            unitHpText.text = unitData.unitLevel.ToString();
            unitHpImage.fillAmount = (unitData.unitMaxHp * unitData.unitHp) * 0.01f;
        }

        public void UnitStaminaDiscount(int stamina)
        {
            Debug.Log(stamina);
            StopCoroutine("StaminaCoolTime");
            coolTime = 0f;
            unitStaminaImage.fillAmount = stamina * 0.2f;
            unitStaminaCoolTimeImage.fillAmount = unitStaminaImage.fillAmount;
            StartCoroutine("StaminaCoolTime");
        }
        IEnumerator StaminaCoolTime()
        {
            yield return new WaitForSeconds(0.01f);
            coolTime += 0.003f;
            if(coolTime >= 0.2f){
                unitStaminaImage.fillAmount = staminaUp() * 0.2f;
                coolTime = 0f;
            }
            unitStaminaCoolTimeImage.fillAmount = unitStaminaImage.fillAmount + coolTime;
            StartCoroutine("StaminaCoolTime");
        }
        #endregion
    }

}
