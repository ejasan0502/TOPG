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
}
