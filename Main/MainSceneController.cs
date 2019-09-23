using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MyIsland_Main
{
    public class MainSceneController : MonoBehaviour
    {
        public void PlayButtonTap(){
            LoadingSceneManager.LoadScene("SinglePlayGame");
        }
    }
}

