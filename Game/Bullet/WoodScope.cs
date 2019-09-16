using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyIsland
{
    public class WoodScope : Bullet
    {

        #region Serialize Field
        [SerializeField]
        private ParticleSystem[] scopeEffects;
        #endregion
        #region Private Field
        private ThrowSimulator throwSimulator;
        private List<Tile> scopes = new List<Tile>();
        private int demageTime;
        #endregion

        #region MonoBehaviour CallBacks
        void Start()
        {
            this.throwSimulator = gameObject.AddComponent<ThrowSimulator>();
        }
        #endregion

        public override void Fire()
        {
            TargetingSetActive(true, scopes);
            throwSimulator.Shoot(this.transform, this.transform.position, this.TargetTile.transform.position, 10f, 5f, () =>
            {
                TargetingSetActive(false, scopes);
                Effect.gameObject.SetActive(true);

                Effect.transform.position = new Vector3(transform.position.x, 1f, transform.position.z);
                Effect.Play();
                StartCoroutine("OneSecTimer");
                StartCoroutine("PositionReset");
            });
        }


        public void ScopeTargeting(List<Tile> tiles)
        {
            scopes = tiles;
        }

        private void TargetingSetActive(bool active, List<Tile> tiles)
        {
            foreach (var i in tiles)
            {
                i.TargetingSetActive(this.TowerKind, active);
            }
        }

        IEnumerator PositionReset()
        {
            yield return new WaitForSeconds(3.0f);
            Effect.Stop();
            foreach (var e in scopeEffects)
            {
                e.gameObject.SetActive(false);
                e.Stop();
            }
            Effect.gameObject.SetActive(false);
            this.transform.localPosition = Vector3.zero;
        }
        IEnumerator OneSecTimer()
        {
            yield return new WaitForSeconds(1);
            if (demageTime > 2)
            {
                demageTime = 0;
                StopCoroutine("OneSecTimer");
                yield return null;
            }
            demageTime++;
            foreach (var i in scopes)
            {
                if (i.tileData.tileState == TileState.BUILDING)
                {
                    i.TileHurt(1);
                }
                EventManager.Instance.emit(EVENT_TYPE.TILE_HIT,this, i);
            }

            
            StartCoroutine("OneSecTimer");
        }
    }
}
