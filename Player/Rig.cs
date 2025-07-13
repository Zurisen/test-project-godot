using Godot;
using System;

public partial class Rig : Node3D
{
    private AnimationTree _animationTree;
    private AnimationNodeStateMachinePlayback _playback;
    private string _runPath;
    private float _runWeightTarget = -1.0f;
    private Skeleton3D _skeleton3D;

    [Export]
    private double _animationBlendDecay = 10;

    [Signal]
    public delegate void HeavyAttackEventHandler();

    public MeshInstance3D[] KnightMeshInstances;
    public MeshInstance3D[] VillagerMeshInstances;

    public override void _Ready()
    {
        base._Ready();

        _animationTree = GetNode<AnimationTree>("AnimationTree");
        _playback = (AnimationNodeStateMachinePlayback)_animationTree.Get("parameters/playback");
        _runPath = "parameters/MoveSpace/blend_position";
        _skeleton3D = GetNode<Skeleton3D>("CharacterRig/GameRig/Skeleton3D");
        
        KnightMeshInstances = [
            GetNode<MeshInstance3D>("CharacterRig/GameRig/Skeleton3D/Knight_01"),
            GetNode<MeshInstance3D>("CharacterRig/GameRig/Skeleton3D/Knight_02")
        ];

        VillagerMeshInstances = [
            GetNode<MeshInstance3D>("CharacterRig/GameRig/Skeleton3D/Villager_01"),
            GetNode<MeshInstance3D>("CharacterRig/GameRig/Skeleton3D/Villager_02")
        ];

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

    public bool isSlashing()
    {
        return _playback.GetCurrentNode() == "Slash";
    }

    public bool isDashing()
    {
        return _playback.GetCurrentNode() == "Dash";
    }

    public bool isDead()
    {
        return _playback.GetCurrentNode() == "Defeat";
    }


    public void SetActiveMesh(MeshInstance3D activeMesh)
    {
        foreach (var childMesh in _skeleton3D.GetChildren())
        {
            if (childMesh is MeshInstance3D mesh)
            {
                mesh.Visible = false;
            }
        }
        activeMesh.Visible = true;
    }

    private void OnAnimationTreeAnimationFinished(string animationName)
    {
        if (animationName == "Overhead")
        {
            EmitSignal(SignalName.HeavyAttack);
        }
    }
    
}
