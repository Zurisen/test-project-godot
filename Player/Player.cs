using System;
using Godot;

public partial class Player : CharacterBody3D
{
	public const float Speed = 5.0f;
	public const float JumpVelocity = 4.5f;

	public float MouseSensitivity = 0.003f;

	private Vector2 _look = Vector2.Zero;
	private Node3D _horizontalPivot;
	private Node3D _verticalPivot;

	public override void _Ready()
	{
		base._Ready();

		_horizontalPivot = GetNode<Node3D>("HorizontalPivot");
		_verticalPivot = GetNode<Node3D>("HorizontalPivot/VerticalPivot");
		Input.MouseMode = Input.MouseModeEnum.Captured;
	}

	public override void _PhysicsProcess(double delta)
	{
		Vector3 velocity = Velocity;
		_handleCameraRotation();

		// Add the gravity.
		if (!IsOnFloor())
		{
			velocity += GetGravity() * (float)delta;
		}

		// Handle Jump.
		if (Input.IsActionJustPressed("ui_accept") && IsOnFloor())
		{
			velocity.Y = JumpVelocity;
		}

		Vector3 direction = _handleMovement();
		if (direction != Vector3.Zero)
		{
			velocity.X = direction.X * Speed;
			velocity.Z = direction.Z * Speed;
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
			velocity.Z = Mathf.MoveToward(Velocity.Z, 0, Speed);
		}

		Velocity = velocity;
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

	private Vector3 _handleMovement()
	{
		Vector2 inputDir = Input.GetVector("move_left", "move_right", "move_forward", "move_back");
		Vector3 direction = (_horizontalPivot.Basis * new Vector3(inputDir.X, 0, inputDir.Y)).Normalized();
		return direction;
	}
}
