using UnityEngine;
using System.Collections;

public class PickupItem : MonoBehaviour, Pickup {
    
    private InventoryItem ii = null;
    private bool init = false;

    public void Initialize(int x){}
    public void Initialize(InventoryItem i){
        ii = i;

        GetComponent<Renderer>().material.mainTexture = ii.item.icon.texture;

        init = true;
    }
    public void Interact(){
        if ( init ){
            GameManager.instance.player.inventory.Add(ii.item,ii.amt);
            Destroy(gameObject);
        }
    }
}
