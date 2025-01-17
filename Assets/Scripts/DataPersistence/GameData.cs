using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public int levelIndex; 
    public int healthPoint;
    public Vector3 playerPosition;
    public SerializableDictionary<string,  bool> paperCollected;


    public GameData(){
        levelIndex = 1; //Level 0
        healthPoint = 3;
        playerPosition = Vector3.zero;
        paperCollected = new SerializableDictionary<string, bool>();
    }

    public void SetSceneIndex(int index){
        this.levelIndex = index;
    }
}
