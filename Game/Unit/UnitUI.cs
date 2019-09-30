using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace MyIsland_InGame
{
    public class UnitUI : MonoBehaviour
    {
        #region Serialize Field
        [SerializeField]
        private Canvas unitCanvas;
        [SerializeField]
        private Text unitHpText;

        [SerializeField]
        private Image unitHpImage;
        [SerializeField]
        private Image unitStaminaImage;
        [SerializeField]
        private Image unitStaminaCoolTimeImage;
        [SerializeField]
        private Transform unitStaminaTrans;
        [SerializeField]
        private RectTransform messegePenel;
        [SerializeField]
        private Text messegeText;
        [SerializeField]
        private TextMove headPopUp;
        
        #endregion

        #region Delegate
        public delegate int StaminaUp();

        private StaminaUp staminaUp;
        #endregion

        #region Private Field
        private int curStamina;
        private float coolTime;
        private Vector3 beforeStaminaPos;
        private bool isShaking;
        private bool isMessegeShowing;
        private bool isHeadPopUpShowing;
        #endregion
        #region MonoBehaviour
        void Start(){
            headPopUp.gameObject.SetActive(false);
        }
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
            if(stamina == 0){
                beforeStaminaPos = unitStaminaTrans.position;
                if(!isShaking){
                    unitStaminaTrans.DOShakePosition(0.3f,new Vector3(15f,0,0)).OnComplete(ResetShake);
                    isShaking = true;
                }
                unitStaminaImage.fillAmount = stamina * 0.2f;
                return;
            }
            StopCoroutine("StaminaCoolTime");
            coolTime = 0f;
            unitStaminaImage.fillAmount = stamina * 0.2f;
            unitStaminaCoolTimeImage.fillAmount = unitStaminaImage.fillAmount;
            StartCoroutine("StaminaCoolTime");
        }

        public void ShowMessege(string messege){
            messegePenel.gameObject.SetActive(true);
            messegeText.text = messege;
            var beforePos = messegePenel.position;
            if(!isMessegeShowing)
                messegePenel.DOAnchorPosY(-35f,1f, true).SetEase(Ease.InOutQuad).OnComplete(()=>HideMessege(messegePenel,beforePos));
            isMessegeShowing = true;
        }

        public void ShowHeadPopUp(string text){
            TextMove instance = Instantiate(headPopUp);
            instance.transform.SetParent(unitCanvas.transform, false);
            instance.gameObject.SetActive(true);
            instance.SetText(text);
            
        }
        private void ShowHeadPopUpComplete(Vector3 pos){
            headPopUp.gameObject.SetActive(false);
            // headPopUp.text = "";
            headPopUp.transform.position = pos;
            isHeadPopUpShowing = false;
        }
        
        
        IEnumerator StaminaCoolTime()
        {
            yield return new WaitForSeconds(0.01f);
            coolTime += 0.01f;
            if(coolTime >= 0.2f){
                unitStaminaImage.fillAmount = staminaUp() * 0.2f;
                coolTime = 0f;
            }
            unitStaminaCoolTimeImage.fillAmount = unitStaminaImage.fillAmount + coolTime;
            StartCoroutine("StaminaCoolTime");
        }
        #endregion

        #region Private Methods
        private void ResetShake(){
            transform.position = beforeStaminaPos;
            isShaking = false;
        }
        private void HideMessege(RectTransform trans,Vector3 beforPos){
            trans.gameObject.SetActive(false);
            trans.position = beforPos;
            isMessegeShowing = false;
        }
        #endregion
    }

}
