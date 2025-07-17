using System;
using Godot;

public partial class Player : CharacterBody3D, IDamageable
{
	public const float Speed = 5.0f;
	public const float JumpVelocity = 4.5f;
	public const float MouseSensitivity = 0.003f;
	public const double Decay = 8;

	public Rig CharacterRig;
	public HealthComponent HealthComponent { get; set; }

	[Export]
	private float _maxHealth = 40;
	[Export]
	private float _movementAnimationDecay = 10f;
	[Export]
	private float _slashMoveSpeed = 0.3f;
	[Export]
	private float _dashMoveSpeed = 1.6f;
	[ExportCategory("RPG Stats")]

	[Export]
	private CharacterStats _characterStats;

	private Vector2 _look = Vector2.Zero;
	private Node3D _horizontalPivot;
	private Node3D _verticalPivot;
	private Node3D _rigPivot;
	private AttackCast _attackCast;
	private AreaAttack _areaAttack;
	private CollisionShape3D _collisionShape3D;
	private Vector3 _movementDirection = Vector3.Zero;

	public override void _Ready()
	{
		base._Ready();

		_horizontalPivot = GetNode<Node3D>("HorizontalPivot");
		_verticalPivot = GetNode<Node3D>("HorizontalPivot/VerticalPivot");
		_rigPivot = GetNode<Node3D>("RigPivot");
		CharacterRig = GetNode<Rig>("RigPivot/CharacterRig");
		_collisionShape3D = GetNode<CollisionShape3D>("CollisionShape3D");
		_attackCast = GetNode<AttackCast>("RigPivot/CharacterRig/RayAttachment/AttackCast");
		_areaAttack = GetNode<AreaAttack>("RigPivot/CharacterRig/AreaAttack");
		CharacterRig.HeavyAttack += OnRigHeavyAttack;
		Input.MouseMode = Input.MouseModeEnum.Captured;

		HealthComponent = GetNode<HealthComponent>("HealthComponent");
		HealthComponent.MaxHealth = _maxHealth;
		HealthComponent.Defeat += _defeatEvent;

		CharacterRig.SetActiveMesh(CharacterRig.KnightMeshInstances[0]);

		GD.Print("Speed:", _characterStats.Speed.Value);
	}

	public override void _PhysicsProcess(double delta)
	{
		Vector3 velocity = Velocity;
		_handleCameraRotation();


		Vector3 direction = _handleMovement();
		CharacterRig.UpdateAnimationTree(direction);

		_handleIdlePhysicsFrame(delta, velocity, direction);
		_handleSlashingPhysicsFrame(delta);
		_handleAreaAttackPhysicsFrame(delta);
		// Add the gravity.
		if (!IsOnFloor())
		{
			velocity += GetGravity() * (float)delta;
		}
		MoveAndSlide();
	}

	public override void _UnhandledInput(InputEvent @event)
	{
		base._UnhandledInput(@event);
		if (Input.MouseMode == Input.MouseModeEnum.Captured)
		{
			if (@event is InputEventMouseMotion mouseMotion)
			{
				_look = -mouseMotion.Relative * MouseSensitivity;
			}
		}

		if (CharacterRig.isIdle())
		{
			if (@event.IsActionPressed("attack_melee"))
			{
				_slashAttack();
			}
			if (@event.IsActionPressed("attack_heavy"))
			{
				_heavyAttack();
			}
		}
	}

	private void _defeatEvent()
	{
		CharacterRig.Travel("Defeat");
		_collisionShape3D.Disabled = true;
		SetPhysicsProcess(false);

	}

	private void _handleCameraRotation()
	{
		var smoothCameraArm = GetNode<SpringArm3D>("SmoothCameraArm");
		if (smoothCameraArm == null || _horizontalPivot == null || _verticalPivot == null) return;

		_horizontalPivot.RotateY(_look.X);
		_verticalPivot.RotateX(_look.Y);

		float newPitch = (float)Mathf.Clamp(_verticalPivot.Rotation.X, -Math.PI / 2f, Math.PI / 2f);
		_verticalPivot.Rotation = new Vector3(newPitch, _verticalPivot.Rotation.Y, _verticalPivot.Rotation.Z);

		_look = Vector2.Zero;
	}

	private void _lookTowardDirection(Vector3 direction, double delta)
	{
		var targetTrasform = _rigPivot.GlobalTransform.LookingAt(
			_rigPivot.GlobalPosition - direction, Vector3.Up, true
		);

		// _rigPivot.GlobalTransform = new Transform3D(targetTrasform.Basis, _rigPivot.GlobalTransform.Origin);
		_rigPivot.GlobalTransform = _rigPivot.GlobalTransform.InterpolateWith(targetTrasform,
			(float)(1 - Mathf.Exp(-_movementAnimationDecay * delta))
		 );

	}

	private void _slashAttack()
	{
		CharacterRig.Travel("Slash");
		_movementDirection = GetMovementDirection();
		_attackCast.ClearExceptions();
	}

	private void _heavyAttack()
	{
		CharacterRig.Travel("Overhead");
		_movementDirection = GetMovementDirection();
		_attackCast.ClearExceptions();
	}

	private void OnRigHeavyAttack()
	{
		_areaAttack.DealDamage(50);
	}

	private Vector3 _handleMovement()
	{
		Vector2 inputDir = Input.GetVector("move_left", "move_right", "move_forward", "move_back");
		Vector3 direction = (_horizontalPivot.Basis * new Vector3(inputDir.X, 0, inputDir.Y)).Normalized();
		return direction;
	}

	private void _handleSlashingPhysicsFrame(double delta)
	{
		if (!CharacterRig.isSlashing()) return;

		Velocity = _movementDirection * _slashMoveSpeed * Speed;
		_lookTowardDirection(_movementDirection, delta);
		_attackCast.DealDamage();
	}

	private void _handleAreaAttackPhysicsFrame(double delta)
	{
		if (!CharacterRig.isOverhead()) return;

		Velocity = _movementDirection * 0f;
		_lookTowardDirection(_movementDirection, delta);

	}

	private void _handleIdlePhysicsFrame(double delta, Vector3 velocity, Vector3 direction)
	{
		if (!CharacterRig.isIdle() && !CharacterRig.isDashing()) return;

		velocity.X = ExponentialDecay(velocity.X, direction.X * Speed, Decay, delta);
		velocity.Z = ExponentialDecay(velocity.Z, direction.Z * Speed, Decay, delta);

		if (direction != Vector3.Zero)
		{
			_lookTowardDirection(direction, delta);
		}

		Velocity = velocity;
	}

	public Vector3 GetMovementDirection()
	{
		Vector3 velocity = Velocity;
		velocity.Y = 0;

		Vector3 rigFacingForward = CharacterRig.GlobalBasis * new Vector3(0, 0, 1);
		return velocity.Length() > 0.001f ? Velocity.Normalized() : rigFacingForward;
	}

	public float ExponentialDecay(double a, double b, double decay, double delta)
	{
		return (float)(b + (a - b) * Math.Exp(-decay * delta));
	}
}
