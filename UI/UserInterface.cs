using Godot;
using System;

public partial class UserInterface : Control
{
    [Export]
    private Player _player;

    private Label _levelLabel;
    private TextureProgressBar _healthBar;
    private Label _healthLabel;

    public override void _Ready()
    {
        base._Ready();
        _levelLabel = GetNode<Label>("MarginContainer/InfoBar/HealthBarOver/LevelLabel");
        _healthBar = GetNode<TextureProgressBar>("MarginContainer/InfoBar/HealtBarUnder/HealthBar");
        _healthLabel = GetNode<Label>("MarginContainer/InfoBar/HealtBarUnder/HealthBar/HealthLabel");
    }

    public void Init()
    {
        UpdateLevelLabel();
        UpdateHealthBar();
    }
    public void UpdateHealthBar()
    {
        GD.Print("Triggered: ", _player.HealthComponent.CurrentHealth);
        _healthBar.MaxValue = _player.HealthComponent.MaxHealth;
        _healthBar.Value = _player.HealthComponent.CurrentHealth;

        _healthLabel.Text = $"{_healthBar.Value} / {_healthBar.MaxValue}";
        
    }

    public void UpdateLevelLabel()
    {
        _levelLabel.Text = (_player.CharacterStats.Level+1).ToString();
    }

}
