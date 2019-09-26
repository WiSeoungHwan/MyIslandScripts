using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MyIsland_InGame
{
    public enum TowerEnum{
        TOWER_1,
        TOWER_2,
        TOWER_3
    }

    public class Tower : Building
    {
        #region Serialize Field
        [SerializeField]
        private TowerData _towerData;
        [SerializeField]
        private Bullet bullet;
        [SerializeField]
        private string _TID;
        [SerializeField]
        private ParticleSystem launchEffect;
        
        #endregion

        #region Private Field
        private TowerSheetData _towerSheetData;
        #endregion

        #region Properties
        public TowerData TowerData{
            get{
                return _towerData;
            }
            set{
                _towerData = value;
            }
        }
        public string TID{
            get{
                return _TID;
            }
        }
        public TowerSheetData TowerSheetData{
            get{
                return _towerSheetData;
            }
        }
        public bool BuildComplete{get; set;}
        #endregion 

        #region MonoBehaviour CallBack
        void Start(){
            BuildComplete = false;
            EventManager.Instance.on(EVENT_TYPE.GM_FIRE,Fire);
            this.buildingKind = BuildingKind.TOWER;
            
        }
        #endregion
        
        #region Public Methods
        public void Targeting(Tile tile){
            var tag = TowerData.isPlayerGround ? "EnemyPlayer" : "Player";
            bullet.Targeting(tile,TowerData.towerDamage,this.TowerData.towerKind, tag);
        }
        public void StraightTargeting(List<Tile> tiles){
            WoodStraight straight = (WoodStraight)bullet;
            straight.StraightTargeting(tiles);
        }
        public void ScopeTargeting(List<Tile> tiles){
            WoodScope scope = (WoodScope)bullet;
            scope.ScopeTargeting(tiles);
        }
        public void Fire(EVENT_TYPE eventType, Component sender, object param = null){
            if(BuildComplete == false){return;}
            EventManager.Instance.emit(EVENT_TYPE.TOWER_FIRE,this);
        }
        public void TowerInit(bool isPlayerGround, int tileIndex){
            this.TowerData.isPlayerGround = isPlayerGround;
            this.TowerData.tileIndex = tileIndex;
        }
        public void TowerSheetDataInit(TowerSheetData data){
            this._towerSheetData = data;
            TowerData.towerHp = TowerSheetData.Health;
            TowerData.towerDamage = TowerSheetData.Damage;
        }
        #endregion
    }

}
