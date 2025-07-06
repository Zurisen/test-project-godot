using Godot;
using System;
using System.Linq;

public partial class Enemy : CharacterBody3D
{

    [Export]
    private float _maxHealth = 20;


    private Rig _characterRig;
    private Random _random;
    private HealthComponent _healthComponent;
    public override void _Ready()
    {
        base._Ready();

        _characterRig = GetNode<Rig>("Rig");
        _random = new Random();
        int randomIdx = _random.Next(_characterRig.VillagerMeshInstances.Length);
        _characterRig.SetActiveMesh(_characterRig.VillagerMeshInstances[randomIdx]);

        _healthComponent = GetNode<HealthComponent>("HealthComponent");
        _healthComponent.MaxHealth = _maxHealth;

    }

}
