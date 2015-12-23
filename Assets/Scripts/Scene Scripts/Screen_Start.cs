using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Screen_Start : MonoBehaviour {
    
    public float waitTime = 1f;
    public List<GameObject> enableList;
    public GameObject petSelect;

    IEnumerator Start(){
        foreach (GameObject o in enableList){
            o.SetActive(false);
        }
        transform.GetChild(1).gameObject.SetActive(false);
        yield return new WaitForSeconds(waitTime);
        transform.GetChild(1).gameObject.SetActive(true);
    }

    public void Next(){
        foreach (GameObject o in enableList){
            o.SetActive(true);
        }

        if ( SceneManager.instance.isNewPlayer ){
            petSelect.SetActive(true);
        } else {
            Destroy(petSelect.gameObject);
            petSelect = null;
        }   

        Destroy(gameObject);
    }
}
