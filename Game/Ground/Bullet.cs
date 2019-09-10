using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MyIsland
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
        #endregion

        #region Public Methods
        public virtual void Fire(){
            
        }
        public void Targeting(Tile tile, float damage,TowerKind towerKind){
            _targetTile = tile;
            this._damage = damage;
            _towerKind = towerKind;
            Fire();
        }
        #endregion
    }
}

