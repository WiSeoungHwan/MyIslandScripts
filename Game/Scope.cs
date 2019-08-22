using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scope : MonoBehaviour
{
    #region Private Field
    private Tile destination;
    private ThrowSimulator throwSimulator;
    private List<Tile> scope = new List<Tile>();

    #endregion

    #region Public Field

    public void ScopeInit(Tile destination, List<Tile> scope){
        this.destination = destination;
        this.scope = scope;
        this.throwSimulator = gameObject.AddComponent<ThrowSimulator>();
    }

    public void Fire(){
        throwSimulator.Shoot(this.transform,this.transform.position,destination.transform.position,10f,3f, ()=> {
            GameManager.Instance.SendMessage("IsPlayerScopeHit", destination);
            Destroy(gameObject);
            });
    }
    #endregion
}
