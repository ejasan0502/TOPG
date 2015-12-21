using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerControls : MonoBehaviour {
    
    public Pet pet;
    public float speed = 5f;
    public float jumpForce = 10f;
    public float freeFallDuration = 3f;
    public GameObject petInfo;

    private CharacterController characterController;
    private Animator anim;

    public Vector3 velocity = Vector3.zero;
    public Vector3 ledgePoint = Vector3.zero;
    public bool isGrounded = false;
    public bool freeFalling = false;
    private float freeFallTime;
    private bool attacking = false;
    public bool knockBack = false;
    private Vector3 impact = Vector3.zero;
    private float knockBackTime;

    private static Object lockObject = new Object();
    private static PlayerControls _instance;
    public static PlayerControls instance {
        get {
            lock (lockObject){
                if ( _instance == null ){
                    _instance = GameObject.FindObjectOfType<PlayerControls>();
                }
            }
            return _instance;
        }
    }

    void Start(){
        DebugWindow.LogSystem(GetType().Name,System.Reflection.MethodBase.GetCurrentMethod().Name);
        if ( pet != null ){
            characterController = pet.GetComponent<CharacterController>();
            anim = pet.GetComponent<Animator>();
        }

        if ( !Application.isMobilePlatform ){
            foreach (Transform t in transform){
                t.gameObject.SetActive(false);
            }
        }
    }
    void Update(){
        if ( !Application.isMobilePlatform ){
            if ( !petInfo.activeSelf ){
                if ( Input.GetKey(KeyCode.A) ){
                    MoveLeft();
                }
                if ( Input.GetKeyUp(KeyCode.A) ){
                    OnMoveUp();
                }
                if ( Input.GetKey(KeyCode.D) ){
                    MoveRight();
                }
                if ( Input.GetKeyUp(KeyCode.D) ){
                    OnMoveUp();
                }
                if ( Input.GetKeyDown(KeyCode.Space) ){
                    Jump();
                }
                if ( Input.GetMouseButtonDown(0) ){
                    OnAttackDown();
                }
                if ( Input.GetMouseButtonUp(0) ){
                    OnAttackUp();
                }
            }
        }
    }
    void LateUpdate(){
        if ( !knockBack ){
            if ( freeFalling ) velocity.y += Physics.gravity.y * Time.deltaTime;
            characterController.Move(velocity * Time.deltaTime);
            anim.SetFloat("Speed",Mathf.Abs(characterController.velocity.x));
        } else {
            impact.y += Physics.gravity.y * Time.deltaTime;
            characterController.Move(impact * Time.deltaTime);
            if ( Time.time - knockBackTime >= 0.5f )
                knockBack = false;
        }
        CheckIfOnGround();
    }

    public void SetPet(Pet p){
        DebugWindow.LogSystem(GetType().Name,System.Reflection.MethodBase.GetCurrentMethod().Name);
        pet = p;
        characterController = p.GetComponent<CharacterController>();
        anim = pet.GetComponent<Animator>();
    }
    public void MoveLeft(){
        if ( pet != null && !attacking ){
            velocity.x = 0f;
            pet.transform.LookAt(Vector3.left+pet.transform.position);
            velocity.x = pet.transform.forward.x*speed;
        }
    }
    public void MoveRight(){
        if ( pet != null && !attacking ){
            velocity.x = 0f;
            pet.transform.LookAt(Vector3.right+pet.transform.position);
            velocity.x = pet.transform.forward.x*speed;
        }
    }
    public void OnMoveUp(){
        velocity.x = 0f;
    }
    public void Jump(){
        if ( pet != null && isGrounded && !attacking ){
            velocity.y = jumpForce;
            anim.SetBool("Jump",true);
        }
    }
    public void OnAttackDown(){
        if ( pet == null ) return;

        attacking = true;
        anim.SetBool("Attack",true);
    }
    public void OnAttackUp(){
        if ( pet == null ) return;

        attacking = false;
        anim.SetBool("Attack",false);
    }
    public void KnockBack(Vector3 direction){
        DebugWindow.LogSystem(GetType().Name,System.Reflection.MethodBase.GetCurrentMethod().Name);
        knockBack = true;
        var dir = -direction;
        dir.y = 1f;
        impact = dir*2.5f;
        knockBackTime = Time.time;
    }

    private void CheckIfOnGround(){
        RaycastHit hit;
        if ( Physics.Raycast(pet.transform.position,pet.transform.TransformDirection(Vector3.down),out hit,0.1f, 1 << LayerMask.NameToLayer("Ground"))){
            isGrounded = true;
            freeFalling = false;
            anim.SetBool("Jump",false);
        } else {
            isGrounded = false;
            if ( !freeFalling ){
                freeFalling = true;
                freeFallTime = Time.time;
                velocity.y = characterController.velocity.y;
            } else if ( Time.time - freeFallTime >= freeFallDuration ){
                anim.SetBool("Jump",true);
            }
        }
    }
}
