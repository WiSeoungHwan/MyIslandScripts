using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace MyIsland
{

    public class BuildPanel : MonoBehaviour
    {
        #region Serialize Field
        [SerializeField]
        private Button tableButton;
        [SerializeField]
        private Button bunkerButton;
        [SerializeField]
        private Button parabolaButton;
        [SerializeField]
        private Button straightButton;
        [SerializeField]
        private Button scopeButton;
        #endregion

        #region Private Field
        private int currentBuiding;
        #endregion

        #region MonoBehaviour CallBack
        void Start()
        {
            EventManager.Instance.on(EVENT_TYPE.TABLE_COUNT_CHANGE, TableCountChange);
            bunkerButton.gameObject.SetActive(false);
            parabolaButton.gameObject.SetActive(false);
            straightButton.gameObject.SetActive(false);
            scopeButton.gameObject.SetActive(false);
        }
        void OnDestory()
        {
            EventManager.Instance.off(EVENT_TYPE.TABLE_COUNT_CHANGE, TableCountChange);
        }
        #endregion

        #region Private Methods
        private void TableCountChange(EVENT_TYPE eventType, Component sender, object param = null)
        {
            int tableCount = (int)param;
            bool active = false;
            if (tableCount <= 0)
            {
                active = false;
            }
            else
            {
                active = true;
            }
            bunkerButton.gameObject.SetActive(active);
            parabolaButton.gameObject.SetActive(active);
            straightButton.gameObject.SetActive(active);
            scopeButton.gameObject.SetActive(active);
        }
        #endregion

        #region Public Methods
        public void TowerBuildButtonTap(int building)
        {
            if (currentBuiding == building)
            {
                EventManager.Instance.emit(EVENT_TYPE.WILL_BUILD_OFF, this);
                currentBuiding = 5;
            }
            else
            {
                EventManager.Instance.emit(EVENT_TYPE.TOWER_WILL_BUILD, this, (TowerEnum)building);
                currentBuiding = building;
            }
        }
        public void TableBuildButtonTap(int building)
        {
            if (currentBuiding == building)
            {
                EventManager.Instance.emit(EVENT_TYPE.WILL_BUILD_OFF, this);
                currentBuiding = 5;
            }
            else
            {
                EventManager.Instance.emit(EVENT_TYPE.TABLE_WILL_BUILD, this);
                currentBuiding = building;
            }
        }
        public void BunkerBuildButtonTap(int building)
        {
            if (currentBuiding == building)
            {
                EventManager.Instance.emit(EVENT_TYPE.WILL_BUILD_OFF, this);
                currentBuiding = 5;
            }
            else
            {
                EventManager.Instance.emit(EVENT_TYPE.BUNKER_WILL_BUILD, this);
                currentBuiding = building;
            }
        }
        #endregion
    }

}