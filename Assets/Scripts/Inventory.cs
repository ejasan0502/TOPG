using UnityEngine;
using System.Xml;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Inventory {
    public List<InventoryItem> items;

    public Inventory(){
        items = new List<InventoryItem>();
    }

    public void Add(Item _item, int _amt){
        DebugWindow.LogSystem(GetType().Name,System.Reflection.MethodBase.GetCurrentMethod().Name);
        InventoryItem inventoryItem = GetInventoryItem(_item);
        if ( inventoryItem != null && inventoryItem.item.stackable ){
            inventoryItem.amt += _amt;
            if ( inventoryItem.amt > 99 ){
                items.Add(new InventoryItem(_item,inventoryItem.amt-99));
                inventoryItem.amt = 99;
                if ( InventoryHud.instance != null ){
                    InventoryHud.instance.UpdateInventoryItem(items.Count-1);
                    InventoryHud.instance.UpdateInventoryItem(inventoryItem);
                }
            }
        } else {
            int x = GetEmptyInventoryIndex();
            if ( x != -1 ){
                items[x] = new InventoryItem(_item,_amt);
                if ( InventoryHud.instance != null ) InventoryHud.instance.UpdateInventoryItem(x);
            } else {
                items.Add(new InventoryItem(_item,_amt));
                if ( InventoryHud.instance != null ) InventoryHud.instance.UpdateInventoryItem(items.Count-1);
            }
        }
    }
    public void Remove(Item _item, int _amt){
        DebugWindow.LogSystem(GetType().Name,System.Reflection.MethodBase.GetCurrentMethod().Name);
        InventoryItem inventoryItem = GetInventoryItem(_item);
        if ( DebugWindow.Assert(inventoryItem == null,"InventoryItem not found") ) return;
        if ( inventoryItem.item.stackable ){
            inventoryItem.amt -= _amt;
            if ( inventoryItem.amt < 1 ){
                items[GetInventoryIndex(inventoryItem)] = null;
            }
        } else {
            items[GetInventoryIndex(inventoryItem)] = null;
        }
        InventoryHud.instance.UpdateInventoryItem(inventoryItem);
    }
    public void Remove(int index, int amt){
        DebugWindow.LogSystem(GetType().Name,System.Reflection.MethodBase.GetCurrentMethod().Name);
        if ( index < items.Count && items[index] != null ){
            items[index].amt -= amt;
            if ( items[index].amt < 1 ){
                items[index] = null;
            }
            InventoryHud.instance.UpdateInventoryItem(index);
        }
    }

    public XmlElement ToXmlElement(XmlDocument xmlDoc){
        XmlElement root = xmlDoc.CreateElement("Inventory");

        foreach (InventoryItem ii in items){
            XmlElement content = xmlDoc.CreateElement("InventoryItem");
            XmlElement itemId = xmlDoc.CreateElement("Id");
            itemId.InnerText = ii.item.id;
            content.AppendChild(itemId);

            XmlElement amt = xmlDoc.CreateElement("Amount");
            amt.InnerText = ii.amt+"";
            content.AppendChild(amt);

            root.AppendChild(content);
        }

        return root;
    }

    private InventoryItem GetInventoryItem(Item _item){
        DebugWindow.LogSystem(GetType().Name,System.Reflection.MethodBase.GetCurrentMethod().Name);
        foreach (InventoryItem ii in items){
            if ( ii.item.id == _item.id ){
                return ii;
            }
        }

        return null;
    }
    private int GetEmptyInventoryIndex(){
        for (int i = 0; i < items.Count; i++){
            if ( items[i] == null ){
                return i;
            }
        }
        return -1;
    }
    public int GetInventoryIndex(InventoryItem ii){
        for (int i = 0; i < items.Count; i++){
            if ( items[i] == ii )
                return i;
        }
        return -1;
    }
}

[System.Serializable]
public class InventoryItem {
    public Item item;
    public int amt;

    public InventoryItem(Item i, int a){
        item = i;
        amt = a;
    }
}
