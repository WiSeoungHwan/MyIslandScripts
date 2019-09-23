using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MyIsland_InGame
{
    public class WoodParabola : Bullet
    {
        #region Private Field
        private ThrowSimulator throwSimulator;
        #endregion

        #region MonoBehaviour CallBacks
        void Start()
        {
            this.throwSimulator = gameObject.AddComponent<ThrowSimulator>();
        }
        #endregion
        public override void Fire()
        {
            TargetTile.TargetingSetActive(this.TowerKind,true);
            throwSimulator.Shoot(this.transform, this.transform.position, this.TargetTile.transform.position, 10f, 5f, () =>
            {
                TargetTile.TargetingSetActive(this.TowerKind,false);
                Effect.gameObject.SetActive(true);
                Effect.transform.position = new Vector3(transform.position.x, 1f, transform.position.z);
                Effect.Play();
                EventManager.Instance.emit(EVENT_TYPE_SINGLE.TILE_HIT,this,TargetTile);
                StartCoroutine("PositionReset");
            });
        }

        IEnumerator PositionReset()
        {
            yield return new WaitForSeconds(2.0f);
            Effect.Stop();
            this.transform.localPosition = Vector3.zero;
        }
    }

}
