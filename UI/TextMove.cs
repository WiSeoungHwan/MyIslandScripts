using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TextMove : MonoBehaviour
{
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private Text damageText;

    void Start(){
        animator = gameObject.GetComponent<Animator>();
        AnimatorClipInfo[] clipInfos = animator.GetCurrentAnimatorClipInfo(0);
        Destroy(gameObject,clipInfos[0].clip.length - 0.01f);
    }


    public void SetText(string text){
        damageText.text = text;
    }
}
