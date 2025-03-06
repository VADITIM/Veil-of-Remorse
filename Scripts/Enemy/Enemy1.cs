using UnityEngine;

public class Enemy1 : EnemyBase
{

    protected override void Start()
    {
        base.Start();

        health = 10;
        experiencePoints = 5000;
        damage = 10;
    }

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
    }

    public override void Die()
    {
        base.Die();
    }
}