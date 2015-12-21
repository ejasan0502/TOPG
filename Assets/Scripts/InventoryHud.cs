using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class InventoryHud : MonoBehaviour {
    
    public GameObject inventorySlotRef;

    private List<InventorySlotHud> inventorySlots;

    private static Object lockObj = new Object();
    private static InventoryHud _instance;
    public static InventoryHud instance {
        get {
            lock(lockObj){
                if ( _instance == null ){
                    _instance = GameObject.FindObjectOfType<InventoryHud>();
                }
            }
            return _instance;
        }
    }

    void Start(){
        DebugWindow.LogSystem(GetType().Name,System.Reflection.MethodBase.GetCurrentMethod().Name);
        inventorySlots = new List<InventorySlotHud>();
        CreateInventorySlots();
        ClearInventorySlots();
        FillInventorySlots();
    }

    private void CreateInventorySlots(){
        DebugWindow.LogSystem(GetType().Name,System.Reflection.MethodBase.GetCurrentMethod().Name);
        RectTransform rt = transform as RectTransform;
        RectTransform slotRefRt = inventorySlotRef.transform as RectTransform;

        int columns = Mathf.FloorToInt(rt.rect.width/slotRefRt.rect.width);
        int rows = Mathf.FloorToInt(rt.rect.height/slotRefRt.rect.height);

        float spacingX = (rt.rect.width - (slotRefRt.rect.width*columns))/(columns+2);
        float spacingY = (rt.rect.height - (slotRefRt.rect.height*rows))/(rows+2);

        float x = transform.position.x + rt.rect.min.x + slotRefRt.rect.width/2.0f + spacingX;
        float y = transform.position.y + rt.rect.max.y - slotRefRt.rect.height/2.0f - spacingY;
        for (int i = 0; i < columns*rows; i++){
            GameObject o = Instantiate(inventorySlotRef);
            o.transform.SetParent(transform);
            o.transform.position = new Vector3(x,y,0);
            o.name = i+"";

            x += spacingX + slotRefRt.rect.width;
            if ( i != 0 && (i+1)%columns == 0 ){
                x = transform.position.x - rt.rect.width/2.0f + slotRefRt.rect.width/2.0f + spacingX;
                y -= spacingY + slotRefRt.rect.height;
            }
            inventorySlots.Add(o.GetComponent<InventorySlotHud>());
        }
    }
    private void FillInventorySlots(){
        DebugWindow.LogSystem(GetType().Name,System.Reflection.MethodBase.GetCurrentMethod().Name);
        Inventory inv = GameManager.instance.player.inventory;
        for (int i = 0; i < inv.items.Count; i++){
            inventorySlots[i].Set(inv.items[i]);
            if ( inv.items[i].item.isUsable ){
                UsableInteraction ui = inventorySlots[i].gameObject.AddComponent<UsableInteraction>();
                ui.slotIndex = i;
            } else if ( inv.items[i].item.isEquip ){
                EquipInteraction ei = inventorySlots[i].gameObject.AddComponent<EquipInteraction>();
                ei.slotIndex = i;
            }
        } 
    }
    private void ClearInventorySlots(){
        DebugWindow.LogSystem(GetType().Name,System.Reflection.MethodBase.GetCurrentMethod().Name);
        foreach (InventorySlotHud ish in inventorySlots){
            ish.Clear();

            foreach (SpecialInteraction si in ish.GetComponents<SpecialInteraction>()){
                si.RemoveSelf();
            }
        }
    }

    public void Refresh(){
        DebugWindow.LogSystem(GetType().Name,System.Reflection.MethodBase.GetCurrentMethod().Name);
        ClearInventorySlots();
        FillInventorySlots();
    }
}
