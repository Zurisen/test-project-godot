using Godot;
using System;
using System.Runtime.CompilerServices;

public partial class Dash : Node3D
{

    [Export]
    private float _speedMultiplier = 2.2f;
    [Export]
    private Player _player;

    private GpuParticles3D _dashParticles;
    private Timer _timer;
    private Vector3 _direction = Vector3.Zero;


    public override void _Ready()
    {
        base._Ready();
        _dashParticles = GetNode<GpuParticles3D>("GPUParticles3D");
        _dashParticles.Emitting = false;
        _timer = GetNode<Timer>("Timer");

    }

    public override void _PhysicsProcess(double delta)
    {
        if (!_player.IsPhysicsProcessing()) return;

        base._PhysicsProcess(delta);
        if (_direction.IsZeroApprox()) return;

        if (_player.CharacterRig.isDashing())
        {
            _dashParticles.Emitting = true;
            _player.Velocity = _direction * _player.CharacterStats.Speed.Value * _speedMultiplier;
        }
        else
        {
            _dashParticles.Emitting = false;
        }
    }


    public override void _UnhandledInput(InputEvent @event)
    {
        if (!_player.IsPhysicsProcessing()) return;
        
        base._UnhandledInput(@event);

        if (!_timer.IsStopped()) return;

        if (@event.IsActionPressed("dash"))
        {
            _direction = _player.GetMovementDirection();
            // This could be done with signal, but that would require handling the logic of the dash
            // inside the players. Since this is a skill, we would like to separate the logic of the skill
            // from the logic of the player, thats why we import the parent (player) into this class
            _player.CharacterRig.Travel("Dash");

            _timer.Start(1.0);
        }

    }
}
