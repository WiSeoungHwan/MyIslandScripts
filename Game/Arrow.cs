using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    #region Private Field
    private Tile destination;
    
    #endregion

    #region Collider

    void OnTriggerEnter(Collider collider){
        Debug.Log(collider.tag);
        Destroy(gameObject);
    }

    #endregion

    #region Pubic Methods

    public void TargetSetup(Tile destination){
        this.destination = destination;
    }

    public void Fire(){
        Shoot(()=>{
            GameManager.Instance.SendMessage("IsPlayerHit",destination);
            Destroy(gameObject);
        });
    }

    #endregion

    #region Private Methods

    private void Shoot(System.Action onComplete){
        while(true){
            transform.LookAt(destination.transform);
            transform.position = new Vector3(transform.position.x + 0.1f,transform.position.y,transform.position.z);
            if(this.transform.position.x >= destination.transform.position.x){
                break;
            }
        }

        onComplete();
    }

    #endregion

}
