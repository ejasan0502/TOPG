using UnityEngine;
using System.Collections;

public class PickupCrystal : MonoBehaviour, Pickup {
    
    private int crystals = 0;
    private bool init = false;

    public void Initialize(int x){
        crystals = x;
        init = true;
    }
    public void Initialize(InventoryItem i){}
    public void Interact(){
        if ( init ){
            GameManager.instance.player.crystals += crystals;
            Destroy(gameObject);
        }
    }
}
