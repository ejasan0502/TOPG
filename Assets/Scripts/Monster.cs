using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
public class Monster : MonoBehaviour, Character {

    public bool flying = false;
    public float movtSpd = 3f;
    public float attackRange = 3f;
    public float viewDistance = 10f;
    public MonsterType monsterType;
    public Stats currentStats;
    public Stats maxStats;

    private State state = State.idle;
    private Renderer ground;
    private float wanderTime;
    private float wanderFrequency = 3f;
    public Vector3 wanderPos;
    private Vector3 velocity = Vector3.zero;

    private CharacterController characterController;
    private Animator anim;
    private Pet target = null;
    private bool attacking = false;

    public bool isAlive {
        get {
            return currentStats.health > 0;
        }
    }
    public bool hasTarget {
        get {
            return target != null;
        }
    }
    public float damage {
        get {
            return currentStats.baseDmg;
        }
    }

    public Pet GetTarget(){
        return target;
    }
    public void SetTarget(Pet p){
        DebugWindow.LogSystem(GetType().Name,System.Reflection.MethodBase.GetCurrentMethod().Name);
        target = p;
        SetState(State.chase);
    }
    public void SetState(State s){
        DebugWindow.LogSystem(GetType().Name,System.Reflection.MethodBase.GetCurrentMethod().Name);
        state = s;
    }

    void Start(){
        DebugWindow.LogSystem(GetType().Name,System.Reflection.MethodBase.GetCurrentMethod().Name);
        characterController = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();

        RaycastHit hit;
        if ( Physics.Raycast(transform.position,transform.TransformDirection(Vector3.down),out hit,100f, 1 << LayerMask.NameToLayer("Ground"))){
            ground = hit.collider.GetComponent<Renderer>();
        }
    }
    void Update(){
        if ( isAlive && monsterType != MonsterType.dummy )
            StateMachine();
    }
    void LateUpdate(){
        characterController.Move(velocity * Time.deltaTime);
        anim.SetFloat("Speed",Mathf.Abs(velocity.x));
    }

    private void StateMachine(){
        switch (state){
        case State.idle:
            FindTarget();
        break;
        case State.chase:
            if ( target != null )
                ChaseTarget();
            else
                SetState(State.idle);
        break;
        case State.attack:
            // Double check target is within attack range, in case player has avoided attack
            if ( Vector3.Distance(transform.position,target.transform.position) < attackRange ){
                attacking = true;
                anim.SetBool("Attack",attacking);
            } else if ( !attacking ){
                SetState(State.chase);
            } else {
                attacking = false;
                anim.SetBool("Attack",attacking);
            }
        break;
        }
    }
    private void Wander(){
        if ( ground != null ){
            velocity = Vector3.zero;
            if ( Time.time - wanderTime >= wanderFrequency ){
                int count = 0;
                wanderPos.x = Random.Range(ground.bounds.min.x,ground.bounds.max.x);
                while ( Vector3.Distance(transform.position,wanderPos) < 3f || count > 3 ){
                    wanderPos.x = Random.Range(ground.bounds.min.x,ground.bounds.max.x);
                    count++;
                }

                wanderFrequency = Random.Range(3f,7f);
                wanderTime = Time.time;
            }

            if ( Vector3.Distance(transform.position,wanderPos) > 0.1f ) {
                if ( transform.position.x - wanderPos.x > 0 ){
                    transform.LookAt(Vector3.left+transform.position);
                } else {
                    transform.LookAt(Vector3.right+transform.position);
                }
                velocity = transform.forward * 5f;
            }
        }
    }
    private void FindTarget(){
        RaycastHit hit;
        if ( Physics.Raycast(transform.position,transform.forward,out hit,viewDistance, 1 << LayerMask.NameToLayer("Character")) ){
            if ( hit.collider.GetComponent<Pet>() != null ){
                Pet p = hit.collider.GetComponent<Pet>();
                target = p;
            } else {
                Monster m = hit.collider.GetComponent<Monster>();
                Pet p = m.GetTarget();
                if ( p != null ){
                    SetTarget(p);
                }
            }
        }
    }
    private void ChaseTarget(){
        if ( Vector3.Distance(transform.position,target.transform.position) < attackRange ){
            SetState(State.attack);
        } else {
            if ( transform.position.x - target.transform.position.x < 0 ){
                // Left 
                transform.LookAt(Vector3.left+transform.position);
            } else {
                // Right
                transform.LookAt(Vector3.right+transform.position);
            }

            if ( flying ){
                velocity = transform.position - target.transform.position;
                if ( velocity.x > 1 ) velocity.x = 1f;
                else if ( velocity.x < -1 ) velocity.x = -1f;
                if ( velocity.y > 1 ) velocity.y = 1f;
                else if ( velocity.y < -1 ) velocity.y = -1f;
                velocity *= movtSpd;
            } else {
                velocity = transform.forward*movtSpd;
            }
        }
    }
    
    public void Hit(float dmg){
        DebugWindow.LogSystem(GetType().Name,System.Reflection.MethodBase.GetCurrentMethod().Name);
        currentStats.health -= dmg - currentStats.baseDef;
        if ( currentStats.health < 1 ){
            currentStats.health = 0f;
            Death();
        }
    }
    public void OnAttackEnd(){
        DebugWindow.LogSystem(GetType().Name,System.Reflection.MethodBase.GetCurrentMethod().Name);
        RaycastHit hit;
        Vector3 pos = transform.position;
        pos.y += characterController.height/2.0f;
        if ( Physics.Raycast(pos,transform.forward,out hit,attackRange,1 << LayerMask.NameToLayer("Character")) ){
            Character target = hit.collider.GetComponent<Character>();
            if ( target != null ){
                target.Hit(damage);
            }
        }
    }

    private void Death(){
        DebugWindow.LogSystem(GetType().Name,System.Reflection.MethodBase.GetCurrentMethod().Name);
        anim.SetBool("Sleep",true);
    }
}

public enum MonsterType {
    dummy,
    basic,
    melee,
    ranged,
    special
}
