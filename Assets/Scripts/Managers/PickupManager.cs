using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class PickupManager : MonoBehaviour {
    
    private int crystals = 0;
    private List<InventoryItem> items = new List<InventoryItem>();

    public void Add(Item i, int amt){
        InventoryItem ii = (InventoryItem) items.Where(inv => inv.item.id == i.id);
        if ( ii == null ){
            items.Add(new InventoryItem(i,amt));
        } else {
            ii.amt += amt;
        }
    }
    public void Add(int x){
        crystals += x;
    }
    public void End(){
        Inventory inv = GameManager.instance.player.inventory;
        foreach (InventoryItem ii in items){
            inv.Add(ii.item,ii.amt);
        }
        GameManager.instance.player.crystals += crystals;
    }
}
