using UnityEngine;

public class Blob : EnemyBase
{
    protected override void Start()
    {
        base.Start();

        health = 60;
        experiencePoints = 50;
        damage = 30;
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