using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(Button))]
public class EnableButton : MonoBehaviour {

    public GameObject obj;

    private bool enable = true;

    void Start(){
        DebugWindow.LogSystem(GetType().Name,System.Reflection.MethodBase.GetCurrentMethod().Name);
        if ( obj != null ){
            GetComponent<Button>().onClick.AddListener(() => OnClick());
            enable = obj.activeSelf;
        }
    }

    public void OnClick(){
        DebugWindow.LogSystem(GetType().Name,System.Reflection.MethodBase.GetCurrentMethod().Name);
        foreach (EnableButton btn in GameObject.FindObjectsOfType<EnableButton>()){
            btn.obj.SetActive(false);
        }

        obj.SetActive(!enable);
        enable = obj.activeSelf;
    }

}
