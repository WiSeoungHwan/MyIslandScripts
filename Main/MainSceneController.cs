using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace MyIsland_Main
{
    public class MainSceneController : MonoBehaviour
    {
        [SerializeField]
        private GameObject comingSoonPanel;
        #region MonoBehaviour
        void Start(){
            AudioManager.Instance.PlayBgm("Main_01");
        }

        void OnDestroy(){
            AudioManager.Instance.StopBGM();
        }
        #endregion
        public void PlayButtonTap(){
            AudioManager.Instance.PlaySfx("Click_01");
            LoadingSceneManager.LoadScene("SinglePlayGame");
        }
        public void OnComingSoonPanel(){
            comingSoonPanel.SetActive(true);
            var image = comingSoonPanel.GetComponent<Image>();
            image.DOFade(0,1f).OnComplete(OffComingSoonPanel);
        }
        public void OffComingSoonPanel(){
            comingSoonPanel.SetActive(false);
            var image = comingSoonPanel.GetComponent<Image>();
            image.DOFade(0.5f,0f);
        }
    }
}

