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

    public void Hit(float dmg, Character c){
        Pet p = c.obj.GetComponent<Pet>();
        if ( !opened && p != null ){ 
            GetComponent<Animation>().Play();
            opened = true;
        }
    }
    public void OnAttackEnd(){

    }
}
