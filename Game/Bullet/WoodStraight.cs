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
                transform.Translate(new Vector3(this.TargetTile.transform.position.x * 2f * Time.deltaTime, 0f, 0f), Space.World);
                if (this.transform.position.x >= this.TargetTile.transform.position.x + 5f)
                {
                    isShoot = false;
                    // foreach (var i in routes)
                    // {
                    //     i.TileTargeting(false);
                    // }
                    this.transform.localPosition = new Vector3(0f,0.5f,0);
                    Effect.gameObject.SetActive(false);
                    Effect.Stop();
                   
                }
            }
            else
            {
                transform.Translate(new Vector3(this.TargetTile.transform.position.x * 1f * Time.deltaTime, 0f, 0f), Space.World);
                if (this.transform.position.x <= this.TargetTile.transform.position.x - 5f)
                {

                    isShoot = false;
                    // foreach (var i in routes)
                    // {
                    //     i.TileTargeting(false);
                    // }
                    this.transform.localPosition = new Vector3(0f,0.5f,0);
                    Effect.gameObject.SetActive(false);
                    Effect.Stop();
                }
            }
        }

        IEnumerator PositionReset()
        {
            yield return new WaitForSeconds(2.0f);
            
        }

        #endregion

        #region MonoBehaviour
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
