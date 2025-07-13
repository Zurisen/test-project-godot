using Godot;
using System;
using System.Linq;

public partial class Enemy : CharacterBody3D, IDamageable
{
    public HealthComponent HealthComponent { get; set; }

    [Export]
    private float _maxHealth = 20;

    private Rig _characterRig;
    private CollisionShape3D _collisionShape3D;
    private ShapeCast3D _playerDetector;
    private Random _random;
    private AreaAttack _areaAttack;

    public override void _Ready()
    {
        base._Ready();

        _characterRig = GetNode<Rig>("Rig");
        _random = new Random();
        _collisionShape3D = GetNode<CollisionShape3D>("CollisionShape3D");
        _playerDetector = GetNode<ShapeCast3D>("Rig/PlayerDetector");
        _areaAttack = GetNode<AreaAttack>("Rig/AreaAttack");

        int randomIdx = _random.Next(_characterRig.VillagerMeshInstances.Length);
        _characterRig.SetActiveMesh(_characterRig.VillagerMeshInstances[randomIdx]);
        _characterRig.HeavyAttack += OnRigHeavyAttack;
        HealthComponent = GetNode<HealthComponent>("HealthComponent");
        HealthComponent.MaxHealth = _maxHealth;

        HealthComponent.Defeat += DefeatEvent;


    }

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
        CheckForAttacks();

    }

    private void CheckForAttacks()
    {
        if (_characterRig.isIdle())
        {
            if (!_playerDetector.IsColliding()) return;

            var collisions = _playerDetector.GetCollisionCount();
            for (int i = 0; i < collisions; i++)
            {
                var collider = _playerDetector.GetCollider(i);
                if (collider is Player)
                {
                    _characterRig.Travel("Overhead");
                }
            }

        }

    }



    private void DefeatEvent()
    {
        _characterRig.Travel("Defeat");
        _collisionShape3D.Disabled = true;
        SetPhysicsProcess(false);

    }

    private void OnRigHeavyAttack()
    {
        _areaAttack.DealDamage(20);
    }

}
