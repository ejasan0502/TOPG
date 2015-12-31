using UnityEngine;
using System.Collections;

public class Ladder : MonoBehaviour {
    
    void OnTriggerEnter(Collider other){
        DebugWindow.LogSystem(GetType().Name,System.Reflection.MethodBase.GetCurrentMethod().Name);
        Pet p = other.GetComponent<Pet>();
        if ( p != null ){
            p.onLadder = gameObject;
            if ( p.transform.position.y >= transform.position.y )
                PlayerControls.instance.fromTop = true; 
        }
    }
    void OnTriggerExit(Collider other){
        DebugWindow.LogSystem(GetType().Name,System.Reflection.MethodBase.GetCurrentMethod().Name);
        Pet p = other.GetComponent<Pet>();
        if ( p != null ){
            p.onLadder = null;
            p.transform.position = new Vector3(p.transform.position.x,p.transform.position.y,0f);
            p.SetCollisionIgnoreWithPlatforms(false);
            PlayerControls.instance.velocity *= 0.5f;
        }
    }

}
