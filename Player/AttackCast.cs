using Godot;
using System;

public partial class AttackCast : RayCast3D
{
    public void DealDamage()
    {
        if (!IsColliding()) return;

        var collider = GetCollider();
        GD.Print($"Damage Dealt! to {collider}");
        if (collider is CollisionObject3D collisionObject)
        {
            AddException(collisionObject);
        }
    }
}
