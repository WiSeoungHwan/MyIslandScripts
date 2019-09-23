using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyIsland_InGame
{
    public class WoodStraight : Bullet
    {
        #region Private Field
        private bool isShoot;
        private List<Tile> routes = new List<Tile>();
        private List<Vector3> routesPosition = new List<Vector3>();
        private BoxCollider collider;
        bool isHiting;
        #endregion

        #region Collider
        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == this.tag)
            {
                this.Hit();
            }
        }
        #endregion
        #region Public Methods
        public override void Fire()
        {
            base.Fire();
            isShoot = true;
            collider.enabled = true;
        }
        public override void Hit()
        {
            base.Hit();
            foreach (var i in routes)
            {
                i.TargetingSetActive(TowerKind.STRAIGHT, false);
            }
            isShoot = false;
            EventManager.Instance.emit(EVENT_TYPE_SINGLE.TILE_HIT, this, TargetTile);
            Effect.gameObject.SetActive(true);
            Effect.Play();
            collider.enabled = false;
            StartCoroutine("PositionReset");
        }
        public void StraightTargeting(List<Tile> tiles)
        {
            routes = tiles;
            foreach (var i in routes)
            {
                routesPosition.Add(i.transform.position);
            }
        }
        #endregion

        #region Private Methods

        private void Shoot()
        {
            if (this.TargetTile.tileData.isPlayerGround)
            {
                transform.Translate(new Vector3((this.TargetTile.transform.position.x + 5f) * 2.5f * Time.deltaTime, 0f, 0f), Space.World);

                foreach (var i in routes)
                {
                    if (i.transform.position.x == (int)transform.position.x)
                    {
                        if (i.tileData.tileState == TileState.BUILDING)
                        {
                            if (!isHiting)
                            {
                                isHiting = true;
                                i.TileHurt(Damage);
                                foreach (var r in routes)
                                {
                                    r.TargetingSetActive(TowerKind.STRAIGHT, false);
                                }
                                isShoot = false;
                                base.Hit();
                                Effect.gameObject.SetActive(true);
                                Effect.Play();
                                collider.enabled = false;
                                StartCoroutine("PositionReset");
                            }
                        }
                    }
                }
                if (this.transform.position.x >= this.TargetTile.transform.position.x + 5f)
                {
                    isShoot = false;
                    foreach (var i in routes)
                    {
                        i.TargetingSetActive(TowerKind.STRAIGHT, false);
                    }
                    this.transform.localPosition = new Vector3(0f, 0.5f, 0);
                    Effect.gameObject.SetActive(false);
                    isHiting = false;
                    Effect.Stop();
                }
            }
            else
            {
                transform.Translate(new Vector3(this.TargetTile.transform.position.x * 4f * Time.deltaTime, 0f, 0f), Space.World);
                foreach (var i in routes)
                {
                    if (i.transform.position.x == (int)transform.position.x)
                    {
                        if (i.tileData.tileState == TileState.BUILDING)
                        {
                            if (!isHiting)
                            {
                                isHiting = true;
                                i.TileHurt(Damage);
                                foreach (var r in routes)
                                {
                                    r.TargetingSetActive(TowerKind.STRAIGHT, false);
                                }
                                base.Hit();
                                isShoot = false;
                                Effect.gameObject.SetActive(true);
                                Effect.Play();
                                collider.enabled = false;

                                StartCoroutine("PositionReset");
                            }
                        }
                    }
                }
                if (this.transform.position.x <= this.TargetTile.transform.position.x - 5f)
                {

                    isShoot = false;
                    foreach (var i in routes)
                    {
                        i.TargetingSetActive(TowerKind.STRAIGHT, false);
                    }
                    this.transform.localPosition = new Vector3(0f, 0.5f, 0);
                    Effect.gameObject.SetActive(false);
                    isHiting = false;
                    Effect.Stop();
                }
            }
        }

        IEnumerator PositionReset()
        {
            yield return new WaitForSeconds(2.0f);
            this.transform.localPosition = new Vector3(0f, 0.5f, 0);
            Effect.gameObject.SetActive(false);
            isHiting = false;
            Effect.Stop();
        }

        #endregion

        #region MonoBehaviour Callback
        void Start()
        {
            collider = GetComponent<BoxCollider>();
        }
        void Update()
        {
            if (isShoot)
            {
                Shoot();
            }

        }
        #endregion
    }
}
