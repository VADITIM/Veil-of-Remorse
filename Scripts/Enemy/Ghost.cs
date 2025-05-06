using UnityEngine;

public class Ghost : EnemyBase
{
    protected override void Start()
    {
        base.Start();

        health = 120;
        experiencePoints = 140;
        damage = 60;
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