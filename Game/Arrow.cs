using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    #region Private Field
    private Tile destination;
    private List<Tile> routes = new List<Tile>();
    private bool isShoot;

    #endregion

    #region Collider
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            var playerLocation = other.transform.parent.position;
            if (destination.tileData.isMine)
            {
                if (playerLocation.x >= 0)
                {
                    GameManager.Instance.SendMessage("IsPlayerArrowHit", true);
                }
            }
            else
            {
                if (playerLocation.x < 0)
                {
                    GameManager.Instance.SendMessage("IsPlayerArrowHit", false);
                }
            }


        }
    }
    #endregion

    #region Pubic Methods

    public void TargetSetup(Tile destination, List<Tile> routes)
    {
        this.destination = destination;
        this.routes = routes;
    }

    public void Fire()
    {
        isShoot = true;

    }

    #endregion

    #region Private Methods

    private void Shoot()
    {
        if (destination.tileData.isMine)
        {
            transform.Translate(new Vector3(destination.transform.position.x * 2f * Time.deltaTime, 0f, 0f) , Space.World);
            if (this.transform.position.x >= destination.transform.position.x + 3)
            {
                isShoot = false;
                foreach (var i in routes)
                {
                    i.TileTargeting(false);
                }
                Destroy(gameObject);
            }
        }
        else
        {
            transform.Translate(new Vector3(destination.transform.position.x * 1f * Time.deltaTime, 0f, 0f) , Space.World);
            if (this.transform.position.x <= destination.transform.position.x - 3)
            {

                isShoot = false;
                foreach (var i in routes)
                {
                    i.TileTargeting(false);
                }
                Destroy(gameObject);
            }
        }
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
