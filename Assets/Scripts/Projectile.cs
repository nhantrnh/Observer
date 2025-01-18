using UnityEngine;

public class Projectile : MonoBehaviour
{
    Rigidbody2D rigidbody2d;
    private float lifeTime = 2f;

    // Awake is called when the Projectile GameObject is instantiated
    void Awake()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {

    }

    public void Launch(Vector2 direction, float force)
    {
        rigidbody2d.AddForce(direction * force);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        BossHealth boss = other.GetComponent<BossHealth>();

        if (boss != null)
        {
            boss.TakeDamage(1);
        }
        CheckHitBoss3(other);
        CheckHitBoss1(other);
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
    }

    private void CheckHitBoss3(Collider2D collision)
    {
        MechController controller = collision.GetComponent<MechController>();
        if (controller != null)
        {
            if (controller.currentHp > 0)
            {
                controller.ChangeHealthPoint(-1);
            }

        }
    }

        private void CheckHitBoss1(Collider2D collision)
    {
        Boss01 controller = collision.GetComponent<Boss01>();
        if (controller != null)
        {
            if (controller.health > 0)
            {
                controller.TakeDamage(1);
            }

        }
    }

}
