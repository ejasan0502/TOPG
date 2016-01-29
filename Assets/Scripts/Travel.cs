using UnityEngine;
using System.Collections;

public class Travel : MonoBehaviour {
    
    public Pet pet;

    public void Mining(){
        DebugWindow.LogSystem(GetType().Name,System.Reflection.MethodBase.GetCurrentMethod().Name);
        if ( pet != null ) {
            TouchManager.instance.CloseAll();
            SceneManager.LoadScene("mining",pet);
        }
    }
    public void Gathering(){
        DebugWindow.LogSystem(GetType().Name,System.Reflection.MethodBase.GetCurrentMethod().Name);
        if ( pet != null ) {
            TouchManager.instance.CloseAll();
            SceneManager.LoadScene("gathering",pet);
        }
    }
    public void Combat(){
        
    }

}
