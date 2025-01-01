using UnityEngine;

public class HealthPotionController : MonoBehaviour
{
    public int healthValue = 1;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnTriggerEnter2D(Collider2D collider2D){ 
        if (collider2D != null) {
            Debug.Log("Hit health potion");
            if (collider2D.gameObject.GetComponent<MainController>()!= null) {
                collider2D.gameObject.GetComponent<MainController>().ChangeHealthPoint(healthValue);
                Destroy(gameObject);
            } 
        }
    }
}
