using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FPS : MonoBehaviour {
    
    private Text text;
    private float startTime;

    void Start(){
        text = GetComponent<Text>();
    }
    void Update(){
        startTime += (Time.deltaTime - startTime) * 0.1f;
        text.text = string.Format("{0:0.0} ms ({1:0.} fps)", startTime*1000.0f, 1.0f/startTime);
    }

}
