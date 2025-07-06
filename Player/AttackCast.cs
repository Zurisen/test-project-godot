using Godot;
using System;

public partial class AttackCast : RayCast3D
{
    public void DealDamage()
    {
        if (!IsColliding()) return;

        var collider = GetCollider();

        if (collider is Enemy enemy)
        {
            enemy.HealthComponent.TakeDamage(10);
        }

        if (collider is CollisionObject3D collisionObject)
            {
                AddException(collisionObject);
            }
    }
}
