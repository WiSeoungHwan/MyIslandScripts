using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyIsland_InGame
{
    public class WoodScope : Bullet
    {

        #region Serialize Field
        [SerializeField]
        private float dotDamageTime;
        [SerializeField]
        private GameObject dotObject;
        #endregion
        #region Private Field
        private ThrowSimulator throwSimulator;
        private int demageTime;
        #endregion

        #region MonoBehaviour CallBacks
        void Start()
        {
            this.throwSimulator = gameObject.AddComponent<ThrowSimulator>();
            if(dotObject)
                dotObject.SetActive(false);
        }
        #endregion

        public override void Fire()
        {
            base.Fire();
            TargetTile.TargetingSetActive(this.TowerKind, true);
            throwSimulator.Shoot(this.transform, this.transform.position, this.TargetTile.transform.position, 10f, 5f, () =>
            {
                Effect.gameObject.SetActive(true);
                TargetTile.TargetingSetActive(this.TowerKind,false);
                Effect.transform.position = new Vector3(transform.position.x, 1f, transform.position.z);
                Effect.Play();
                if(dotObject)
                    dotObject.SetActive(true);
                Hit();
                StartCoroutine("OneSecTimer");
            });
        }
        IEnumerator PositionReset()
        {
            yield return new WaitForSeconds(3.0f);
            Effect.Stop();
            Effect.gameObject.SetActive(false);
            this.transform.position = ReturnPos;
        }

        IEnumerator OneSecTimer()
        {
            yield return new WaitForSeconds(0.5f);
            if (demageTime > dotDamageTime)
            {
                demageTime = 0;
                if(dotObject)
                    dotObject.SetActive(false);
                StartCoroutine("PositionReset");
                StopCoroutine("OneSecTimer");
                yield return null;
            }
            demageTime++;
            if (TargetTile.tileData.tileState == TileState.BUILDING)
            {
                TargetTile.TileHurt(Damage);
            }
            EventManager.Instance.emit(EVENT_TYPE.TILE_HIT, this, TargetTile);
            StartCoroutine("OneSecTimer");
        }
    }
}
