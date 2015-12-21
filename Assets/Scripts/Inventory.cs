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
            }
        } else {
            items.Add(new InventoryItem(_item,_amt));
        }
    }
    public void Remove(Item _item, int _amt){
        DebugWindow.LogSystem(GetType().Name,System.Reflection.MethodBase.GetCurrentMethod().Name);
        InventoryItem inventoryItem = GetInventoryItem(_item);
        if ( inventoryItem != null && inventoryItem.item.stackable ){
            inventoryItem.amt -= _amt;
            if ( inventoryItem.amt < 1 ){
                items.Remove(inventoryItem);
            }
        } else {
            items.Remove(inventoryItem);
        }
    }
    public void Remove(int index, int amt){
        DebugWindow.LogSystem(GetType().Name,System.Reflection.MethodBase.GetCurrentMethod().Name);
        if ( index < items.Count && items[index] != null ){
            items[index].amt -= amt;
            if ( items[index].amt < 1 ){
                items.RemoveAt(index);
            }
            InventoryHud.instance.Refresh();
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
