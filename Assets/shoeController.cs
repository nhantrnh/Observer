using UnityEngine;

public class shoeController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnTriggerEnter2D(Collider2D collider)
    {
        MainController controller = collider.GetComponent<MainController>();  // Sử dụng 'collider' thay vì 'collision'
        if (controller != null)
        {
            controller.CollectionItem();  // Gọi hàm CollectionItem trong MainController
            Destroy(gameObject);  // Xóa item sau khi nhặt
        }
    }
}
