using Godot;
using System;

public partial class Inventory : Control
{
    public Label LevelLabel;
    public Label StrengthLabel;
    public Label AgilityLabel;
    public Label SpeedLabel;
    public Label EnduranceLabel;

    private Player _player;

    public override void _Ready()
    {
        base._Ready();
        LevelLabel = GetNode<Label>("MarginContainer/VBoxContainer/HBoxContainer/VBoxContainer/CharacterSheet/VBoxContainer/LevelLabel");
        StrengthLabel = GetNode<Label>("MarginContainer/VBoxContainer/HBoxContainer/VBoxContainer/CharacterSheet/VBoxContainer/GridContainer/Attribute/VBoxContainer/StrengthLabel");
        AgilityLabel = GetNode<Label>("MarginContainer/VBoxContainer/HBoxContainer/VBoxContainer/CharacterSheet/VBoxContainer/GridContainer/Attribute2/VBoxContainer/AgilityLabel");
        SpeedLabel = GetNode<Label>("MarginContainer/VBoxContainer/HBoxContainer/VBoxContainer/CharacterSheet/VBoxContainer/GridContainer/Attribute3/VBoxContainer/SpeedLabel");
        EnduranceLabel = GetNode<Label>("MarginContainer/VBoxContainer/HBoxContainer/VBoxContainer/CharacterSheet/VBoxContainer/GridContainer/Attribute4/VBoxContainer/EnduranceLabel");
    }

    public void Init(Player player) {
        _player = player;
        UpdateStatsLabels();
    }

    public void UpdateStatsLabels()
    {
        LevelLabel.Text = $"Level {_player.CharacterStats.Level+1}";
        StrengthLabel.Text = ((int)_player.CharacterStats.Strength.Value).ToString();
        AgilityLabel.Text = ((int)(_player.CharacterStats.Agility.Value*100)).ToString();
        SpeedLabel.Text = ((int)_player.CharacterStats.Speed.Value).ToString();
        EnduranceLabel.Text = ((int)_player.CharacterStats.Endurance.Value).ToString();
    }

}
