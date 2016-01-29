using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Screen_PetSelect : MonoBehaviour {

    public string selectedPet = "";
    public Button petButton;

    private Pet pet = null;

    public void SetPet(string s){
        Resources.UnloadUnusedAssets();

        pet = Resources.Load<Pet>("Pets/"+s);
        selectedPet = s;
        petButton.gameObject.SetActive(true);
    }

    public void Confirm(){
        if ( pet != null ){
            GameManager.instance.player.AddPet(pet);
            SceneManager.LoadScene("playerscene");
        }
    }

}
