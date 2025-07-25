using Godot;
using System;

public partial class VfxManager : Node3D
{
    public static VfxManager Instance { get; private set; }
    public readonly PackedScene DamageNumber = ResourceLoader.Load<PackedScene>("res://Player/damage_number.tscn");

    public override void _Ready()
    {
        base._Ready();
        Instance = this;
    }

    public void SpawnDamageNumber(float damage, Color color, int size, Vector3 positionIn)
    {
        DamageNumber newNumber = DamageNumber.Instantiate() as DamageNumber;
        if (newNumber != null)
        {
            AddChild(newNumber);
            newNumber.Setup(damage, color, size, positionIn);
        }
    }
}
