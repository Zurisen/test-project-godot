using Godot;
using System;
using System.Linq;
using System.Reflection;

public partial class Enemy : CharacterBody3D, IDamageable
{
    public HealthComponent HealthComponent { get; set; }

    [Export]
    private float _maxHealth = 20;
    [Export]
    private long _xpValue = 30;

    private Rig _characterRig;
    private CollisionShape3D _collisionShape3D;
    private ShapeCast3D _playerDetector;
    private Random _random;
    private AreaAttack _areaAttack;
    private Player _player;
    private NavigationAgent3D _navigationAgent3D;

    public override void _Ready()
    {
        base._Ready();

        _characterRig = GetNode<Rig>("Rig");
        _random = new Random();
        _collisionShape3D = GetNode<CollisionShape3D>("CollisionShape3D");
        _playerDetector = GetNode<ShapeCast3D>("Rig/PlayerDetector");
        _areaAttack = GetNode<AreaAttack>("Rig/AreaAttack");
        _player = GetTree().GetFirstNodeInGroup("PlayersGroup") as Player;
        _navigationAgent3D = GetNode<NavigationAgent3D>("NavigationAgent3D");

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

        var velocityTarget = Vector3.Zero;
        _navigationAgent3D.TargetPosition = _player.GlobalPosition;

        CheckForAttacks();
        if (!_navigationAgent3D.IsTargetReached())
        {
            velocityTarget = _getLocalNavigationDirection() * 5.0f;
            _orientRig(_navigationAgent3D.GetNextPathPosition());
        }
        _navigationAgent3D.Velocity = velocityTarget;

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

    private Vector3 _getLocalNavigationDirection()
    {
        var destination = _navigationAgent3D.GetNextPathPosition();
        var localDestination = destination - this.GlobalPosition;
        return localDestination.Normalized();
    }

    private void _orientRig(Vector3 targetPosition)
    {
        targetPosition.Y = _characterRig.GlobalPosition.Y;
        if (_characterRig.GlobalPosition.IsEqualApprox(targetPosition)) return;

        _characterRig.LookAt(targetPosition, Vector3.Up, true);
    }



    private void DefeatEvent()
    {
        _characterRig.Travel("Defeat");
        _collisionShape3D.Disabled = true;
        SetPhysicsProcess(false);
        _player.CharacterStats.Xp += _xpValue;

    }

    private void OnRigHeavyAttack()
    {
        _areaAttack.DealDamage(20, 0);
    }


    // Connected Signals
    private void _OnNavigationAgent3dVelocityComputed(Vector3 safeVelocity)
    {
        Velocity = safeVelocity;
        MoveAndSlide();
    }

}
