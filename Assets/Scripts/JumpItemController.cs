using UnityEngine;

public class JumpItemController : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collider2D)
    {
        if (collider2D != null)
        {
            Debug.Log("Hit jump item");
            if (collider2D.gameObject.GetComponent<MainController>() != null)
            {
                collider2D.gameObject.GetComponent<MainController>().GainJumpForce();
                Destroy(gameObject);
            }
        }
    }
}
