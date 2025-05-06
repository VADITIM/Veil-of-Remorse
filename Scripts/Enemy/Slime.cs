using UnityEngine;

public class Slime : EnemyBase
{
    protected override void Start()
    {
        base.Start();

        health = 50;
        experiencePoints = 20;
        damage = 20;
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