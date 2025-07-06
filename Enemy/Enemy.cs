using Godot;
using System;
using System.Linq;

public partial class Enemy : CharacterBody3D
{
    public HealthComponent HealthComponent;

    [Export]
    private float _maxHealth = 20;


    private Rig _characterRig;
    private Random _random;
    public override void _Ready()
    {
        base._Ready();

        _characterRig = GetNode<Rig>("Rig");
        _random = new Random();
        int randomIdx = _random.Next(_characterRig.VillagerMeshInstances.Length);
        _characterRig.SetActiveMesh(_characterRig.VillagerMeshInstances[randomIdx]);

        HealthComponent = GetNode<HealthComponent>("HealthComponent");
        HealthComponent.MaxHealth = _maxHealth;

        HealthComponent.Defeat += DefeatEvent;

    }

    private void DefeatEvent()
    {
        _characterRig.Travel("Defeat");

    }

}
