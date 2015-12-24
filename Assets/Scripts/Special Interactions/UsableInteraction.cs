using UnityEngine;
using System.Collections;

public class UsableInteraction : MonoBehaviour, SpecialInteraction {
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
            RaycastHit hit;
            if ( Physics.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition),Camera.main.transform.forward*1000f, out hit) ){
                Interactable i = hit.collider.GetComponent<Interactable>();
                if ( i != null && i.isPet ){
                    Pet p = i.GetComponent<Pet>();
                    inv.items[slotIndex].item.AsUsable.Use(p);
                    inv.Remove(slotIndex,1);
                    p.anim.SetBool("Eat",true);
                }
            }
        }
    }
}
