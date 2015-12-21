using UnityEngine;
using UnityEngine.UI;
using System.Reflection;
using System.Collections;

public class DebugWindow : MonoBehaviour {

    public bool persistent = false;
    public Text debugText;
    public Scrollbar scrollBar;
    public Image image;

    private static Object lockObj = new Object();
    private static DebugWindow _instance;
    public static DebugWindow instance {
        get {
            lock (lockObj){
                if ( _instance == null ){
                    _instance = GameObject.FindObjectOfType<DebugWindow>();
                }
            }
            return _instance;
        }
    }

    private Vector2 originSize;
    private bool display;

    void Start(){
        if ( GameObject.FindObjectsOfType<DebugWindow>().Length > 1 )
            Destroy(gameObject);
        DontDestroyOnLoad(this);

        originSize = instance.debugText.rectTransform.sizeDelta;
        display = image.enabled;
    }

    void Update(){
        if ( Input.GetKeyUp(KeyCode.Menu) ){
            display = !display;
            foreach (Transform t in transform){
                t.gameObject.SetActive(display);
                image.enabled = display;
            }
        }
    }
    public static void LogSystem(string scriptName, string methodName){
        Log(scriptName + ".cs: " + methodName + "()");
    }
    public static void Log(string s){
        if ( instance != null && instance.debugText != null ){
            if ( instance.persistent ) instance.debugText.text += "\n" + s;
            else instance.debugText.text = s;

            float size = Screen.height*0.05f*instance.debugText.text.Split('\n').Length;
            if ( size < instance.originSize.y ) size = instance.originSize.y;

            instance.debugText.rectTransform.sizeDelta = new Vector2(instance.originSize.x,size);
            instance.scrollBar.value = 0f;
        }
        Debug.Log(s);
    }
    public static void Log(params bool[] b){
        string s = "";
        foreach (bool bo in b){
            s += " , " + bo.ToString();
        }
        Log(s);
    }
    public static bool Assert(bool condition, string message){
        if ( condition )
            Log(message);

        return condition;
    }
}
