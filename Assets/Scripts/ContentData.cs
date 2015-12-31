using UnityEngine;
using System.Xml;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;

public class ContentData {

    private string[] paths = new string[3] {
        "Data/materials",
        "Data/equips",
        "Data/usables"
    };

    private List<Item> materials;
    private List<Equip> equips;
    private List<Usable> usables;

    public ContentData(){
        materials = new List<Item>();
        equips = new List<Equip>();
        usables = new List<Usable>();

        LoadData();
    }

    public Item GetItem(string id){
        DebugWindow.LogSystem(GetType().Name,System.Reflection.MethodBase.GetCurrentMethod().Name);

        int index = int.Parse(id.Split('-')[1]);
        switch( id.ToLower()[0] ){
        case 'e':
        foreach (Equip e in equips){
            if ( e.id.ToLower() == id.ToLower() )
                return e;
        }
        break;
        case 'u':
        foreach (Usable u in usables){
            if ( u.id.ToLower() == id.ToLower() )
                return u;
        }
        break;
        case 'm':
        if ( index < materials.Count )
            return materials[index];
        break;
        }

        return null;
    }

    private void LoadData(){
        DebugWindow.LogSystem(GetType().Name,System.Reflection.MethodBase.GetCurrentMethod().Name);
        for (int i = 0; i < paths.Length; i++){
            TextAsset ta = Resources.Load(paths[i]) as TextAsset;
            if ( ta == null ) {
                DebugWindow.Log("Cannot find xml file");
                return;
            }

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(ta.text);

             XmlNode root = xmlDoc.FirstChild;
             foreach (XmlNode contents in root.ChildNodes){
                List<object> args = new List<object>();

                foreach (XmlNode content in contents.ChildNodes){
                    switch(content.Name.ToLower()){
                        case "name":
                        args.Add(content.InnerText);
                        break;
                        case "id":
                        args.Add(content.InnerText);
                        break;
                        case "description":
                        args.Add(content.InnerText);
                        break;
                        case "icon":
                        Sprite sprite = Resources.Load<Sprite>(content.InnerText);
                        if ( sprite == null ){
                            sprite = Resources.Load<Sprite>("Content/Icons/_Default");
                        }
                        args.Add(sprite);
                        break;
                        case "stats":
                        string[] vals = content.InnerText.Split(',');
                        Stats st = new Stats();
                        foreach (string s in vals){
                            string[] vals2 = s.Split('-');
                            st.GetType().GetField(vals2[0]).SetValue(st,float.Parse(vals2[1]));
                        }
                        args.Add(st);
                        break;
                        case "usabletype":
                        UsableType ut = UsableType.add;
                        if ( content.InnerText.ToLower() == "sub" )
                            ut = UsableType.sub;
                        args.Add(ut);
                        break;
                        case "equiptype":
                        EquipType et = EquipType.weapon;
                        if ( content.InnerText.ToLower() == "costume" )
                            et = EquipType.costume;
                        else if ( content.InnerText.ToLower() == "hat" )
                            et = EquipType.hat;
                        args.Add(et);
                        break;
                        case "basedmg":
                        args.Add(float.Parse(content.InnerText));
                        break;
                        case "basedef":
                        args.Add(float.Parse(content.InnerText));
                        break;
                        case "assetpath":
                        args.Add(content.InnerText);
                        break;
                        case "insulation":
                        args.Add(float.Parse(content.InnerText));
                        break;
                    }
                }

                if ( contents.Name.ToLower() == "equip" ){
                    args.Insert(2,ItemType.equip);
                    Equip e = new Equip(args);
                    equips.Add(e);
                } else if ( contents.Name.ToLower() == "material" ){
                    args.Insert(2,ItemType.material);
                    Item it = new Item(args);
                    materials.Add(it);
                } else if ( contents.Name.ToLower() == "usable" ){
                    args.Insert(2,ItemType.usable);
                    Usable u = new Usable(args);
                    usables.Add(u);
                }
             }
        }
    }

}

// Note
// Rarer loot is at bottom of list
