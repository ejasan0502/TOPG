using UnityEngine;
using System.Collections;

public class Ore : MonoBehaviour, Character {
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
        Destroy(gameObject);
    }
    public void OnAttackEnd(){

    }
}
