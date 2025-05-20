using System.Collections.Generic;

namespace Plugins.AhojSystem.Tools.MonoBehaviourTree.Blackboard {
    public class Blackboard {
        private readonly Dictionary<string, object> _blackboard = new();

        public void Set<T>(string key, T value) {
            _blackboard[key] = value;
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

        public void SetSingleton<T>(T value) where T : class {
            var type = typeof(T).Name;
            if (!_blackboard.TryAdd(type, value)) {
                throw new InvalidOperationException($"Key '{type}' already exists in the blackboard.");
            }
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

        public void Remove(string key) {
            _blackboard.Remove(key);
        }
        
        public void Clear() {
            _blackboard.Clear();
        }

        public IReadOnlyBlackboard GetReadOnlyBlackboard() {
            return new ReadOnlyBlackboard(_blackboard);
        }
    }
}