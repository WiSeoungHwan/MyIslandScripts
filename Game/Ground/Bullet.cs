using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MyIsland_InGame
{
    public class Bullet : MonoBehaviour
    {
        #region Private Field
        private Tile _targetTile;
        private float _damage;
        private TowerKind _towerKind;
        #endregion

        #region Properties
        public Tile TargetTile{
            get{
                return _targetTile;
            }
        }
        public ParticleSystem Effect{
            get{
                return _effect;
            }
        }
        public TowerKind TowerKind{
            get{
                return _towerKind;
            }
        }
        public float Damage{
            get{
                return _damage;
            }
        }
        #endregion

        #region Serialize Field
        [SerializeField]
        private ParticleSystem _effect;
        [SerializeField]
        private GameObject _body;
        #endregion

        #region Public Methods
        public virtual void Fire(){
            _body.SetActive(true);
        }
        public virtual void Hit(){
            _body.SetActive(false);
        }
        public void Targeting(Tile tile, float damage,TowerKind towerKind, string tag){
            this.tag = tag;
            _targetTile = tile;
            this._damage = damage;
            _towerKind = towerKind;
            Fire();
        }
        
        
        #endregion
    }
}

