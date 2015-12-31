using UnityEngine;
using System.Collections;

public class Portal : MonoBehaviour {
    
    public string scene;

    void Start(){
        RaycastHit hit;
        if ( Physics.Raycast(transform.position,transform.TransformDirection(Vector3.down),out hit,1000f,1 << LayerMask.NameToLayer("Ground")) ){
            Vector3 point = hit.point;
            Bounds b = GetComponent<Renderer>().bounds;

            transform.position = new Vector3(point.x,point.y+b.size.y/2.0f,0f);
        }
    }

}
