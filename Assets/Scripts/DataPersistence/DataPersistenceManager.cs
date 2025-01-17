using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;
using UnityEngine.Rendering;

public class DataPersistenceManager: MonoBehaviour {

    [Header("File Storage Config")]
    [SerializeField] private string fileName;


    public static DataPersistenceManager instance {get; private set;}

    private GameData gameData;
    private FileHandler fileHandler;
    private List<IDataPersistence> dataPersistences;


    private void Awake(){
        if (instance !=null) {
            Debug.Log("Found a second one, delete the newest one.");
            Destroy(this.gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(this.gameObject);
        this.fileHandler = new FileHandler(Application.persistentDataPath, fileName);
    }

    private void OnEnable(){
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable(){
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode){
        this.dataPersistences = FindAllDataPersistenceObjects();
        
    }

    public void NewGame(){
        this.gameData = new GameData();
        Debug.Log("Game data created");
        SceneManager.LoadSceneAsync(1); //Level 0;
    }
    

    public void LoadGame(){
        this.gameData = fileHandler.Load();
        if (this.gameData == null) {
            Debug.Log("Somehow null-ed");
            NewGame();
        }
        else {
            Debug.Log("Not null");
        }
        if (gameData.levelIndex != SceneManager.GetActiveScene().buildIndex) {
            //Load correct scene
            SceneManager.LoadSceneAsync(gameData.levelIndex);
        }
        foreach(IDataPersistence data in dataPersistences){
            data.LoadData(gameData);
        }
    }

    public void SaveGame(){
        if (dataPersistences == null || !dataPersistences.Any())
        {
            this.dataPersistences = FindAllDataPersistenceObjects();
        }

        gameData.SetSceneIndex(SceneManager.GetActiveScene().buildIndex);
        
        foreach(IDataPersistence data in dataPersistences){
            data.SaveData(ref gameData);
        }

        fileHandler.Save(gameData);
    }

    private List<IDataPersistence> FindAllDataPersistenceObjects(){
        IEnumerable<IDataPersistence> dataPersistences = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None).OfType<IDataPersistence>();
        return new List<IDataPersistence>(dataPersistences);
    }


}