using UnityEngine;

public class PlayerHurtbox : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (ClassManager.Instance.Attack == null) return;

        EnemyBase enemy = collision.gameObject.GetComponent<EnemyBase>();
        if (enemy != null)
        {
            PolygonCollider2D bodyCollider = enemy.GetComponent<PolygonCollider2D>();
            if (collision != bodyCollider) return; 

            if (ClassManager.Instance.Attack.isNormalAttacking1)
            {
                enemy.TakeDamage(10);
            }
            else if (ClassManager.Instance.Attack.isNormalAttacking2)
            {
                enemy.TakeDamage(15);
            }
            else if (ClassManager.Instance.Attack.isNormalAttacking3)
            {
                enemy.TakeDamage(20);
            }

            if (ClassManager.Instance.Attack.isHeavyAttacking)
            {
                enemy.TakeDamage((int)ClassManager.Instance.Attack.GetHeavyAttackDamage());
            }
        }
    }
}