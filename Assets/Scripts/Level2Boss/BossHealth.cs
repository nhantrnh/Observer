using Unity.VisualScripting;
using UnityEngine;

public class BossHealth : MonoBehaviour
{
    public int health = 50;
    public bool isInvulnerable = false;

    public void TakeDamage(int damage)
    {
        if (isInvulnerable)
            return;

        health -= damage;
        Debug.Log(health);

        if (health == 20)
        {
            GetComponent<Animator>().SetTrigger("Skill");
        }

        if (health <= 0)
        {
            
            GetComponent<Animator>().SetTrigger("Death");
           
        }
    }

    public void resetSkill()
    {
        GetComponent<Animator>().ResetTrigger("Skill");
    }

}
