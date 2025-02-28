using UnityEngine;

public class Enemy2 : EnemyBase
{

    protected override void Start()
    {
        base.Start();

        health = 50;
        experiencePoints = 110;
    }

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
    }

    protected override void Die()
    {
        base.Die();
    }
}