using UnityEngine;

public class JumpItemController : MonoBehaviour, IDataPersistence
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

    void OnTriggerEnter2D(Collider2D collider2D)
    {
        if (collider2D != null)
        {
            Debug.Log("Hit jump item");
            if (collider2D.gameObject.GetComponent<MainController>() != null)
            {
                collider2D.gameObject.GetComponent<MainController>().GainJumpForce();
                collected = true;
                visual.gameObject.SetActive(false);
            }
        }
    }
}
