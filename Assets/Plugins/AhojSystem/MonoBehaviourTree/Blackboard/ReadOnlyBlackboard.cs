using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Plugins.AhojSystem.Tools.MonoBehaviourTree.Blackboard {
    public class ReadOnlyBlackboard : IReadOnlyBlackboard {
        private readonly ReadOnlyDictionary<string, object> _blackboard;

        public ReadOnlyBlackboard(Dictionary<string, object> blackboard) {
            _blackboard = new ReadOnlyDictionary<string, object>(blackboard);
        }

        public T Get<T>(string key) {
            if (_blackboard.TryGetValue(key, out var value)) {
                return (T)value;
            }

            throw new KeyNotFoundException($"Key '{key}' not found in the blackboard.");
        }

        public bool TryGet<T>(string key, out T value) {
            if (_blackboard.TryGetValue(key, out var v) && v is T t) {
                value = t;
                return true;
            }

            value = default!;
            return false;
        }

        public T GetSingleton<T>() where T : class {
            var type = typeof(T).Name;
            if (_blackboard.TryGetValue(type, out var value)) {
                return (T)value;
            }

            throw new KeyNotFoundException($"Key '{type}' not found in the blackboard.");
        }

        public bool HasKey(string key) {
            return _blackboard.ContainsKey(key);
        }
    }
}