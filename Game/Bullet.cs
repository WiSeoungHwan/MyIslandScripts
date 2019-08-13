using System.Collections;
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

    public void Fire(){
        throwSimulator.Shoot(this.transform,this.transform.position,destination.transform.position,10f,3f, ()=> {
            Debug.Log("Sucess");
            Destroy(gameObject);
            });
    }

    #endregion
}
