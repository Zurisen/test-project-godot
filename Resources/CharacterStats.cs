using Godot;
using System;

public partial class CharacterStats : Resource
{
    public class Stat<T> where T: IComparable<T>
    {
        T _minValue;
        T _maxValue;
        private T _score;
        public T Score
        {
            get => _score;
            set => _score = Clamp(value, _minValue, _maxValue);
        }
        public Stat(T minValue, T maxValue)
        {
            _minValue = minValue;
            _maxValue = maxValue;
        }

        private T Clamp(T value, T min, T max)
        {
            if (value.CompareTo(min) < 0) return min;
            if (value.CompareTo(max) > 0) return max;
            return value;
        }
    }

    public Stat<int> Level = new Stat<int>(0, 60);
    public Stat<double> Xp = new Stat<double>(0, 1000);
    public Stat<double> Strength = new Stat<double>(0, 10000);
    public Stat<double> Speed = new Stat<double>(0, 100);
    public Stat<double> Endurance = new Stat<double>(0, 100);
    public Stat<double> Agility = new Stat<double>(0, 10000);
}
