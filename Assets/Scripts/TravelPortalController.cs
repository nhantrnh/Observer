using UnityEngine;

public class TravelPortalController : MonoBehaviour
{
    public float duration = 1f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {   
        Destroy(gameObject, duration);
    }
}
