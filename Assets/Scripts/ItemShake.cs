using UnityEngine;

public class ItemShake : MonoBehaviour
{
    public float amplitude = 0.5f; // Biên độ dao động (độ cao lắc lên xuống)
    public float frequency = 2f; // Tần số dao động (tốc độ lắc)
    private Vector3 startPos;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // Tính toán dao động theo trục Y
        float newY = startPos.y + Mathf.Sin(Time.time * frequency) * amplitude;

        // Cập nhật vị trí
        transform.position = new Vector3(startPos.x, newY, startPos.z);
    }

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
