﻿using UnityEngine;
using System.Collections;

public class Herb : MonoBehaviour, Character {
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
        
    }
    public void OnAttackEnd(){

    }
}
