using UnityEngine;

public class ShieldHandle : MonoBehaviour
{
    public void Destroy()
    {
        gameObject.SetActive(false);
    }
}
