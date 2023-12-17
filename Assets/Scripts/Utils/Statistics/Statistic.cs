using System.Collections.Generic;
using CoinPackage.Debugging;
using TMPro;
using UnityEngine;

namespace Utils.Statistics {
    public class Statistic {
        private static readonly CLogger Logger = Loggers.LoggersList[Loggers.LoggerType.UTILS];
        public float Value {
            get => _value;
            set => _value = Mathf.Clamp(value, _min, _max);
        }

        private Dictionary<string, float> _modifiers = new();

        private string _name;
        private float _value;
        private readonly float _min;
        private readonly float _max;

        public Statistic(string name, float defaultValue, float min, float max) {
            _value = defaultValue;
            _min = min;
            _max = max;
        }
        
        public static implicit operator float(Statistic statistic)
        {
            return statistic.Value;
        }

        public string AddModifier(StatOperation operation, float value, string identifier = null) {
            var oldValue = Value;
            switch (operation) {
                case StatOperation.Addition:
                    Value += value;
                    break;
                case StatOperation.Multiplication:
                    Value *= value;
                    break;
            }
            var diff = oldValue - Value;
            var guid = identifier ?? System.Guid.NewGuid().ToString();
            _modifiers.Add(guid, diff);
            return guid;
        }

        public void RemoveModifier(string identifier) {
            if (_modifiers.TryGetValue(identifier, out var value)) {
                Value += value;
                _modifiers.Remove(identifier);
            }
        }
    }
}