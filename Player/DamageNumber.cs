using Godot;
using System;

public partial class DamageNumber : Node3D
{

    private Label3D _label3D;
    public override void _Ready()
    {
        base._Ready();
        _label3D = GetNode<Label3D>("Label3D");
    }

    public void Setup(float damage, Color color, int size, Vector3 positionIn)
    {
         _label3D.Text = ((int)damage).ToString();
        _label3D.FontSize = size;
        _label3D.Modulate = color;
        GlobalPosition = positionIn;

    }
}
