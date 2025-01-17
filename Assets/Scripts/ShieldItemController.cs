using UnityEngine;

public class ShieldItemController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnTriggerEnter2D(Collider2D collider)
    {
        MainController controller = collider.GetComponent<MainController>();
        if (controller != null)
        {
            controller.ShieldUp();
            Destroy(gameObject);
        }
    }
}
