using Godot;
using System;

public partial class HealthComponent : Node
{
    [Signal]
    public delegate void HealthChangedEventHandler();
    [Signal]
    public delegate void DefeatEventHandler();

    private float _maxHealth;
    public float MaxHealth
    {
        get => _maxHealth;
        set
        {
            _maxHealth = value;
            CurrentHealth = MaxHealth;
        }
    }

    private float _currentHealth;
    public float CurrentHealth
    {
        get => _currentHealth;
        set
        {
            _currentHealth = Math.Max(0, value);
            if (_currentHealth == 0)
            {
                EmitSignal(SignalName.Defeat);
            }
            EmitSignal(SignalName.HealthChanged);
        }
    }

    public void TakeDamage(float damage)
    {
        CurrentHealth -= damage;
    }
}
// 