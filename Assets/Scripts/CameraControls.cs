using UnityEngine;
using System.Collections;

public class CameraControls : MonoBehaviour {
    private static Object lockObject = new Object();
    private static CameraControls _instance;
    public static CameraControls instance {
        get {
            lock (lockObject){
                if ( _instance == null ){
                    _instance = Camera.main.GetComponent<CameraControls>();
                }
            }
            return _instance;
        }
    }

    public float followSpeed = 1f;
    public Transform objectToFollow;
    public Vector3? posToMove = null;

    public Rect? boundingRect = null;

    private float cameraWidth;
    private float cameraHeight;

    public void SetRect(Rect r){
        boundingRect = r;
    }

    void Start(){
        Vector3 min = Camera.main.ScreenToWorldPoint(new Vector3(0f,0f,0f));
        Vector3 max = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width,Screen.height,0f));

        cameraWidth = max.x - min.x;
        cameraHeight = max.y - min.y;
    }
    void FixedUpdate(){
        if ( objectToFollow != null || posToMove != null ){
            Vector3 desiredPos = transform.position;
            if ( objectToFollow != null ){
                desiredPos = objectToFollow.position;
            } else if ( posToMove.HasValue )
                desiredPos = posToMove.Value;

            desiredPos.z = transform.position.z;

            if ( boundingRect.HasValue ){
                Rect r = boundingRect.Value;
                if ( desiredPos.x - cameraWidth/2.0f < r.min.x )
                    desiredPos.x = r.min.x + cameraWidth/2.0f;
                else if ( desiredPos.x + cameraWidth/2.0f > r.max.x )
                    desiredPos.x = r.max.x - cameraWidth/2.0f;

                if ( desiredPos.y - cameraHeight/2.0f < r.min.y )
                    desiredPos.y = r.min.y + cameraHeight/2.0f;
                else if ( desiredPos.y + cameraHeight/2.0f > r.max.y )
                    desiredPos.y = r.max.y - cameraHeight/2.0f;
            }

            transform.position = Vector3.Lerp(transform.position,desiredPos,Time.deltaTime*followSpeed);
        }
    }

    public void Follow(Transform pos){
        objectToFollow = pos;
        posToMove = null;
    }
    public void Move(Vector3 pos){
        posToMove = pos;
        objectToFollow = null;
    }
    public void ClearFollowing(){
        DebugWindow.LogSystem(GetType().Name,System.Reflection.MethodBase.GetCurrentMethod().Name);
        posToMove = null;
        objectToFollow = null;
    }
}
