using UnityEngine;
using System.Collections;

public class Ore : MonoBehaviour, Character {
    
    [HideInInspector] public InventoryItem item = null;

    public GameObject obj {
        get {
            return gameObject;
        }
    }
    public float damage {
        get {
            return 0f;
        }
    }
    public bool isAlive {
        get {
            return true;
        }
    }

    public void Hit(float dmg, Character c){
        DebugWindow.LogSystem(GetType().Name,System.Reflection.MethodBase.GetCurrentMethod().Name);

        if ( item != null ){
            GameObject o = (GameObject) Instantiate(Resources.Load("SceneObjects/PickupItem"));
            o.GetComponent<Pickup>().Initialize(item);
            o.transform.position = transform.position;

            RaycastHit hit;
            if ( Physics.Raycast(transform.position,transform.TransformDirection(Vector3.up),out hit,1000f,1 << LayerMask.NameToLayer("Ground")) ){
                Vector3 pos = hit.point;
                pos.y += 0.5f;
                o.transform.position = pos;
            }
            Destroy(gameObject);
        }
    }
    public void OnAttackEnd(){

    }
}
