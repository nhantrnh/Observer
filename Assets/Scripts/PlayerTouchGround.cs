using UnityEngine;

public class PlayerTouchGround : MonoBehaviour
{
    public GameObject player;
    private MainController mcController;
    void Start(){
        mcController = player.GetComponent<MainController>();
        
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (mcController == null) return;  
        if (collision.gameObject.CompareTag("Ground")){
            mcController.SetIsGrounded(true);
        }
        if (collision.gameObject.CompareTag("HeadHitbox")){
            mcController.SetIsGrounded(true);
            Debug.Log("Hit");
            Transform targetTransform = collision.gameObject.GetComponent<Transform>();
            Destroy(targetTransform.parent.gameObject);
        }
    }
}
