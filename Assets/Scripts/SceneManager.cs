using UnityEngine;
using System.IO;
using System.Xml;
using System.Collections;
using System.Collections.Generic;

public class SceneManager : MonoBehaviour {
    
    public List<GameObject> ignoreList;

    private static Object lockObject = new Object();
    private static SceneManager _instance;
    public static SceneManager instance {
        get {
            lock (lockObject){
                if ( _instance == null ){
                    _instance = GameObject.FindObjectOfType<SceneManager>();
                }
            }
            return _instance;
        }
    }

    private Scene currentScene = null;
    private Scene playerScene = null;

    void Start(){
        DebugWindow.LogSystem(GetType().Name,System.Reflection.MethodBase.GetCurrentMethod().Name);
        LoadData();
    }
    private void LoadData(){
        DebugWindow.LogSystem(instance.GetType().Name,System.Reflection.MethodBase.GetCurrentMethod().Name);

        playerScene = new Scene();
        playerScene.name = "PlayerScene";

        XmlDocument xmlDoc = new XmlDocument();
        if ( File.Exists(Application.persistentDataPath+"/PlayerScene") ){
            xmlDoc.Load(Application.persistentDataPath+"/PlayerScene");
        } else {
            TextAsset textAsset = (TextAsset) Resources.Load<TextAsset>("Data/playerscene");
            xmlDoc.LoadXml(textAsset.text);
        }

        XmlNode root = xmlDoc.FirstChild;
        foreach (XmlNode objects in root.ChildNodes){
            SceneObject so = new SceneObject();
            foreach (XmlNode content in objects.ChildNodes){
                if ( content.Name == "name" ){
                    so.name = content.InnerText;
                } else if ( content.Name == "position" ){
                    string[] s = content.InnerText.Split(',');
                    so.position = new Vector3(float.Parse(s[0]),float.Parse(s[1]),float.Parse(s[2]));
                } else if ( content.Name == "rotation" ){
                    string[] s = content.InnerText.Split(',');
                    so.rotation = Quaternion.Euler(new Vector3(float.Parse(s[0]),float.Parse(s[1]),float.Parse(s[2])));
                } else if ( content.Name == "scale" ){
                    string[] s = content.InnerText.Split(',');
                    so.scale = new Vector3(float.Parse(s[0]),float.Parse(s[1]),float.Parse(s[2]));
                }
            }
            playerScene.sceneObjects.Add(so);
        }
    }
    private void Generate(string s){
        
    }

    public static void Save(){
        if ( instance.playerScene == null || instance.currentScene != instance.playerScene ) return;
        DebugWindow.LogSystem(instance.GetType().Name,System.Reflection.MethodBase.GetCurrentMethod().Name);

        XmlDocument xmlDoc = new XmlDocument();
        XmlElement root = xmlDoc.CreateElement("PlayerScene");
        foreach (GameObject o in GameObject.FindGameObjectsWithTag("SceneObject")){
            if ( instance.ignoreList.Contains(o) ) continue;

            XmlElement baseNode = xmlDoc.CreateElement("SceneObject");

            XmlElement nameXml = xmlDoc.CreateElement("name");
            if ( o.name.Contains("(Clone)") ){
                nameXml.InnerText = o.name.Split('(')[0];
            } else {
                nameXml.InnerText = o.name;
            }
            baseNode.AppendChild(nameXml);

            XmlElement posXml = xmlDoc.CreateElement("position");
            posXml.InnerText = o.transform.position.x + "," + o.transform.position.y + "," + o.transform.position.z;
            baseNode.AppendChild(posXml);

            XmlElement rotXml = xmlDoc.CreateElement("rotation");
            rotXml.InnerText = o.transform.rotation.x + "," + o.transform.position.y + "," + o.transform.position.z;
            baseNode.AppendChild(rotXml);

            XmlElement scaXml = xmlDoc.CreateElement("scale");
            scaXml.InnerText = o.transform.localScale.x + "," + o.transform.localScale.y + "," + o.transform.localScale.z;
            baseNode.AppendChild(scaXml);

            root.AppendChild(baseNode);
        }

        xmlDoc.Save(Application.persistentDataPath+"/PlayerScene");
    }
    public static void LoadScene(string s){
        if ( instance.currentScene == null ) return;
        DebugWindow.LogSystem(instance.GetType().Name,System.Reflection.MethodBase.GetCurrentMethod().Name);
        if ( instance.currentScene.gameObjects.Count > 0 ){
            foreach (GameObject o in instance.currentScene.gameObjects){
                Destroy(o);
            }
            instance.currentScene.gameObjects = new List<GameObject>();
        }

        if ( s.ToLower().Contains("tutorial") ){
            GameObject obj = (GameObject) Instantiate(Resources.Load<GameObject>("Scenes/"+s));
            instance.currentScene = new Scene();
            instance.currentScene.name = s;
            instance.currentScene.gameObjects.Add(obj);
        } else if ( s.ToLower().Contains("playerscene") ){
            foreach (SceneObject so in instance.playerScene.sceneObjects){
                GameObject o = (GameObject) Instantiate(Resources.Load<GameObject>("SceneObjects/"+so.name));
                o.transform.position = so.position;
                o.transform.rotation = so.rotation;
                o.transform.localScale = so.scale;
                instance.playerScene.gameObjects.Add(o);
            }
            instance.currentScene = instance.playerScene;
        } else {
            instance.Generate(s);
        }
    }
}

public class SceneObject {
    public string name;
    public Vector3 position;
    public Quaternion rotation;
    public Vector3 scale;

    public SceneObject(){}
}

public class Scene {
    public string name;
    public List<SceneObject> sceneObjects;
    public List<GameObject> gameObjects;

    public Scene(){
        name = "";
        sceneObjects = new List<SceneObject>();
        gameObjects = new List<GameObject>();
    }
}