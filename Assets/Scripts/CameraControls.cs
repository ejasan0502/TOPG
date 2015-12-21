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

    void FixedUpdate(){
        if ( objectToFollow != null || posToMove != null ){
            Vector3 desiredPos = transform.position;
            if ( objectToFollow != null )
                desiredPos = objectToFollow.position;
            else if ( posToMove.HasValue )
                desiredPos = posToMove.Value;

            desiredPos.z = transform.position.z;
            transform.position = Vector3.Lerp(transform.position,desiredPos,Time.deltaTime*followSpeed);
        }
    }

    public void Follow(Transform pos){
        DebugWindow.LogSystem(GetType().Name,System.Reflection.MethodBase.GetCurrentMethod().Name);
        objectToFollow = pos;
        posToMove = null;
    }
    public void Move(Vector3 pos){
        DebugWindow.LogSystem(GetType().Name,System.Reflection.MethodBase.GetCurrentMethod().Name);
        posToMove = pos;
        objectToFollow = null;
    }
    public void ClearFollowing(){
        DebugWindow.LogSystem(GetType().Name,System.Reflection.MethodBase.GetCurrentMethod().Name);
        posToMove = null;
        objectToFollow = null;
    }
}
