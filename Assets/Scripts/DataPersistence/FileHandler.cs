using UnityEngine;
using System.Collections;
using System;
using System.IO;

public class FileHandler
{
    private string dataDirPath = "";
    private string dataFileName = "";
    

    public FileHandler(string path, string name){
        this.dataDirPath  =path;
        this.dataFileName = name;
    }

    public GameData Load(){
        string fullPath = Path.Combine(dataDirPath, dataFileName);

        GameData loaded =null;
        if (File.Exists(fullPath)){
            try {
                string dataToLoad = "";
                using (FileStream stream = new FileStream(fullPath, FileMode.Open)){
                    using (StreamReader reader = new StreamReader(stream)){
                        dataToLoad = reader.ReadToEnd();
                    }
                }

                loaded = JsonUtility.FromJson<GameData>(dataToLoad);   
            } catch (Exception e) {
                Debug.LogError("Save failed somewhere: " + e);
            }
        }
        return loaded;
    }

    public void Save(GameData data){
        string fullPath = Path.Combine(dataDirPath, dataFileName);
    
        try {
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
            string dataToStore = JsonUtility.ToJson(data, true);
            using (FileStream stream = new FileStream(fullPath, FileMode.Create)){
                using (StreamWriter writer = new StreamWriter(stream)){
                    writer.Write(dataToStore);
                }
            }

        }catch(Exception e){    
            Debug.Log("Save failed somewhere: " + e);
        }
    }
}
