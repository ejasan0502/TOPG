using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {
    
    public Player player;
    public ContentData contentData;

    public List<string> startItems;

    private static Object lockObj = new Object();
    private static GameManager _instance;
    public static GameManager instance {
        get {
            lock(lockObj){
                if ( _instance == null ){
                    GameObject o = GameObject.Find("GameManager");

                    if ( o == null ){
                        o = new GameObject("GameManager");
                        _instance = o.AddComponent<GameManager>();
                    } else {
                        _instance = o.GetComponent<GameManager>();
                    }
                }
            }
            return _instance;
        }
    }

    void Start(){
        DebugWindow.LogSystem(GetType().Name,System.Reflection.MethodBase.GetCurrentMethod().Name);
        if ( GameObject.FindObjectsOfType<GameManager>().Length > 1 )
            DestroyImmediate(gameObject);

        DebugWindow.LogSystem(GetType().Name,System.Reflection.MethodBase.GetCurrentMethod().Name);
        DontDestroyOnLoad(this);
        contentData = new ContentData();
        _instance = this;
        GiveStartItems();
    }

    private void GiveStartItems(){
        DebugWindow.LogSystem(GetType().Name,System.Reflection.MethodBase.GetCurrentMethod().Name);
        if ( startItems.Count > 0 ){
            foreach (string s in startItems){
                Item i = contentData.GetItem(s);
                if ( i != null )
                    player.inventory.Add(i,1);
            }
        }
    }

}
