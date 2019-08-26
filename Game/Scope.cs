using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scope : MonoBehaviour
{
    #region Private Field
    private Tile destination;
    private ThrowSimulator throwSimulator;
    private List<Tile> scope = new List<Tile>();
    private int demageTime = 0;

    private GameObject effect;

    #endregion

    #region Public Field

    public void ScopeInit(Tile destination, List<Tile> scope, GameObject effect){
        this.destination = destination;
        this.effect = effect;
        this.scope = scope;
        this.throwSimulator = gameObject.AddComponent<ThrowSimulator>();
        foreach(var i in scope){
            i.TileTargeting(true);
        }
    }

    public void Fire(){
        throwSimulator.Shoot(this.transform,this.transform.position,destination.transform.position,10f,5f, ()=> {
            var effectObject = Instantiate(effect,destination.transform.position, Quaternion.identity);
            effectObject.SetActive(true);
            GameManager.Instance.SendMessage("IsPlayerScopeHit", destination);
                StartCoroutine("OneSecTimer");
            });
    }

    IEnumerator OneSecTimer(){
        yield return new WaitForSeconds(1);
        demageTime++;
        foreach(var i in scope){
            if(i.tileData.tileState == TileState.building){
                i.TileHit(1);
            }
            i.TileTargeting(true);
        }
        GameManager.Instance.SendMessage("IsPlayerScopeHit", destination);
        
        if(demageTime > 2){
            foreach(var i in scope){
                i.TileTargeting(false);
            }
            Destroy(gameObject);
            yield return null;
        }
        StartCoroutine("OneSecTimer");
    }
    #endregion
}
