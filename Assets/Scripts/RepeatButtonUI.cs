using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class RepeatButtonUI : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler {
    
    public MonoBehaviour target;
    public string methodName;
    private bool pressed = false;

    public void OnPointerEnter(PointerEventData eventData){
        pressed = true;
    }   
    public void OnPointerDown(PointerEventData eventData){
        pressed = true;   
    }
    public void OnPointerUp(PointerEventData eventData){
        pressed = false;
    }
    public void OnPointerExit(PointerEventData eventData){
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
