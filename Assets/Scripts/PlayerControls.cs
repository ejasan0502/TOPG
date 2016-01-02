using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerControls : MonoBehaviour {
    
    public bool testingMobile = false;
    public Pet pet;
    public float speed = 5f;
    public float jumpForce = 10f;
    public float freeFallDuration = 1f;
    public GameObject petInfo;
    public GameObject petHud;

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
    public bool climbing = false;
    public bool fromTop = false;
    public bool jumpLeft = false;
    public bool jumpRight = false;

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

        if ( !Application.isMobilePlatform && !testingMobile ){
            foreach (Transform t in transform){
                t.gameObject.SetActive(false);
            }
        }
    }
    void Update(){
        if ( !Application.isMobilePlatform && !testingMobile ){
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
                if ( Input.GetKey(KeyCode.W) ){
                    MoveUp();
                }
                if ( Input.GetKey(KeyCode.S) ){
                    MoveDown();
                }
                if ( Input.GetKeyUp(KeyCode.W) ){
                    OnClimbEnd();
                }
                if ( Input.GetKeyUp(KeyCode.S) ){
                    OnClimbEnd();
                }
            }
        }
    }
    void LateUpdate(){
        if ( pet != null ){
            if ( !knockBack ){
                if ( freeFalling && !climbing ) velocity.y += Physics.gravity.y * Time.deltaTime;
                characterController.Move(velocity * Time.deltaTime);
                if ( climbing  && !jumpLeft && !jumpRight )
                    anim.SetFloat("Speed",Mathf.Abs(characterController.velocity.y));
                else
                    anim.SetFloat("Speed",Mathf.Abs(characterController.velocity.x));
            } else {
                impact.y += Physics.gravity.y * Time.deltaTime;
                characterController.Move(impact * Time.deltaTime);
                if ( Time.time - knockBackTime >= 0.5f )
                    knockBack = false;
            }
            CheckIfOnGround();
            if ( climbing && isGrounded || pet.onLadder == null ){
                climbing = false;
                pet.SetCollisionIgnoreWithPlatforms(false);
            }
        }
    }

    public void SetPet(Pet p){
        DebugWindow.LogSystem(GetType().Name,System.Reflection.MethodBase.GetCurrentMethod().Name);
        pet = p;
        speed = pet.movtSpd;
        characterController = p.GetComponent<CharacterController>();
        anim = pet.GetComponent<Animator>();
    }
    public void MoveLeft(){
        if ( pet != null && !attacking && !climbing ){
            velocity.x = 0f;
            pet.transform.LookAt(Vector3.left+pet.transform.position);
            velocity.x = pet.transform.forward.x*speed;
        } else if ( climbing ){
            jumpLeft = true;
            jumpRight = false;
        }
    }
    public void MoveRight(){
        if ( pet != null && !attacking && !climbing ){
            velocity.x = 0f;
            pet.transform.LookAt(Vector3.right+pet.transform.position);
            velocity.x = pet.transform.forward.x*speed;
        } else if ( climbing ){
            jumpLeft = false;
            jumpRight = true;
        }
    }
    public void MoveUp(){
        if ( pet != null && pet.onLadder ){
            velocity.y = pet.movtSpd;
            climbing = true;
            pet.transform.position = new Vector3(pet.onLadder.transform.position.x,pet.transform.position.y,-0.25f);
            pet.transform.LookAt(pet.transform.position+Vector3.forward);
            pet.SetCollisionIgnoreWithPlatforms(true);
        }
    }
    public void MoveDown(){
        if ( pet != null ){
            if ( pet.onLadder && (!isGrounded || fromTop) ){
                velocity.y = -pet.movtSpd;
                climbing = true;
                pet.transform.position = new Vector3(pet.onLadder.transform.position.x,pet.transform.position.y,-0.25f);
                pet.transform.LookAt(pet.transform.position+Vector3.forward);
                pet.SetCollisionIgnoreWithPlatforms(true);
                fromTop = false;
            }
        }
    }
    public void OnMoveUp(){
        velocity.x = 0f;
    }
    public void OnClimbEnd(){
        velocity.y = 0f;
        jumpLeft = false;
        jumpRight = false;
    }
    public void Jump(){
        if ( pet != null ){
            if ( climbing ){
                if ( jumpLeft ){
                    velocity.x = 0f;
                    pet.transform.LookAt(Vector3.left+pet.transform.position);
                    velocity.x = pet.transform.forward.x*speed;
                } else if ( jumpRight ){
                    velocity.x = 0f;
                    pet.transform.LookAt(Vector3.right+pet.transform.position);
                    velocity.x = pet.transform.forward.x*speed;
                }
            } else if ( isGrounded && !attacking ){
                velocity.y = jumpForce;
                anim.SetBool("Jump",true);
            }
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
            if ( !climbing ) anim.SetBool("Jump",false);
        } else {
            isGrounded = false;
            if ( !freeFalling ){
                freeFalling = true;
                freeFallTime = Time.time;
                velocity.y = characterController.velocity.y;
            } else if ( Time.time - freeFallTime >= freeFallDuration ){
                if ( !climbing ) anim.SetBool("Jump",true);
            }
        }
    }
}
