using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyIsland_InGame
{
    public class AdamLazer: WoodStraight
    {
        [SerializeField]
        private GameObject lazer;
        [SerializeField]
        private BoxCollider lazerCollider;

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == this.tag)
            {
                EventManager.Instance.emit(EVENT_TYPE.TILE_HIT, this, TargetTile);
            }
        }
        public override void Fire(){
            lazer.gameObject.SetActive(true);
            lazerCollider.enabled = true;
            StartCoroutine(LazerOff());
        }
        IEnumerator LazerOff()
        {   
            yield return new WaitForSeconds(0.5f);
            lazer.gameObject.SetActive(false);
            lazerCollider.enabled = false;
        }
    }
}

