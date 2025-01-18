
using TMPro;
using UnityEngine;

public class ChatBubble : MonoBehaviour
{
    private SpriteRenderer backgroundSpriteRenderer;
    private TextMeshPro textMeshPro;

    public void Create(string content, Transform parent, Vector3 localPosition){
        Transform transform = Instantiate(gameObject.transform, parent);
        transform.localPosition = localPosition;
        transform.localScale  = new Vector3(50,50,50);

        transform.GetComponent<ChatBubble>().SetText(content);
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        backgroundSpriteRenderer = transform.Find("Background").GetComponent<SpriteRenderer>();
        textMeshPro = transform.Find("PaperContent").GetComponent<TextMeshPro>();
    }

    void Start(){
        SetText(" \"Hãy nhặt những trang sách...\" ");
        // transform.Find("Background").GetComponent<SpriteRenderer>().enabled = false;
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetText(string text){
        textMeshPro.SetText(text);
        textMeshPro.ForceMeshUpdate();
        Vector2 textSize = textMeshPro.GetRenderedValues(false);
        Vector2 padding = new Vector2(-4.5f, 0.5f);
        backgroundSpriteRenderer.size = textSize + padding;
        // transform.Find("Background").GetComponent<SpriteRenderer>().enabled = true;
    }
}
