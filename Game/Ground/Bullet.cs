using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MyIsland
{
    public class Bullet : MonoBehaviour
    {
        #region Private Field
        private Tile _targetTile;
        private float damage;
        #endregion

        #region Properties
        public Tile TargetTile{
            get{
                return _targetTile;
            }
        }
        #endregion

        #region Serialize Field
        [SerializeField]
        private ParticleSystem effect;
        #endregion

        #region Public Methods
        public virtual void Fire(){
            
        }
        public void Targeting(Tile tile, float damage){
            _targetTile = tile;
            this.damage = damage;
            Fire();
        }
        #endregion
    }
}

