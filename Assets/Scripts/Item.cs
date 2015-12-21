using UnityEngine;
using System.Xml;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;

public class Item {
    public string name;
    public string id;
    public ItemType itemType;
    public string description;
    public Sprite icon;

    public bool isEquip {
        get {
            return AsEquip != null;
        }
    }
    public bool isUsable {
        get {
            return AsUsable != null;
        }
    }
    public virtual Equip AsEquip {
        get {
            return null;
        }
    }
    public virtual Usable AsUsable {
        get {
            return null;
        }
    }
    public bool stackable {
        get {
            return AsEquip != null;
        }
    }

    public Item(){
        name = "";
        id = "";
        itemType = ItemType.material;
        description = "";
        icon = null;
    }
    public Item(Item item){
        FieldInfo[] fields = GetType().GetFields();
        FieldInfo[] itemFields = item.GetType().GetFields();
        for (int i = 0; i < fields.Length; i++){
            fields[i].SetValue(this,itemFields[i].GetValue(item));
        }
    }
    public Item(List<object> args){
        FieldInfo[] fields = GetType().GetFields();
        for (int i = 0; i < args.Count; i++){
            if ( i < fields.Length ){
                fields[i].SetValue(this,args[i]);
            }
        }
    }
}

public enum ItemType {
    equip,
    usable,
    material
}
