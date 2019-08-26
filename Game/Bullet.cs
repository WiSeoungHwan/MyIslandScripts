﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    #region Private Field
    private Tile destination;
    private ThrowSimulator throwSimulator;
    #endregion


    #region Public Methods

    public void BulletInit(Tile destination){
        this.destination = destination;
        this.throwSimulator = gameObject.AddComponent<ThrowSimulator>();
    }

    public void Fire(GameObject effect){
        throwSimulator.Shoot(this.transform,this.transform.position,destination.transform.position,10f,3f, ()=> {
            GameManager.Instance.SendMessage("IsPlayerHit", destination);
            if(destination.tileData.tileState == TileState.building){
                destination.TileHit(5);
            }
            var effectObject = Instantiate(effect,destination.transform.position, Quaternion.identity);
            effectObject.SetActive(true);
            destination.TileTargeting(false);
            Destroy(gameObject);
            });
    }

    #endregion
}
