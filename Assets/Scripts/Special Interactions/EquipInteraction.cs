using UnityEngine;
using System.Collections;

public class EquipInteraction : MonoBehaviour, SpecialInteraction {
    public int slotIndex;

    void Start(){
        DebugWindow.LogSystem(GetType().Name,System.Reflection.MethodBase.GetCurrentMethod().Name);
    }

    public void RemoveSelf(){
        DebugWindow.LogSystem(GetType().Name,System.Reflection.MethodBase.GetCurrentMethod().Name);
        Destroy(this);
    }

    public void OnEnter(){}
    public void OnHold(){}
    public void OnDrag(){}
    public void OnExit(){
        DebugWindow.LogSystem(GetType().Name,System.Reflection.MethodBase.GetCurrentMethod().Name);
        Inventory inv = GameManager.instance.player.inventory;
        if ( slotIndex < inv.items.Count && inv.items[slotIndex] != null && inv.items[slotIndex].item != null ){
            DebugWindow.Log(slotIndex < inv.items.Count,inv.items[slotIndex] != null,inv.items[slotIndex].item != null);
            RaycastHit hit;
            if ( Physics.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition),Camera.main.transform.forward*1000f, out hit) ){
                DebugWindow.Log(hit.collider.name + " found");
                Interactable i = hit.collider.GetComponent<Interactable>();
                if ( i != null && i.isPet && i.GetComponent<Pet>().selected ){
                    DebugWindow.Log(i != null,i.isPet,i.GetComponent<Pet>().selected);
                    Pet p = i.GetComponent<Pet>();
                    p.Equip(slotIndex);
                }
            }
        }
    }
}
