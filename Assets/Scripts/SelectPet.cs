using UnityEngine;
using System.Collections;

public class SelectPet : MonoBehaviour {

    public Screen_PetSelect petSelect;

    void OnMouseDown(){
        DebugWindow.LogSystem(GetType().Name,System.Reflection.MethodBase.GetCurrentMethod().Name);
        GetComponent<Animator>().SetBool("Wave",true);
        petSelect.SetPet(name);
    }
}
