using UnityEngine;
using UnityEngine.SceneManagement;

public class BossTrigger : MonoBehaviour
{
    public GameObject boss;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Could use other.GetComponent<Player>() to see if the game object has a Player component
        // Tags work too. Maybe some players have different script components?
        if (other.tag == "Player")
        {
            boss.SetActive(true);
        }
    }
}
