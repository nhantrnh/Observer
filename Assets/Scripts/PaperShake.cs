using UnityEngine;

public class PaperShake : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public float amplitude = 0.5f; // Biên độ dao động (độ cao lắc lên xuống)
    public float frequency = 2f; // Tần số dao động (tốc độ lắc)    
    public GameObject chatBubble;

    private Vector3 startPos;

    public int id;
    private string paperContent;

    void Start()
    {   
        if (PaperContents.paperContents.Length > id) {
            paperContent = PaperContents.paperContents[id];
        }  
        else {
            paperContent = "";
        }
        startPos = transform.position;
        chatBubble.GetComponent<ChatBubble>().Create(paperContent, gameObject.transform, new Vector3(7200, 1060));

    }

    // Update is called once per frame
    void Update()
    {
        // Tính toán dao động theo trục Y
        float newY = startPos.y + Mathf.Sin(Time.time * frequency) * amplitude;

        // Cập nhật vị trí
        transform.position = new Vector3(startPos.x, newY, startPos.z);
    }

    public string GetPaperContent(){
        return this.paperContent;
    }
}
