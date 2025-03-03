using UnityEngine;

public class Enemy1 : EnemyBase
{

    protected override void Start()
    {
        base.Start();

        health = 100;
        experiencePoints = 50;
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