using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DungeonGenerator : MonoBehaviour {

    public GameObject invisBlock;
    public GameObject grass;
    public GameObject dirt;
    public GameObject ladder;
    public GameObject exit;

    public GameObject ore;
    public GameObject herb;
    public GameObject chest;

    public GameObject testPet;
    public string testMap = "gathering";

    public float chestChance = 1f;
    public float oreChance = 25f;
    public float herbChance = 25f;

    public float petScaleSize = 4f;
    public float petJumpHeight = 2f;
    public float petJumpDistance = 3f;

    public int difficulty = 1;
    public int roomsPerDiff = 3;
    public float roomSize = 20f;
    public float ladderZ = 0.3f;
    public float platformZ = 0.2f;

    private float startTime;
    private string mapType = "";

    void Start(){
        if ( testMap != "" )
            StartCoroutine(GenerateMap(testMap));
    }   

    private bool OverlapWith(List<Rect> rectsToCheck, Rect rect){
        foreach (Rect r in rectsToCheck){
            float width = r.width;
            float height = r.height*5f;

            Rect check = new Rect(r.center.x-width/2.0f,r.center.y-height/2.0f,width,height);
            if ( rect.Overlaps(check) ){
                return true;
            }
        }
        return false;
    }
    private IEnumerator GenerateMap(string s){
        DebugWindow.LogSystem(GetType().Name,System.Reflection.MethodBase.GetCurrentMethod().Name);

        startTime = Time.time;
        mapType = s;
        DebugWindow.Assert(mapType == "gathering" || mapType == "mining","Do not recognize map type.");

        List<RoomNode> roomPath = new List<RoomNode>();         // List of rooms that path will follow
        List<RoomNode> rooms = new List<RoomNode>();            // List of all available rooms
        List<Rect> objects = new List<Rect>();

        #region Create Layout
        // Create grid of rooms
        Vector3 pos = Vector3.zero;
        pos.x = roomSize/2.0f;
        for (int i = 0; i < Mathf.Pow(difficulty*roomsPerDiff,2); i++){
            if ( i != 0 && i%(difficulty*roomsPerDiff) == 0 ){
                pos.x = roomSize/2.0f;
                pos.y += roomSize;
            } else if ( i != 0 ){
                pos.x += roomSize;
            }

            RoomNode rn = new RoomNode();
            rn.position = pos;
            rn.rect = new Rect(pos.x-roomSize/2.0f,pos.y-roomSize/2.0f,roomSize,roomSize);

            rooms.Add(rn);
        }

        // Set neighbor rooms for each room
        for (int i = 0; i < rooms.Count; i++){
            // Check Top
            if ( i-difficulty*roomsPerDiff >= 0 )
                rooms[i].neighbors.Add(rooms[i-difficulty*roomsPerDiff]);
            // Check Right
            if ( i+1 < rooms.Count && (i+1)%(difficulty*roomsPerDiff) != 0 )
                rooms[i].neighbors.Add(rooms[i+1]);
            // Check Bottom
            if ( i+difficulty*roomsPerDiff < rooms.Count )
                rooms[i].neighbors.Add(rooms[i+difficulty*roomsPerDiff]);
            // Check Left
            if ( i-1 >= 0 && (i)%(difficulty*roomsPerDiff) != 0 )
                rooms[i].neighbors.Add(rooms[i-1]);
        }   

        // Fill roomPath
        roomPath.Add(rooms[Random.Range(0,rooms.Count)]);
        while ( roomPath.Count < difficulty*roomsPerDiff ){
            int x = Random.Range(0,roomPath[roomPath.Count-1].neighbors.Count);
            if ( !roomPath.Contains(roomPath[roomPath.Count-1].neighbors[x]) ){
                roomPath.Add(roomPath[roomPath.Count-1].neighbors[x]);
            }
        }

        // Create borders
        for (int i = 0; i < roomPath.Count; i++){
            RoomNode prevRoom = null;
            RoomNode nextRoom = null;
            RoomNode currentRoom = roomPath[i];

            if ( i-1 >= 0 ) prevRoom = roomPath[i-1];
            if ( i+1 < roomPath.Count ) nextRoom = roomPath[i+1];

            // Up
            if ( prevRoom == null || prevRoom != null && currentRoom.position.y >= prevRoom.position.y ){
                if ( nextRoom == null || nextRoom != null && currentRoom.position.y >= nextRoom.position.y ){
                    GameObject t = Instantiate(grass);
                    t.transform.localScale = new Vector3(roomSize+1f,1f,1f);
                    t.transform.position = new Vector3(currentRoom.position.x,currentRoom.position.y+roomSize/2.0f,0f);
                    t.name = i+"";

                    BoxCollider bc = t.GetComponent<BoxCollider>();
                    Rect r = new Rect(bc.bounds.min.x,bc.bounds.min.y,bc.bounds.size.x,bc.bounds.size.y);
                    objects.Add(r);
                    t.transform.SetParent(transform);
                }
            }

            // Right
            if ( prevRoom == null || prevRoom != null && currentRoom.position.x >= prevRoom.position.x ){
                if ( nextRoom == null || nextRoom != null && currentRoom.position.x >= nextRoom.position.x ){
                    GameObject r = Instantiate(dirt);
                    r.transform.localScale = new Vector3(1f,roomSize,1f);
                    r.transform.position = new Vector3(currentRoom.position.x+roomSize/2.0f,currentRoom.position.y,0f);
                    r.name = i+"";

                    BoxCollider bc = r.GetComponent<BoxCollider>();
                    Rect rect = new Rect(bc.bounds.min.x,bc.bounds.min.y,bc.bounds.size.x,bc.bounds.size.y);
                    objects.Add(rect);
                    r.transform.SetParent(transform);
                }
            }

            // Down
            if ( prevRoom == null || prevRoom != null && currentRoom.position.y <= prevRoom.position.y ){
                if ( nextRoom == null || nextRoom != null && currentRoom.position.y <= nextRoom.position.y ){
                    GameObject d = Instantiate(grass);
                    d.transform.localScale = new Vector3(roomSize+1f,1f,1f);
                    d.transform.position = new Vector3(currentRoom.position.x,currentRoom.position.y-roomSize/2.0f,0f);
                    d.name = i+"";

                    BoxCollider bc = d.GetComponent<BoxCollider>();
                    Rect r = new Rect(bc.bounds.min.x,bc.bounds.min.y,bc.bounds.size.x,bc.bounds.size.y);
                    objects.Add(r);
                    d.transform.SetParent(transform);
                }
            }

            // Left
            if ( prevRoom == null || prevRoom != null && currentRoom.position.x <= prevRoom.position.x ){
                if ( nextRoom == null || nextRoom != null && currentRoom.position.x <= nextRoom.position.x ){
                    GameObject l = Instantiate(dirt);
                    l.transform.localScale = new Vector3(1f,roomSize,1f);
                    l.transform.position = new Vector3(currentRoom.position.x-roomSize/2.0f,currentRoom.position.y,0f);
                    l.name = i+"";

                    BoxCollider bc = l.GetComponent<BoxCollider>();
                    Rect r = new Rect(bc.bounds.min.x,bc.bounds.min.y,bc.bounds.size.x,bc.bounds.size.y);
                    objects.Add(r);
                    l.transform.SetParent(transform);
                }
            }
        }
        
        DebugWindow.Log("Generated map layout. Took " + (Time.time-startTime) + " secs");
        startTime = Time.time;
        #endregion
        #region Create Solution Path
        Vector3 prevPoint = Vector3.zero;
        for (int i = 0; i < roomPath.Count; i++){
            // Select a random point
            Vector3 point = Vector3.zero;
            point.x = Random.Range(roomPath[i].rect.min.x+petScaleSize,roomPath[i].rect.max.x-petScaleSize);
            point.y = Random.Range(roomPath[i].rect.min.y+petScaleSize,roomPath[i].rect.max.y-petScaleSize);

            if ( i != 0 ){
                float distX = point.x - prevPoint.x;
                float distY = point.y - prevPoint.y;
                
                if ( Mathf.Abs(distX) > Mathf.Abs(distY) ){
                    // Create platform
                    pos = Vector3.zero;
                    pos.x = prevPoint.x + distX/2.0f;
                    pos.y = prevPoint.y;
                    CreatePlatform(pos,new Vector3(Mathf.Abs(distX)+1f,1f,platformZ),objects);

                    if ( Mathf.Abs(distY) >= petScaleSize || Mathf.Abs(distY) >= petJumpHeight ){
                        pos = Vector3.zero;
                        pos.x = prevPoint.x + distX;
                        pos.y = prevPoint.y + distY/2.0f + 0.5f;
                        CreateLadder(pos,new Vector3(1f,Mathf.Abs(distY)+0.5f,ladderZ),objects);

                        if ( i == roomPath.Count-1 ){
                            pos = Vector3.zero;
                            pos.x = point.x;
                            pos.y = point.y;
                            CreatePlatform(pos,new Vector3(Random.Range(5f,roomSize-petScaleSize)+1f,1f,platformZ),objects);
                        }
                    }
                } else {
                    if ( i-1 == 0 ){
                        pos = Vector3.zero;
                        pos.x = prevPoint.x;
                        pos.y = prevPoint.y;
                        CreatePlatform(pos,new Vector3(Random.Range(5f,roomSize-petScaleSize)+1f,1f,platformZ),objects);
                    }

                    // Create ladder
                    pos = Vector3.zero;
                    pos.x = prevPoint.x;
                    pos.y = prevPoint.y + distY/2.0f + 0.5f;
                    CreateLadder(pos,new Vector3(1f,Mathf.Abs(distY)+0.5f,ladderZ),objects);

                    pos = Vector3.zero;
                    pos.x = prevPoint.x + distX/2.0f;
                    pos.y = prevPoint.y + distY;
                    CreatePlatform(pos,new Vector3(Mathf.Abs(distX)+1f,1f,platformZ),objects);
                }
            } else {
                GameObject spawnPoint = new GameObject("Spawn Point");
                spawnPoint.transform.position = point;
                spawnPoint.transform.SetParent(transform);
            }

            prevPoint = point;
        }

        DebugWindow.Log("Solution path created. Took " + (Time.time-startTime) + " secs");
        startTime = Time.time;
        #endregion
        #region Fill Map with platforms/ladders/ores/herbs/chests
        // Add platforms
        yield return new WaitForSeconds(1f);
        foreach (RoomNode rn in roomPath){
            for (float x = rn.rect.xMin; x < rn.rect.xMax; x++){
                for (float y = rn.rect.yMin; y < rn.rect.yMax; y++){
                    if ( Random.Range(0,100) < 10f ){
                        Rect r = new Rect(x,y,Random.Range(5f,rn.rect.width/2.0f-5f),1f);
                        if ( !OverlapWith(objects,r) ){
                            CreatePlatform(new Vector3(r.x+r.width/2.0f,r.y+r.height/2.0f,0f),new Vector3(r.width,r.height,platformZ),objects);
                        }
                    }
                }
            }
        }

        // Add exit
        GameObject portalObj = Instantiate(exit);
        portalObj.transform.position = prevPoint;
        portalObj.transform.SetParent(transform);

        DebugWindow.Log("Filled map. Took " + (Time.time-startTime) + " secs");
        #endregion

        if ( testPet != null ){
            testPet.transform.position = GameObject.Find("Spawn Point").transform.position;
        }
    }
    private Bounds CreatePlatform(Vector3 position, Vector3 scale, List<Rect> rectList){
        GameObject o = Instantiate(grass);
        o.transform.localScale = scale;
        o.transform.position = position;
        o.transform.SetParent(transform);
        o.AddComponent<TiledMesh>();

        if ( rectList != null ){
            BoxCollider bc = o.GetComponent<BoxCollider>();
            Rect re = new Rect(bc.bounds.min.x,bc.bounds.min.y,bc.bounds.size.x,bc.bounds.size.y);
            rectList.Add(re);
        }

        // Create chest/ore/herb
        Vector3 pos = o.transform.position;
        Bounds b = o.GetComponent<Renderer>().bounds;
        GameObject obj = null;

        for (int i = 0; i < 5/difficulty; i++){
            pos.x = Random.Range(b.min.x,b.max.x);
            pos.y = b.max.y;
            if ( Random.Range(0,100) <= chestChance*difficulty ){
                obj = Instantiate(chest);
            } else if ( mapType == "gathering" && Random.Range(0,100) <= herbChance ){
                obj = Instantiate(herb);
                pos.y = b.max.y + obj.GetComponent<Renderer>().bounds.size.y/2.0f;
            } else if ( mapType == "mining" && Random.Range(0,100) <= oreChance ){
                obj = Instantiate(ore);
                pos.y = b.max.y + obj.GetComponent<Renderer>().bounds.size.y/2.0f;
            }

            if ( obj != null ){
                obj.transform.position = pos;
                obj = null;
            }
        }

        // Create Ladder
        pos = Vector3.zero;
        pos.x = Random.Range(b.min.x,b.max.x);
        pos.y = b.min.y;
        RaycastHit hit;
        if ( Physics.Raycast(pos,o.transform.TransformDirection(Vector3.down),out hit,1000f,1 << LayerMask.NameToLayer("Ground")) ){
            DebugWindow.Log(b.center.y+" "+hit.point.y);
            pos.y = b.center.y - Mathf.Abs(b.center.y-hit.point.y)/2.0f + 0.5f;
            CreateLadder(pos,new Vector3(1f,Mathf.Abs(b.center.y-hit.point.y)+0.5f,ladderZ),rectList);
        }

        return o.GetComponent<Renderer>().bounds;
    }
    private Bounds CreateLadder(Vector3 position, Vector3 scale, List<Rect> rectList){
        GameObject o = Instantiate(ladder);
        o.transform.localScale = scale;
        o.transform.position = position;
        o.transform.SetParent(transform);
        o.AddComponent<TiledMesh>();

        if ( rectList != null ){
            BoxCollider bc = o.GetComponent<BoxCollider>();
            Rect re = new Rect(bc.bounds.min.x,bc.bounds.min.y,bc.bounds.size.x,bc.bounds.size.y);
            rectList.Add(re);
        }

        return o.GetComponent<Renderer>().bounds;
    }
}

public class RoomNode {
    public Vector3 position;
    public Rect rect;
    public List<RoomNode> neighbors;

    public RoomNode(){
        position = Vector3.zero;
        rect = new Rect();
        neighbors = new List<RoomNode>();
    }
}
