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

        if (Attack.isNormalAttacking1)
        {
            if (collision.gameObject.GetComponent<Enemy>() != null)
                Debug.Log("Hit enemy with normal attack 1");
        }
        else if (Attack.isNormalAttacking2)
        {
            if (collision.gameObject.GetComponent<Enemy>() != null)
                Debug.Log("Hit enemy with normal attack 2");
        }
        else if (Attack.isNormalAttacking3)
        {
            if (collision.gameObject.GetComponent<Enemy>() != null)
                Debug.Log("Hit enemy with normal attack 3");
        }
    }
}