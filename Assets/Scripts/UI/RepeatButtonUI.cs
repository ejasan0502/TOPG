using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class RepeatButtonUI : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler {
    
    public MonoBehaviour target;
    public string methodName;
    private bool pressed = false;

    public void OnPointerEnter(PointerEventData eventData){
        DebugWindow.LogSystem(GetType().Name,System.Reflection.MethodBase.GetCurrentMethod().Name);
        pressed = true;
    }   
    public void OnPointerDown(PointerEventData eventData){
        DebugWindow.LogSystem(GetType().Name,System.Reflection.MethodBase.GetCurrentMethod().Name);
        pressed = true;   
    }
    public void OnPointerUp(PointerEventData eventData){
        DebugWindow.LogSystem(GetType().Name,System.Reflection.MethodBase.GetCurrentMethod().Name);
        pressed = false;
    }
    public void OnPointerExit(PointerEventData eventData){
        DebugWindow.LogSystem(GetType().Name,System.Reflection.MethodBase.GetCurrentMethod().Name);
        pressed = false;
    }

    void Update(){
        if ( pressed ){
            if ( target != null ){
                target.SendMessage(methodName);
            }
        }   
    }

}
