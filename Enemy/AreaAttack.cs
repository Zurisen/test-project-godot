using Godot;
using System;

public partial class AreaAttack : ShapeCast3D
{
    public void DealDamage()
    {
        var collisions = GetCollisionCount();
        for (int i = 0; i < collisions; i++)
        {
            var collider = GetCollider(i);
            if (collider is IDamageable entity)
            {
                entity.HealthComponent.TakeDamage(10);
            }
        }
    }
}
