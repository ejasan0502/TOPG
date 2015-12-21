using UnityEngine;
using System.Collections;

public class Brush : MonoBehaviour, SpecialInteraction {

    public Stats additive;
    public float frequency = 3f;

    private Vector3 originPos;
    private Vector3 prevPos;
    private float startTime;

    void Start(){
        DebugWindow.LogSystem(GetType().Name,System.Reflection.MethodBase.GetCurrentMethod().Name);
        startTime = Time.time;
        originPos = transform.position;
    }

    public void OnEnter(){}
    public void OnHold(){}
    public void OnDrag(){
        DebugWindow.LogSystem(GetType().Name,System.Reflection.MethodBase.GetCurrentMethod().Name);
        RaycastHit hit;
        if ( Physics.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition),Camera.main.transform.forward*1000f, out hit) ){
            Interactable i = hit.collider.GetComponent<Interactable>();
            if ( i != null && i.isPet && Input.mousePosition != prevPos && Time.time - startTime >= frequency ){
                Pet p = i.GetComponent<Pet>();
                p.AddToCurrentStats(additive);
                prevPos = Input.mousePosition;
                startTime = Time.time;
            }
        }
    }
    public void OnExit(){
        DebugWindow.LogSystem(GetType().Name,System.Reflection.MethodBase.GetCurrentMethod().Name);
        transform.position = originPos;
    }

    public void RemoveSelf(){
        DebugWindow.LogSystem(GetType().Name,System.Reflection.MethodBase.GetCurrentMethod().Name);
        Destroy(this);
    }
}
