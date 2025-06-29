using Godot;
using System;

public partial class Rig : Node3D
{
    private AnimationTree _animationTree;
    private AnimationNodeStateMachinePlayback _playback;
    private string _runPath;
    private float _runWeightTarget = -1.0f;

    [Export]
    private double _animationBlendDecay = 10;

    public override void _Ready()
    {
        base._Ready();

        _animationTree = GetNode<AnimationTree>("AnimationTree");
        _playback = (AnimationNodeStateMachinePlayback)_animationTree.Get("parameters/playback");
        _runPath = "parameters/MoveSpace/blend_position";

    }

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);

        var blendPosition = _animationTree.Get(_runPath);
        var blendMovement = Mathf.MoveToward(
            blendPosition.As<float>(),
            _runWeightTarget,
            delta * _animationBlendDecay
        );
        _animationTree.Set(_runPath, blendMovement);
    }

    public void UpdateAnimationTree(Vector3 direction)
    {
        if (direction.IsZeroApprox())
        {
            _runWeightTarget = -1.0f;
        }
        else
        {
            _runWeightTarget = 1.0f;
        }

    }

    public void Travel(string animationName)
    {
        _playback.Travel(animationName);
    }

    public bool isIdle()
    {
        return _playback.GetCurrentNode() == "MoveSpace";
    }
}
