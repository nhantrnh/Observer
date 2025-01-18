using UnityEngine;

public class VictoryTrigger : MonoBehaviour
{
    public GameObject VictoryUI;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
             VictoryUI.SetActive(true);
        }
    }
}
