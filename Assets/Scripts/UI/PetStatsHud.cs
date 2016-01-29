using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PetStatsHud : MonoBehaviour {
    public Pet pet;

    public Image happyBarFill;
    public Image hungryBarFill;
    public Image thirstyBarFill;
    public Image sleepyBarFill;
    public Image temperatureBarFill;
    public Image hygieneBarFill;

    void Update(){
        if ( pet != null ){
            happyBarFill.fillAmount = pet.currentStats.happy/pet.maxStats.happy;
            hungryBarFill.fillAmount = pet.currentStats.hungry/pet.maxStats.hungry;
            thirstyBarFill.fillAmount = pet.currentStats.thirsty/pet.maxStats.thirsty;
            sleepyBarFill.fillAmount = pet.currentStats.sleepy/pet.maxStats.sleepy;
            temperatureBarFill.fillAmount = pet.currentStats.temperature/pet.maxStats.temperature;
            hygieneBarFill.fillAmount = pet.currentStats.hygiene/pet.maxStats.hygiene;
        }
    }
}
