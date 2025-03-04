using UnityEngine;

public class AttackHitbox : MonoBehaviour
{
    public Attack Attack;

    void Start()
    {
        Attack = GetComponentInParent<Attack>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (Attack == null) return;

        if (collision.gameObject.CompareTag("Enemy"))
        {
            EnemyBase enemy = collision.gameObject.GetComponent<EnemyBase>();
            if (enemy != null)
            {
                if (Attack.isNormalAttacking1)
                {
                    enemy.TakeDamage(10); 
                }
                else if (Attack.isNormalAttacking2)
                {
                    enemy.TakeDamage(15); 
                }
                else if (Attack.isNormalAttacking3)
                {
                    enemy.TakeDamage(20); 
                }
            }
        }
    }
}