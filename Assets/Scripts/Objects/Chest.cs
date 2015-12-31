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
            
            Player p = GameManager.instance.player;
            p.crystals += crystals;
            foreach (InventoryItem i in items){
                p.inventory.Add(i.item,i.amt);
            }

            opened = true;
        }
    }
    public void OnAttackEnd(){

    }
}
