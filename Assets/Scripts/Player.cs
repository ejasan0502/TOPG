using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Player {
    
    public List<Pet> pets;
    public Inventory inventory;
    public int crystals;

    public Player(){
        pets = new List<Pet>();
        inventory = new Inventory();
        crystals = 0;
    }

    public void AddPet(Pet p){
        if ( !pets.Contains(p) ){
            pets.Add(p);
            GameObject.Instantiate(p);
            Camera.main.orthographicSize = 5;
        }
    }
}
