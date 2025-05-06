using UnityEngine;

public class Zombie : EnemyBase
{
    protected override void Start()
    {
        base.Start();

        health = 100;
        experiencePoints = 90;
        damage = 40;
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