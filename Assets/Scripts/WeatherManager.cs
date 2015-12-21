using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class WeatherManager : MonoBehaviour {

    public float secondsInADay = 1800f;
    public Season currentSeason;
    public Weather currentWeather;

    private float startGameTime;
    private Weather prevWeather = Weather.raining;
    private List<Dictionary<Weather,float>> possibles = new List<Dictionary<Weather,float>>(){
        #region Autumn
        new Dictionary<Weather,float>(){
            { Weather.raining, 10 },
            { Weather.storming, 40 },
            { Weather.sunny, 50 },
            { Weather.cloudy, 80 },
            { Weather.snowing, 90 },
            { Weather.foggy, 100 },
        },
        #endregion
        #region Winter
        new Dictionary<Weather,float>(){
            { Weather.raining, 10 },
            { Weather.storming, 50 },
            { Weather.cloudy, 70 },
            { Weather.snowing, 100 },
        },
        #endregion
        #region Spring
        new Dictionary<Weather,float>(){
            { Weather.raining, 40 },
            { Weather.storming, 60 },
            { Weather.sunny, 80 },
            { Weather.cloudy, 90 },
            { Weather.foggy, 100 },
        },
        #endregion
        #region Summer
        new Dictionary<Weather,float>(){
            { Weather.sunny, 20 },
            { Weather.dry, 50 },
            { Weather.windy, 60 },
            { Weather.hot, 100 }
        }
        #endregion
    };

    void Start(){
        DebugWindow.LogSystem(GetType().Name,System.Reflection.MethodBase.GetCurrentMethod().Name);
        startGameTime = Time.time;
    }
    void Update(){
        AdjustPetsTemperatures();
        if ( Time.time - startGameTime >= secondsInADay ){
            startGameTime = Time.time;
            NextDay();
        }
    }

    public Weather GetRandomWeather(){
        DebugWindow.LogSystem(GetType().Name,System.Reflection.MethodBase.GetCurrentMethod().Name);
        prevWeather = currentWeather;
        Weather weather = prevWeather;

        while (weather != prevWeather ){
            int x = UnityEngine.Random.Range(0,100);
            foreach (var f in possibles[(int)currentSeason]){
                if ( f.Value < x )
                    weather = f.Key;
            }
        }

        return weather;
    }
    public void AdjustPetsTemperatures(){
        foreach (Pet p in GameObject.FindObjectsOfType<Pet>()){
            float insulation = 0f;

            if ( p.equipment[(int)EquipType.costume] != null ){
                insulation += p.equipment[(int)EquipType.costume].insulation;
            }

            float desiredTempDelta = p.currentStats.temperature - (int) currentWeather;
            if ( desiredTempDelta < 0 ){
                // Increases Temp
                desiredTempDelta += insulation;
            } else {
                // Decreases Temp
                desiredTempDelta -= insulation;
            }

            p.currentStats.temperature = (p.currentStats.temperature - desiredTempDelta)*Time.deltaTime;
        }
    }
    private void NextDay(){
        DebugWindow.LogSystem(GetType().Name,System.Reflection.MethodBase.GetCurrentMethod().Name);
    }
}

public enum Weather {
    raining = 40,
    storming = 30,
    sunny = 50,
    cloudy = 60,
    dry = 70,
    windy = 40,
    snowing = 10,
    foggy = 30,
    hot = 100
}

public enum Season {
    autumn = 0,
    winter = 1,
    spring = 2,
    summer = 4
}
