using UnityEngine;
using System.Collections;

public class Travel : MonoBehaviour, SpecialInteraction {

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
        CameraControls.instance.Move(transform.position);
    }


    public void Mining(){
        DebugWindow.LogSystem(GetType().Name,System.Reflection.MethodBase.GetCurrentMethod().Name);
        GameObject o = new GameObject("PickupManager");
        o.AddComponent<PickupManager>();
        SceneManager.LoadScene("mining");
    }
    public void Gathering(){
        DebugWindow.LogSystem(GetType().Name,System.Reflection.MethodBase.GetCurrentMethod().Name);
        GameObject o = new GameObject("PickupManager");
        o.AddComponent<PickupManager>();
        SceneManager.LoadScene("gathering");
    }
    public void Combat(){
        DebugWindow.LogSystem(GetType().Name,System.Reflection.MethodBase.GetCurrentMethod().Name);
        GameObject o = new GameObject("PickupManager");
        o.AddComponent<PickupManager>();
    }
}
