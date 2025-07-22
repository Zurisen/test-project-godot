using Godot;
using System;

public partial class AttackCast : RayCast3D
{
    public void DealDamage(float damage)
    {
        if (!IsColliding()) return;

        var collider = GetCollider();

        if (collider is IDamageable entity)
        {
            entity.HealthComponent.TakeDamage(damage);
        }

        if (collider is CollisionObject3D collisionObject)
        {
            AddException(collisionObject);
        }
    }
}
