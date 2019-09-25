using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MyIsland_Main
{
    public enum SubButtons{
        CHAR,
        TOWER,
        STORE,
        SETTING,
        RANK
    }
    public class SubButtonsPanel : MonoBehaviour
    {
        public void SubButtonsTap(int button){
            AudioManager.Instance.PlaySfx("SubButtons_01");
            switch ((SubButtons)button){
                case SubButtons.CHAR:
                break;
                case SubButtons.TOWER:
                break;
                case SubButtons.STORE:
                break;
                case SubButtons.SETTING:
                break;
                case SubButtons.RANK:
                break;
            }
        }
    }

}
