using UnityEngine;

public class HealthPotionController : MonoBehaviour, IDataPersistence
{

    [SerializeField] private string uid;
    [ContextMenu("Generated guid for id")]
    private void GenerateGuid(){
        uid = System.Guid.NewGuid().ToString();
    }
    private bool collected = false;

    private SpriteRenderer visual;
    void Awake(){
        visual = gameObject.GetComponent<SpriteRenderer>();
    }
    public int healthValue = 1;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnTriggerEnter2D(Collider2D collider2D){ 
        if (collider2D != null) {
            Debug.Log("Hit health potion");
            if (collider2D.gameObject.GetComponent<MainController>()!= null) {
                collider2D.gameObject.GetComponent<MainController>().ChangeHealthPoint(healthValue);
                collected = true;
                visual.gameObject.SetActive(false);
            } 
        }
    }

    public void LoadData(GameData data)
    {
        data.paperCollected.TryGetValue(uid, out collected); 
        if (collected){
            if (gameObject != null) {
                visual.gameObject.SetActive(false);
            } 
        }
        else {
            visual.gameObject.SetActive(true);
        }
    }

   
    public void SaveData(ref GameData data)
    {
        if (data.paperCollected.ContainsKey(uid)){
            data.paperCollected.Remove(uid);
        }
        data.paperCollected.Add(uid, collected);
    }
}
