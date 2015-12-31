using UnityEngine;
using System.Collections;

public class TravelSign : MonoBehaviour, SpecialInteraction {

    public GameObject travelCanvas;

    public void RemoveSelf(){
        Destroy(gameObject);
    }

    public void OnEnter(){}
    public void OnHold(){}
    public void OnDrag(){}
    public void OnExit(){
        DebugWindow.LogSystem(GetType().Name,System.Reflection.MethodBase.GetCurrentMethod().Name);
        travelCanvas.SetActive(!travelCanvas.activeSelf);
    }


    public void Mining(){
        DebugWindow.LogSystem(GetType().Name,System.Reflection.MethodBase.GetCurrentMethod().Name);
    }
    public void Gathering(){
        DebugWindow.LogSystem(GetType().Name,System.Reflection.MethodBase.GetCurrentMethod().Name);
    }
    public void Combat(){
        DebugWindow.LogSystem(GetType().Name,System.Reflection.MethodBase.GetCurrentMethod().Name);
    }
}
