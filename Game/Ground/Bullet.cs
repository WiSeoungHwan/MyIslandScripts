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
        public Vector3 ReturnPos{get;set;}
        #endregion

        #region Serialize Field
        [SerializeField]
        private ParticleSystem _effect;
        [SerializeField]
        private AudioClip _sound;
        [SerializeField]
        private GameObject _body;
        #endregion

        #region Public Methods
        public virtual void Fire(){
            _body.SetActive(true);
            ReturnPos = gameObject.transform.position;
        }
        public virtual void Hit(){
            if(_sound)
                AudioManager.Instance.PlaySfx(_sound);
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

