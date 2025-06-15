using Godot;
using System;
using System.Runtime;

public partial class SmoothCameraArm : SpringArm3D
{
    [Export]
    private Node3D _target;
    [Export]
    private float _decay = 20.0f;
    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
        this.GlobalTransform = GlobalTransform.InterpolateWith(
            _target.GlobalTransform,
            (float)(1 - Mathf.Exp(-_decay * delta))
            );
    }

}
