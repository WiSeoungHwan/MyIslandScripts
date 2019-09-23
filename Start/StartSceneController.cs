using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartSceneController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        LoadingSceneManager.LoadScene("MainScene");  
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
