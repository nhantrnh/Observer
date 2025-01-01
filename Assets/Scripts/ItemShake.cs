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
}
