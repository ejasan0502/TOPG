using UnityEngine;
using System.Collections;

public class SelectPet : MonoBehaviour, SpecialInteraction {

    public Screen_PetSelect petSelect;

    public void RemoveSelf(){
        DebugWindow.LogSystem(GetType().Name,System.Reflection.MethodBase.GetCurrentMethod().Name);
        Destroy(this);
    }

    public void OnEnter(){}
    public void OnHold(){}
    public void OnDrag(){}
    public void OnExit(){
        DebugWindow.LogSystem(GetType().Name,System.Reflection.MethodBase.GetCurrentMethod().Name);
        GetComponent<Animator>().SetBool("Wave",true);
        petSelect.SetPet(name);
    }
}
