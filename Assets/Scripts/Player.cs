using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Player {
    
    public List<Pet> pets;
    public Inventory inventory;

    public Player(){
        pets = new List<Pet>();
        inventory = new Inventory();
    }

    public void AddPet(Pet p){
        if ( !pets.Contains(p) ){
            pets.Add(p);
            Pet o = GameObject.Instantiate(p);
            o.gameObject.AddComponent<TempGravity>();
            Camera.main.orthographicSize = 5;
        }
    }
}
