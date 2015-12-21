using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CharacterHealthBar : MonoBehaviour {

    public bool faceCamera = false;

    public Monster monster;
    public Pet pet;

    public Image healthFillBar;

    void Update(){
        if ( pet != null ){
            healthFillBar.fillAmount = pet.currentStats.health/pet.maxStats.health;
        } else if ( monster != null ){
            healthFillBar.fillAmount = monster.currentStats.health/monster.maxStats.health;
        }

        if ( faceCamera )
            transform.LookAt(Camera.main.transform);
    }
}
