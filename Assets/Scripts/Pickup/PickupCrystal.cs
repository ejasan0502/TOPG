using UnityEngine;
using System.Collections;

public class PickupCrystal : MonoBehaviour, Pickup {
    
    private int crystals = 0;
    private bool init = false;
    private Pet pet = null;

    void Update(){
        if ( pet != null ){
            transform.position = Vector3.Lerp(transform.position,pet.transform.position+(new Vector3(0f,0.5f,0f)),10f*Time.deltaTime);
            if ( Vector3.Distance(transform.position,pet.transform.position+(new Vector3(0f,0.5f,0f))) < 0.2f )
                Destroy(gameObject);
        }
    }

    public void Initialize(int x){
        DebugWindow.LogSystem(GetType().Name,System.Reflection.MethodBase.GetCurrentMethod().Name);
        crystals = x;
        init = true;
    }
    public void Initialize(InventoryItem i){}
    public void Interact(Pet p){
        DebugWindow.LogSystem(GetType().Name,System.Reflection.MethodBase.GetCurrentMethod().Name);
        if ( init && pet == null  ){
            pet = p;
            GameManager.instance.player.crystals += crystals;
        }
    }
}
