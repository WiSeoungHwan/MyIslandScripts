using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MyIsland
{
    public delegate void UpgradeButtonTap();

    public class TileUI : MonoBehaviour
    {
        #region Serialize field
        [SerializeField]
        private Text hpText;
        [SerializeField]
        private GameObject tablePanel;
        #endregion

        #region Delegate

        private UpgradeButtonTap upgradeButtonTap;
        #endregion


        #region Public Methods
        public void SetDelegate(UpgradeButtonTap upgradeButtonTap){
            this.upgradeButtonTap = upgradeButtonTap;
        }
        public void TileUIUpdate(int hp){
            hpText.text = hp.ToString();
        }

        public void TileUISetActive(bool active){
            gameObject.SetActive(active);
            hpText.gameObject.SetActive(active);
        }
        public void TablePanelSetActive(){
            tablePanel.SetActive(tablePanel.activeInHierarchy ? false : true);
        }
        public void UpgradeButton(){
            TablePanelSetActive();
            upgradeButtonTap();
        }
        #endregion
    }
}

