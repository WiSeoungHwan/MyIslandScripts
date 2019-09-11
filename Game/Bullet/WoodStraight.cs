using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyIsland
{
    public class WoodStraight : Bullet
    {
        #region Private Field
        private bool isShoot;
        #endregion

        #region Collider
        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player")
            {
                isShoot = false;
                EventManager.Instance.emit(EVENT_TYPE.TILE_HIT,this,TargetTile);
                Effect.Play();
                StartCoroutine("PositionReset");
                
            }
        }
        #endregion
        #region Public Methods
        public override void Fire()
        {
            base.Fire();
            Effect.gameObject.SetActive(true);
            Effect.Play();
            isShoot = true;
        }
        #endregion

        #region Private Methods

        private void Shoot()
        {
            if (this.TargetTile.tileData.isPlayerGround)
            {
                transform.Translate(new Vector3(this.TargetTile.transform.position.x * 4f * Time.deltaTime, 0f, 0f), Space.World);
                if (this.transform.position.x >= this.TargetTile.transform.position.x + 5f)
                {
                    isShoot = false;
                    // foreach (var i in routes)
                    // {
                    //     i.TileTargeting(false);
                    // }
                    this.transform.localPosition = new Vector3(0f, 0.5f, 0);
                    Effect.gameObject.SetActive(false);
                    Effect.Stop();

                }
            }
            else
            {
                transform.Translate(new Vector3(this.TargetTile.transform.position.x * 4f * Time.deltaTime, 0f, 0f), Space.World);
                if (this.transform.position.x <= this.TargetTile.transform.position.x - 5f)
                {

                    isShoot = false;
                    // foreach (var i in routes)
                    // {
                    //     i.TileTargeting(false);
                    // }
                    this.transform.localPosition = new Vector3(0f, 0.5f, 0);
                    Effect.gameObject.SetActive(false);
                    Effect.Stop();
                }
            }
        }

        IEnumerator PositionReset()
        {
            yield return new WaitForSeconds(2.0f);
            this.transform.localPosition = new Vector3(0f, 0.5f, 0);
            Effect.gameObject.SetActive(false);
            Effect.Stop();
        }

        #endregion

        #region MonoBehaviour Callback
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
