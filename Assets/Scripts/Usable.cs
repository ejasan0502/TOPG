using UnityEngine;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;

public class Usable : Item {
    public Stats stats;
    public UsableType usableType;

    public override Usable AsUsable{
        get{
            return this;
        }
    }

    public Usable(){
        name = "";
        id = "";
        itemType = ItemType.usable;
        description = "";
        icon = null;

        stats = new Stats();
        usableType = UsableType.add;
    }
    public Usable(Usable u){
        FieldInfo[] fields = GetType().GetFields();
        FieldInfo[] itemFields = u.GetType().GetFields();
        for (int i = 0; i < fields.Length; i++){
            fields[i].SetValue(this,itemFields[i].GetValue(u));
        }
    }
    public Usable(List<object> args){
        FieldInfo[] fields = GetType().GetFields();
        int x = args.Count - 2;
        for (int i = 0; i < args.Count; i++){
            if ( i < fields.Length )
                fields[i].SetValue(this, args[x]);
            x++;
            if ( x >= args.Count )
                x = 0;
        }
    }

    public void Use(Pet pet){
        DebugWindow.LogSystem(GetType().Name,System.Reflection.MethodBase.GetCurrentMethod().Name);
        switch(usableType){
        case UsableType.add:
        pet.AddToCurrentStats(stats);
        break;
        case UsableType.sub:
        pet.SubtractFromCurrentStats(stats);
        break;
        }
    }
}

public enum UsableType {
    add,
    sub
}
