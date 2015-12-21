using UnityEngine;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;

public class Equip : Item {
    
    public EquipType equipType;
    public float baseDmg;
    public float baseDef;
    public string assetPath;
    public float insulation;

    public override Equip AsEquip{
        get{
            return this;
        }
    }

    public Equip(){
        name = "";
        id = "";
        itemType = ItemType.equip;
        description = "";
        icon = null;

        equipType = EquipType.hat;
        baseDmg = 0f;
        baseDef = 0f;
        assetPath = "";
        insulation = 0f;
    }
    public Equip(Equip e){
        FieldInfo[] fields = GetType().GetFields();
        FieldInfo[] itemFields = e.GetType().GetFields();
        for (int i = 0; i < fields.Length; i++){
            fields[i].SetValue(this,itemFields[i].GetValue(e));
        }
    }
    public Equip(List<object> args){
        FieldInfo[] fields = GetType().GetFields();
        int x = args.Count - 5;
        for (int i = 0; i < args.Count; i++){
            if ( i < fields.Length )
                fields[i].SetValue(this, args[x]);
            x++;
            if ( x >= args.Count )
                x = 0;
        }
    }

}

public enum EquipType {
    hat = 0,
    costume = 1,
    weapon = 2
}
