using Godot;
using System;

[GlobalClass]
public partial class CharacterStats : Resource
{
    private int _maxLevel = 60;
    private int _level;
    public int Level
    {
        get => _level;
        set => _level = Math.Clamp(value, 0, _maxLevel);
    }

    private long _xp = 0;
    public long Xp
    {
        get => _xp;
        set
        {
            if (_xp + value >= PercentageLvlUpBoundary())
            {
                _xp = 0;
                LevelUp();
            }
            else
            {
                _xp += value;
            }
            GD.Print("XP: ", _xp, "/", PercentageLvlUpBoundary());
        }
    }

    // Damage bonus on attack
    public Stat Strength = new Stat(2, 40);

    // Movement Speed m/s
    public Stat Speed = new Stat(3, 8);

    // HP Bonus per level
    public Stat Endurance = new Stat(5, 25);

    // Crit chance
    public Stat Agility = new Stat(0.05f, 0.25f);
    public class Stat
    {
        private float _minModifier;
        private float _maxModifier;
        private float _value;
        public float Value
        {
            get => _GetModifier();
            set => _value = Math.Clamp(value, 0, 100);
        }

        public Stat(float minModifier, float maxModifier)
        {
            _minModifier = minModifier;
            _maxModifier = maxModifier;
        }

        public void Increase()
        {
            Value = _value + (float)GD.RandRange(2.0, 5.0);
        }

        private float _GetModifier()
        {
            return _PercentageLerp(_minModifier, _maxModifier);
        }

        private float _PercentageLerp(float minBound, float maxBound)
        {
            return Single.Lerp(minBound, maxBound, _value / 100);

        }

    }

    public void LevelUp()
    {
        if (Level >= _maxLevel) return;

        Level += 1;
        Strength.Increase();
        Agility.Increase();
        Endurance.Increase();
        Speed.Increase();

        GD.PrintT(Strength.Value, Agility.Value, Speed.Value, Endurance.Value);
    }

    public long PercentageLvlUpBoundary()
    {
        return (long) (50 * Math.Pow(1.2f, Level));
    }

}
