using UnityEngine;

public class SpikeController : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D collider){

        MainController mainController = collider.GetComponent<MainController>();
        if (mainController != null){
            if (mainController.HealthPoint > 0) {
                mainController.ChangeHealthPoint(-1);
            }
            
        }
    }
}
