using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace MyIsland_InGame
{
    public class BGController : MonoBehaviour
    {
        #region Serialize Field
        [SerializeField]
        GameObject[] plenetPieces;
        #endregion

        #region MonoBehaviour
        void Start()
        {
            foreach(var i in plenetPieces){
                ObjectRandomMove(i);
            }
        }
        #endregion

        #region Private Field
        private void ObjectRandomMove(GameObject piece){
            piece.transform.DOMoveX(piece.transform.position.x + 20f,15f).SetLoops(-1);
            piece.transform.DOMoveY(piece.transform.position.y + 0.5f,2f).SetLoops(-1,LoopType.Yoyo);
            piece.transform.DORotate(new Vector3(90f,-20f,200f),10f).SetLoops(-1, LoopType.Yoyo);
            
        }
        #endregion
    }
}
