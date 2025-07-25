using Godot;
using System;

public partial class AreaAttack : ShapeCast3D
{
    public void DealDamage(float damage, float critChance)
    {
        var collisions = GetCollisionCount();
        for (int i = 0; i < collisions; i++)
        {
            var collider = GetCollider(i);
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
        }
    }
}
