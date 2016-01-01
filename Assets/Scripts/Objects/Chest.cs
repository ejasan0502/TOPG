using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Chest : MonoBehaviour, Character {
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

    private bool opened = false;
    private List<InventoryItem> items = new List<InventoryItem>();
    private int crystals = 0;

    public void Initialize(List<InventoryItem> i, int c){
        items = i;
        crystals = c;
    }

    public void Hit(float dmg, Character c){
        DebugWindow.LogSystem(GetType().Name,System.Reflection.MethodBase.GetCurrentMethod().Name);
        Pet pet = c.obj.GetComponent<Pet>();
        if ( !opened && pet != null ){ 
            GetComponent<Animation>().Play();
            
            GameObject o;
            Vector3 pos;
            foreach (InventoryItem ii in items){
                o = (GameObject) Instantiate(Resources.Load("SceneObjects/PickupItem"));
                o.GetComponent<Pickup>().Initialize(ii);
                pos = transform.position;
                pos.y += 0.5f;
                o.transform.position = pos;
            }
            o = (GameObject) Instantiate(Resources.Load("SceneObjects/PickupCrystal"));
            o.GetComponent<Pickup>().Initialize(crystals);
            pos = transform.position;
            pos.y += 0.5f;
            o.transform.position = pos;

            opened = true;
        }
    }
    public void OnAttackEnd(){

    }
}
