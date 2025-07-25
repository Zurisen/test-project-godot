using Godot;
using System;

public partial class AttackCast : RayCast3D
{
    public void DealDamage(float damage, float critChance)
    {
        if (!IsColliding()) return;

        var collider = GetCollider();

        if (collider is IDamageable entity)
        {
            bool isCrit = false;
            if (GD.Randf() <= critChance)
            {
                isCrit = true;
                damage *= 2;
            }
            entity.HealthComponent.TakeDamage(damage, isCrit);
        }

        if (collider is CollisionObject3D collisionObject)
        {
            AddException(collisionObject);
        }
    }
}
