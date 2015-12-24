using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class InventorySlotHud : MonoBehaviour {
    public Image icon;
    public Text amt;

    public void Set(InventoryItem ii){
        DebugWindow.LogSystem(GetType().Name,System.Reflection.MethodBase.GetCurrentMethod().Name);
        if ( ii == null ){
            icon.sprite = null;
            amt.text = "";
        } else {
            icon.sprite = ii.item.icon;
            if ( ii.amt == 1 )
                amt.text = "";
            else
                amt.text = ii.amt+"";
        }
    }

    public void Clear(){
        DebugWindow.LogSystem(GetType().Name,System.Reflection.MethodBase.GetCurrentMethod().Name);
        icon.sprite = null;
        amt.text = "";
    }
}
