using UnityEngine;
using System.Reflection;
using System.Collections;

[RequireComponent(typeof(Interactable))]
public class Pet : MonoBehaviour, Character {
    
    public string description = "";
    public float movtSpd = 5f;
    public float attackRange = 2f;
    public GameObject mesh;
    public Transform weaponPos;
    public Transform hatPos;

    public Stats currentStats;
    public Stats maxStats = new Stats(
        100f,
        1f,
        0f,
        100f,
        100f,
        100f,
        100f,
        100f,
        100f
    );

    public Stats statDrain = new Stats(
        0f,
        0f,
        0f,
        0.15f,
        0.3f,
        0.1f,
        0.3f,
        0f,
        -0.1f
    );
    public Stats statDrainFrequency = new Stats(
        0f,
        0f,
        0f,
        5f,
        3f,
        10f,
        3f,
        0f,
        1f
    );

    public LevelStat[] levelStats = new LevelStat[3]{
        new LevelStat("attacking",1,0),
        new LevelStat("mining",1,0),
        new LevelStat("gathering",1,0)
    };

    public Equip[] equipment = new Equip[3] {
        null,
        null,
        null
    };

    private Material bodyMaterial;
    private float[] drainTime;
    private FieldInfo[] statFields;
    private State state = State.idle;
    [HideInInspector] public Animator anim;
    private GameObject primaryWeapon = null;
    [HideInInspector] public PlayerControls playerControls;

    private CharacterController characterController;
    private Renderer ground = null;
    private float wanderTime;
    private float wanderFrequency = 3f;
    public Vector3 wanderPos;
    private Vector3 velocity = Vector3.zero;
    [HideInInspector] public bool selected = false;
    public bool isGrounded = false;
    
    public bool isAlive {
        get {
            return currentStats.health > 0;
        }
    }
    public float damage {
        get {
            return currentStats.baseDmg;
        }
    }
    public GameObject obj {
        get {
            return gameObject;
        }
    }

    private float invincibilityStartTime = 0f;
    private float invincibilityFrequency = 3f;

    void Start(){
        DebugWindow.LogSystem(GetType().Name,System.Reflection.MethodBase.GetCurrentMethod().Name);
        currentStats = new Stats(maxStats);
        currentStats.temperature = maxStats.temperature/2.0f;
        currentStats.hygiene = 0;

        statFields = statDrainFrequency.GetType().GetFields();
        drainTime = new float[statFields.Length];

        anim = GetComponent<Animator>();
        playerControls = PlayerControls.instance;

        characterController = GetComponent<CharacterController>();
        wanderPos = transform.position;
        wanderTime = Time.time;

        foreach (Material m in mesh.GetComponent<Renderer>().materials){
            if ( m.name.Contains("body") ){
                bodyMaterial = m;
            }
        }

        RaycastHit hit;
        if ( Physics.Raycast(transform.position,transform.TransformDirection(Vector3.down),out hit,100f, 1 << LayerMask.NameToLayer("Ground"))){
            ground = hit.collider.GetComponent<Renderer>();
        }
    }
    void Update(){
        if ( !selected && playerControls.pet == null ){
            switch(state){
            case State.idle:
            Wander();
            break;
            }
        } else if ( selected ){
            transform.LookAt(Camera.main.transform);
            anim.SetFloat("Speed",0f);
        }
    }
    void FixedUpdate(){
        for (int i = 0; i < statFields.Length; i++){
            if ( (float)statFields[i].GetValue(statDrainFrequency) != 0 && Time.time - drainTime[i] >= (float)statFields[i].GetValue(statDrainFrequency) ){
                FieldInfo[] current = currentStats.GetType().GetFields();
                FieldInfo[] drain = statDrain.GetType().GetFields();

                float val = (float)current[i].GetValue(currentStats) - (float)drain[i].GetValue(statDrain);
                if ( val > 100 ) val = 100f;
                else if ( val < 0 ) val = 0f;
                current[i].SetValue(currentStats,val);

                drainTime[i] = Time.time;
            }
        }
    }

    private void UpdateEquipStats(){
        DebugWindow.LogSystem(GetType().Name,System.Reflection.MethodBase.GetCurrentMethod().Name);
        maxStats.baseDmg = 1f;
        maxStats.baseDef = 0f;

        foreach (Equip e in equipment){
            if ( e != null ){
                maxStats.baseDmg += e.baseDmg;
                maxStats.baseDef += e.baseDef;
            }
        }

        currentStats.baseDmg = maxStats.baseDmg;
        currentStats.baseDef = maxStats.baseDef;
    }
    private void Wander(){
        if ( ground != null ){
            velocity.x = 0f;
            if ( isGrounded ){
                if ( Time.time - wanderTime >= wanderFrequency ){
                    int count = 0;
                    wanderPos.x = Random.Range(ground.bounds.min.x,ground.bounds.max.x);
                    while ( Vector3.Distance(transform.position,wanderPos) < 3f || count > 3 ){
                        wanderPos.x = Random.Range(ground.bounds.min.x,ground.bounds.max.x);
                        count++;
                    }

                    if ( transform.position.x - wanderPos.x > 0 ){
                        transform.LookAt(Vector3.left+transform.position);
                    } else {
                        transform.LookAt(Vector3.right+transform.position);
                    }

                    wanderFrequency = Random.Range(3f,7f);
                    wanderTime = Time.time;
                }

                if ( Vector3.Distance(transform.position,wanderPos) > 0.5f ) {
                    velocity = transform.forward * movtSpd;
                }
            }

            if ( Physics.Raycast(transform.position,transform.TransformDirection(Vector3.down),0.1f, 1 << LayerMask.NameToLayer("Ground"))){
                isGrounded = true;
            } else {
                isGrounded = false;
                wanderPos = transform.position;
            }
            if ( !isGrounded ) 
                velocity.y += Physics.gravity.y * Time.deltaTime;
            characterController.Move(velocity * Time.deltaTime);
            anim.SetFloat("Speed",Mathf.Abs(velocity.x));
        }
    }

    public void BePetted(){
        DebugWindow.LogSystem(GetType().Name,System.Reflection.MethodBase.GetCurrentMethod().Name);
        currentStats.happy += 5f;
        if ( currentStats.happy > maxStats.happy ){
            currentStats.happy = maxStats.happy;
        }
    }
    public void Equip(int slotIndex){
        DebugWindow.LogSystem(GetType().Name,System.Reflection.MethodBase.GetCurrentMethod().Name);
        Inventory inv = GameManager.instance.player.inventory;

        if ( slotIndex < inv.items.Count && inv.items[slotIndex] != null && inv.items[slotIndex].item != null ){
            Equip e = inv.items[slotIndex].item.AsEquip;
            if ( e != null ){
                equipment[(int)e.equipType] = e;

                if ( e.equipType == EquipType.costume ){
                    bodyMaterial.SetTexture("_BlendTex",Resources.Load<Texture>(e.assetPath));
                } else if ( e.equipType == EquipType.weapon ){
                    if ( primaryWeapon != null ){
                        Destroy(primaryWeapon);
                        primaryWeapon = null;
                    }
                    primaryWeapon = Instantiate(Resources.Load<GameObject>(e.assetPath));
                    primaryWeapon.transform.SetParent(weaponPos);
                    primaryWeapon.transform.position = weaponPos.position;
                    primaryWeapon.transform.rotation = Quaternion.identity;
                }
                inv.Remove(slotIndex,1);

                UpdateEquipStats();
            } else {
                DebugWindow.Log("Cannot equip inventory slot " + slotIndex + ". It is not an equip.");
            }
        }
    }
    public void Unequip(int slotIndex){
        DebugWindow.LogSystem(GetType().Name,System.Reflection.MethodBase.GetCurrentMethod().Name);
        if ( slotIndex < equipment.Length )
            equipment[slotIndex] = null;

        UpdateEquipStats();
    }
    public void AddToCurrentStats(Stats s){
        DebugWindow.LogSystem(GetType().Name,System.Reflection.MethodBase.GetCurrentMethod().Name);
        currentStats += s;

        FieldInfo[] current = currentStats.GetType().GetFields();
        FieldInfo[] max = maxStats.GetType().GetFields();
        for (int i = 0; i < current.Length; i++){
            if ( (float)current[i].GetValue(currentStats) > (float)max[i].GetValue(maxStats) ){
                current[i].SetValue(currentStats, (float)max[i].GetValue(maxStats));
            } else if ( (float)current[i].GetValue(currentStats) < 0 )
                current[i].SetValue(currentStats, 0f);
        }
    }
    public void SubtractFromCurrentStats(Stats s){
        DebugWindow.LogSystem(GetType().Name,System.Reflection.MethodBase.GetCurrentMethod().Name);
        currentStats -= s;

        FieldInfo[] current = currentStats.GetType().GetFields();
        FieldInfo[] max = maxStats.GetType().GetFields();
        for (int i = 0; i < current.Length; i++){
            if ( (float)current[i].GetValue(currentStats) > (float)max[i].GetValue(maxStats) ){
                current[i].SetValue(currentStats, (float)max[i].GetValue(maxStats));
            } else if ( (float)current[i].GetValue(currentStats) < 0 )
                current[i].SetValue(currentStats, 0f);
        }
    }

    public void Hit(float dmg, Character c){
        if ( !isAlive ) return;
        if ( Time.time - invincibilityStartTime >= invincibilityFrequency ){
            DebugWindow.LogSystem(GetType().Name,System.Reflection.MethodBase.GetCurrentMethod().Name);
            currentStats.health -= dmg - currentStats.baseDef;
            invincibilityStartTime = Time.time;
            playerControls.KnockBack(c.obj.transform.position-transform.position);
            if ( currentStats.health < 1 ){
                currentStats.health = 0f;
                Death();
            }
        }
    }
    public void OnAttackEnd(){
        DebugWindow.LogSystem(GetType().Name,System.Reflection.MethodBase.GetCurrentMethod().Name);
        if ( !isAlive ) return;

        RaycastHit hit;
        Vector3 pos = transform.position;
        pos.y += characterController.height/2.0f;
        if ( Physics.Raycast(pos,transform.forward,out hit,attackRange,1 << LayerMask.NameToLayer("Character")) ){
            Character target = hit.collider.GetComponent<Character>();
            if ( target != null ){
                target.Hit(damage,this);
            }
        }
    }

    private void Death(){
        DebugWindow.LogSystem(GetType().Name,System.Reflection.MethodBase.GetCurrentMethod().Name);
    }
}

public enum State {
    idle,
    chase,
    attack
}
public class LevelStat {
    public string name;
    public int level;
    public float exp;
    public float maxExp;

    public LevelStat(string n, int l, float xp){
        name = n;
        level = l;
        exp = xp;

        maxExp = 3 + 0.135f * level * level ;
    }
}
