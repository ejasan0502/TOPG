using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
public class TempGravity : MonoBehaviour {
    
    private CharacterController cc;
    private Vector3 velocity = Vector3.zero;

    void Start(){
        cc = GetComponent<CharacterController>();
    }

    void Update(){
        if ( cc != null ){
            velocity += Physics.gravity * Time.deltaTime;
            cc.Move(velocity*Time.deltaTime);
        } else {
            Destroy(this);
        }

        if ( Physics.Raycast(transform.position,transform.TransformDirection(Vector3.down),0.1f, 1 << LayerMask.NameToLayer("Ground"))){
            Destroy(this);
        }
    }

}
