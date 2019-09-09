using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MyIsland
{
    public class WoodParabola : Bullet
    {
        #region Private Field
        private ThrowSimulator throwSimulator;
        #endregion

        #region MonoBehaviour CallBacks
        void Start(){
            this.throwSimulator = gameObject.AddComponent<ThrowSimulator>();
        }
        #endregion
        public override void Fire(){
            throwSimulator.Shoot(this.transform,this.transform.position, this.TargetTile.transform.position,10f,3f, () => {
                Debug.Log("Tile Hit");
                this.transform.localPosition = Vector3.zero;
            });
        }   
    }

}
