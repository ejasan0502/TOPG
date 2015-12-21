using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TouchManager : MonoBehaviour {

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
    private float startTime = 0f;
    private int siblingIndex = 0;

    private bool petting = false;
    void Start(){
        DebugWindow.LogSystem(GetType().Name,System.Reflection.MethodBase.GetCurrentMethod().Name);
        if ( GameObject.FindObjectsOfType<TouchManager>().Length > 1 ){
            DestroyImmediate(gameObject);
        }
        DontDestroyOnLoad(this);

        startTime = Time.time;
    }
    void Update(){
        if ( Application.isMobilePlatform ){
            if ( Input.touchCount > 0 ){
                Touch touch = Input.touches[0];
                if ( touch.phase == TouchPhase.Began ){
                    if ( !InDeadZone() ){
                        #region Selecting an object
                        if ( selectedObj != null ){
                            if ( selectedObj.isDraggable ){
                                siblingIndex = selectedObj.transform.GetSiblingIndex();
                                selectedObj.transform.SetAsLastSibling();
                            }

                            SpecialInteraction si = selectedObj.GetComponent<SpecialInteraction>();
                            if ( si != null ) si.OnEnter();
                        #endregion
                        #region Move Camera
                        } else if ( !PlayerControls.instance.gameObject.activeSelf ){
                            if ( camera_onClickMove ){
                                CameraControls.instance.Move(Camera.main.ScreenToWorldPoint(Input.mousePosition));
                            } else {
                                selectedObj = GetInteractable();
                            }
                        }
                        #endregion
                    }
                    prevMousePos = Input.mousePosition;
                }
                if ( touch.phase == TouchPhase.Stationary || touch.phase == TouchPhase.Moved ){
                    #region An object was selected
                    if ( selectedObj != null ){
                        if ( selectedObj.isDraggable ){
                            Vector3 desiredPos = Vector3.zero;
                            desiredPos = Input.mousePosition;
                            desiredPos.z = selectedObj.transform.position.z;
                            selectedObj.transform.position = desiredPos;
                        } else if ( selectedObj.isPet ){
                            if ( Time.time - startTime >= 1f && Vector3.Distance(prevMousePos,Input.mousePosition) > 1f ){
                                Pet p = selectedObj.GetComponent<Pet>();
                                p.BePetted();

                                startTime = Time.time;
                                prevMousePos = Input.mousePosition;
                                petting = true;
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
                    } else if ( !PlayerControls.instance.gameObject.activeSelf ){
                    #endregion
                    #region Move Camera
                        if ( !camera_onClickMove && !InDeadZone() ){
                            Vector3 desiredPos = Camera.main.ScreenToWorldPoint(prevMousePos) - Camera.main.ScreenToWorldPoint(Input.mousePosition);
                            CameraControls.instance.Move(desiredPos);
                        }
                    }
                    #endregion
                }
                if ( touch.phase == TouchPhase.Ended ){
                    if ( selectedObj != null ){
                        if ( selectedObj.isDraggable ){
                            selectedObj.transform.SetSiblingIndex(siblingIndex);
                        } else if ( selectedObj.isPet  && !petting ){
                            petInfo.transform.GetChild(0).GetComponent<PetStatsHud>().pet = selectedObj.GetComponent<Pet>();
                            petInfo.SetActive(!petInfo.activeSelf);
                            if ( petInfo.activeSelf ){
                                Pet p = selectedObj.GetComponent<Pet>();
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
            }
        } else {
            if ( Input.GetMouseButtonDown(0) ){
                if ( !InDeadZone() ){
                    #region Selecting an object
                    if ( selectedObj != null ){
                        if ( selectedObj.isDraggable ){
                            siblingIndex = selectedObj.transform.GetSiblingIndex();
                            selectedObj.transform.SetAsLastSibling();
                        }

                        SpecialInteraction si = selectedObj.GetComponent<SpecialInteraction>();
                        if ( si != null ) si.OnEnter();
                    #endregion
                    #region Move Camera
                    } else if ( !PlayerControls.instance.gameObject.activeSelf ){
                        if ( camera_onClickMove ){
                            CameraControls.instance.Move(Camera.main.ScreenToWorldPoint(Input.mousePosition));
                        } else {
                            selectedObj = GetInteractable();
                        }
                    }
                    #endregion
                }
                prevMousePos = Input.mousePosition;
            }
            if ( Input.GetMouseButton(0) ){
                #region An object was selected
                if ( selectedObj != null ){
                    if ( selectedObj.isDraggable ){
                        Vector3 desiredPos = Vector3.zero;
                        desiredPos = Input.mousePosition;
                        desiredPos.z = selectedObj.transform.position.z;
                        selectedObj.transform.position = desiredPos;
                    } else if ( selectedObj.isPet ){
                        if ( Time.time - startTime >= 1f && Vector3.Distance(prevMousePos,Input.mousePosition) > 1f ){
                            Pet p = selectedObj.GetComponent<Pet>();
                            p.BePetted();

                            startTime = Time.time;
                            prevMousePos = Input.mousePosition;
                            petting = true;
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
                } else if ( !PlayerControls.instance.gameObject.activeSelf ){
                #endregion
                #region Move Camera
                    if ( !camera_onClickMove && !InDeadZone() ){
                        Vector3 desiredPos = Camera.main.ScreenToWorldPoint(prevMousePos) - Camera.main.ScreenToWorldPoint(Input.mousePosition);
                        CameraControls.instance.Move(desiredPos);
                    }
                }
                #endregion
            }
            if ( Input.GetMouseButtonUp(0) ){
                if ( selectedObj != null ){
                    if ( selectedObj.isDraggable ){
                        selectedObj.transform.SetSiblingIndex(siblingIndex);
                    } else if ( selectedObj.isPet  && !petting ){
                        petInfo.transform.GetChild(0).GetComponent<PetStatsHud>().pet = selectedObj.GetComponent<Pet>();
                        petInfo.SetActive(!petInfo.activeSelf);
                        if ( petInfo.activeSelf ){
                            Pet p = selectedObj.GetComponent<Pet>();
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
        }
    }

    private Interactable GetInteractable(){
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
