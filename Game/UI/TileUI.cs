using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MyIsland
{
    public class TileUI : MonoBehaviour
    {
        #region Serialize field
        [SerializeField]
        private Text hpText;
        #endregion


        #region Public Methods
        public void TileUIUpdate(int hp){
            hpText.text = hp.ToString();
        }

        public void TileUISetActive(bool active){
            gameObject.SetActive(active);
            hpText.gameObject.SetActive(active);
        }
        #endregion
    }
}

