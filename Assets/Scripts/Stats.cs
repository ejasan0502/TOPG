using UnityEngine;
using System.Collections;
using System.Reflection;

[System.Serializable]
public class Stats {
    public float health;
    public float baseDmg;
    public float baseDef;

    public float happy;
    public float hungry;
    public float sleepy;
    public float thirsty;
    public float temperature;
    public float hygiene;

    public Stats(){
        FieldInfo[] fields = GetType().GetFields();
        for (int i = 0; i < fields.Length; i++){
            fields[i].SetValue(this,0f);
        }
    }
    public Stats(float v){
        FieldInfo[] fields = GetType().GetFields();
        for (int i = 0; i < fields.Length; i++){
            fields[i].SetValue(this,v);
        }
    }
    public Stats(params float[] v){
        FieldInfo[] fields = GetType().GetFields();
        for (int i = 0; i < v.Length; i++){
            if ( i < fields.Length ){
                fields[i].SetValue(this,v[i]);
            }
        }
    }
    public Stats(Stats s){
        FieldInfo[] fields = GetType().GetFields();
        FieldInfo[] fields2 = s.GetType().GetFields();
        for (int i = 0; i < fields.Length; i++){
            fields[i].SetValue(this,fields2[i].GetValue(s));
        }
    }

    public void Set(string fieldName, float val){
        foreach (FieldInfo fields in GetType().GetFields()){
            if ( fields.Name.ToLower() == fieldName.ToLower() ){
                fields.SetValue(this,val);
            }
        }
    }

    #region Operators
    public static Stats operator+(Stats s1, Stats s2){
        Stats s = new Stats();

        FieldInfo[] f = s.GetType().GetFields();
        FieldInfo[] f1 = s1.GetType().GetFields();
        FieldInfo[] f2 = s2.GetType().GetFields();

        for (int i = 0; i < f.Length; i++){
            f[i].SetValue(s,(float)f1[i].GetValue(s1)+(float)f2[i].GetValue(s2));
        }

        return s;
    }
    public static Stats operator-(Stats s1, Stats s2){
        Stats s = new Stats();

        FieldInfo[] f = s.GetType().GetFields();
        FieldInfo[] f1 = s1.GetType().GetFields();
        FieldInfo[] f2 = s2.GetType().GetFields();

        for (int i = 0; i < f.Length; i++){
            f[i].SetValue(s,(float)f1[i].GetValue(s1)-(float)f2[i].GetValue(s2));
        }

        return s;
    }
    public static Stats operator*(Stats s1, Stats s2){
        Stats s = new Stats();

        FieldInfo[] f = s.GetType().GetFields();
        FieldInfo[] f1 = s1.GetType().GetFields();
        FieldInfo[] f2 = s2.GetType().GetFields();

        for (int i = 0; i < f.Length; i++){
            f[i].SetValue(s,(float)f1[i].GetValue(s1)*(float)f2[i].GetValue(s2));
        }

        return s;
    }
    public static Stats operator/(Stats s1, Stats s2){
        Stats s = new Stats();

        FieldInfo[] f = s.GetType().GetFields();
        FieldInfo[] f1 = s1.GetType().GetFields();
        FieldInfo[] f2 = s2.GetType().GetFields();

        for (int i = 0; i < f.Length; i++){
            f[i].SetValue(s,(float)f1[i].GetValue(s1)/(float)f2[i].GetValue(s2));
        }

        return s;
    }
    public static Stats operator*(Stats s1, float val){
        Stats s = new Stats();

        FieldInfo[] f = s.GetType().GetFields();
        FieldInfo[] f1 = s1.GetType().GetFields();

        for (int i = 0; i < f.Length; i++){
            f[i].SetValue(s,(float)f1[i].GetValue(s1)*val);
        }

        return s;
    }
    #endregion
}
