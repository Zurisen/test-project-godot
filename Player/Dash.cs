using Godot;
using System;
using System.Runtime.CompilerServices;

public partial class Dash : Node3D
{
    private Timer _timer;
    private Vector3 _direction = Vector3.Zero;

    [Export]
    private Player _player;

    public override void _Ready()
    {
        base._Ready();
        _timer = GetNode<Timer>("Timer");

    }

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
        if (_direction.IsZeroApprox()) return;

        if (_player.CharacterRig.isDashing())
        {
            _player.Velocity = _direction * _player.Speed * 3f;
        }
    }


    public override void _UnhandledInput(InputEvent @event)
    {
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
