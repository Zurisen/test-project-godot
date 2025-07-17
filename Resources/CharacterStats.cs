using Godot;
using System;

[GlobalClass]
public partial class CharacterStats : Resource
{
    private int _level;
    public int Level
    {
        get => _level;
        set => _level = Math.Clamp(value, 0, 60);
    }
    public Stat Xp = new Stat(0, 1000);
    public Stat Strength = new Stat(0, 10000);
    public Stat Speed = new Stat(3, 4);
    public Stat Endurance = new Stat(0, 100);
    public Stat Agility = new Stat(0, 10000);
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


        private float _GetModifier()
        {
            return _PercentageLerp(_minModifier, _maxModifier);
        }

        private float _PercentageLerp(float minBound, float maxBound)
        {
            return Single.Lerp(minBound, maxBound, _value / 100);

        }

    }

}
