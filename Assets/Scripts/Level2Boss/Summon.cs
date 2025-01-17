using UnityEngine;

public class Summon : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collider)
    {

        MainController mainController = collider.GetComponent<MainController>();
        if (mainController != null)
        {
            if (mainController.HealthPoint > 0)
            {
                mainController.ChangeHealthPoint(-1);
            }

        }
        
    }
    public void Destroy()
    {
        Destroy(gameObject);
    }
}
