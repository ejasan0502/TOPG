using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TouchManager : MonoBehaviour {

    public GameObject travelInfo;
    public GameObject petInfo;
    public bool camera_onClickMove = true;
    public RectTransform[] deadZones;
    public GameObject[] hudElements;

    private static Object lockObj = new Object();
    private static TouchManager _instance;
    public static TouchManager instance {
        get {
            lock (lockObj){
                if ( _instance == null ){
                    _instance = GameObject.FindObjectOfType<TouchManager>();
                }
            }
            return _instance;
        }
    }

    private Interactable selectedObj = null;
    private Vector3 prevMousePos = Vector3.zero;
    private Vector3 originPos = Vector3.zero;
    private float startTime = 0f;
    private int siblingIndex = 0;

    private bool petting = false;
    private bool traveling = false;
    private Pet travelingPet = null;

    public Pet GetTravelingPet {
        get {
            return travelingPet;
        }
    }

    void Start(){
        DebugWindow.LogSystem(GetType().Name,System.Reflection.MethodBase.GetCurrentMethod().Name);
        if ( GameObject.FindObjectsOfType<TouchManager>().Length > 1 ){
            DestroyImmediate(gameObject);
        }
        DontDestroyOnLoad(this);

        startTime = Time.time;
    }
    void Update(){
        #region Mobile
        if ( Application.isMobilePlatform ){
            if ( Input.touchCount > 0 ){
                Touch touch = Input.touches[0];
                if ( touch.phase == TouchPhase.Began ){
                    OnEnter();
                }
                if ( touch.phase == TouchPhase.Stationary || touch.phase == TouchPhase.Moved ){
                    OnHold();
                }
                if ( touch.phase == TouchPhase.Ended ){
                    OnExit();
                }
            }
        } else {
        #endregion
        #region Editor/PC
            if ( Input.GetMouseButtonDown(0) ){
                OnEnter();
            }
            if ( Input.GetMouseButton(0) ){
                OnHold();
            }
            if ( Input.GetMouseButtonUp(0) ){
                OnExit();
            }
        }
        #endregion
    }

    private void OnEnter(){
        DebugWindow.LogSystem(GetType().Name,System.Reflection.MethodBase.GetCurrentMethod().Name);
        if ( !InDeadZone() ){
            #region Selecting an object
            selectedObj = GetInteractable();
            if ( selectedObj != null ){
                if ( selectedObj.isDraggable ){
                    originPos = selectedObj.transform.position;
                    siblingIndex = selectedObj.transform.GetSiblingIndex();
                    selectedObj.transform.SetAsLastSibling();
                }

                SpecialInteraction si = selectedObj.GetComponent<SpecialInteraction>();
                if ( si != null ) si.OnEnter();
            #endregion
            #region Move Camera
            } else if ( PlayerControls.instance.pet == null ){
                if ( camera_onClickMove ){
                    CameraControls.instance.Move(Camera.main.ScreenToWorldPoint(Input.mousePosition));
                }
            }
            #endregion
        }
        prevMousePos = Input.mousePosition;
    }
    private void OnHold(){
        #region An object was selected
        if ( selectedObj != null ){
            if ( selectedObj.isDraggable ){
                Vector3 desiredPos = Vector3.zero;
                desiredPos = Input.mousePosition;
                desiredPos.z = selectedObj.transform.position.z;
                selectedObj.transform.position = desiredPos;
            } else if ( selectedObj.isPet ){
                if ( petInfo.activeSelf )
                    if ( Time.time - startTime >= 1f && Vector3.Distance(prevMousePos,Input.mousePosition) > 1f ){
                        Pet p = selectedObj.GetComponent<Pet>();
                        p.BePetted();

                        startTime = Time.time;
                        prevMousePos = Input.mousePosition;
                        petting = true;
                    }
                 else {
                    travelInfo.SetActive(true);
                    travelingPet = selectedObj.GetComponent<Pet>();
                    traveling = true;
                 }
            }

            SpecialInteraction si = selectedObj.GetComponent<SpecialInteraction>();
            if ( si != null ) {
                if ( Vector3.Distance(prevMousePos,Input.mousePosition) < 1f ){
                    si.OnHold();
                } else {
                    si.OnDrag();
                }
            }
        } else if ( PlayerControls.instance.pet == null ){
        #endregion
        #region Move Camera
            if ( !camera_onClickMove && !InDeadZone() ){
                Vector3 desiredPos = Camera.main.ScreenToWorldPoint(prevMousePos) - Camera.main.ScreenToWorldPoint(Input.mousePosition);
                CameraControls.instance.Move(desiredPos);
            }
        }
        #endregion
    }
    private void OnExit(){
        if ( selectedObj != null ){
            if ( selectedObj.isDraggable ){
                selectedObj.transform.SetSiblingIndex(siblingIndex);
                selectedObj.transform.position = originPos;
            } else if ( selectedObj.isPet  && !petting && PlayerControls.instance.pet == null ){
                petInfo.SetActive(!petInfo.activeSelf);
                if ( petInfo.activeSelf ){
                    Pet p = selectedObj.GetComponent<Pet>();
                    petInfo.transform.GetChild(0).GetComponent<PetStatsHud>().pet = p;
                    p.selected = true;
                    CameraControls.instance.Follow(selectedObj.transform);
                } else {
                    Pet p = selectedObj.GetComponent<Pet>();
                    p.selected = false;
                    CameraControls.instance.ClearFollowing();
                }
            }

            SpecialInteraction si = selectedObj.GetComponent<SpecialInteraction>();
            if ( si != null ) si.OnExit();

            selectedObj = null;
            petting = false;
        }
    }

    private Interactable GetInteractable(){
        DebugWindow.LogSystem(GetType().Name,System.Reflection.MethodBase.GetCurrentMethod().Name);
        foreach (Interactable o in GameObject.FindObjectsOfType<Interactable>()){
            if ( o.isUI ){
                RectTransform rt = o.transform as RectTransform;
                if ( RectTransformUtility.RectangleContainsScreenPoint(rt,Input.mousePosition,null) ){
                    return o;
                }
            } else {
                RaycastHit hit;
                if ( Physics.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition),Camera.main.transform.forward*1000f, out hit) ){
                    Interactable i = hit.collider.GetComponent<Interactable>();
                    if ( i != null && i == o ){
                        return o;
                    }
                }
            }
        }
        return null;
    }
    private bool InDeadZone(){
        foreach (RectTransform rt in deadZones){
            if ( rt.gameObject.activeSelf && RectTransformUtility.RectangleContainsScreenPoint(rt,Input.mousePosition,null) ){
                return true;
            }
        }

        return false;
    }

    public void CloseAll(){
        DebugWindow.LogSystem(GetType().Name,System.Reflection.MethodBase.GetCurrentMethod().Name);
        foreach (Pet p in GameObject.FindObjectsOfType<Pet>()){
            p.selected = false;
        }

        selectedObj = null;
        petting = false;

        foreach (GameObject o in hudElements){
            o.SetActive(false);
        }
    }
}
