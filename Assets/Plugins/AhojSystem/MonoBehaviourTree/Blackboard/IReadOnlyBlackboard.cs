namespace Plugins.AhojSystem.Tools.MonoBehaviourTree.Blackboard {
    public interface IReadOnlyBlackboard {
        public T Get<T>(string key);
        public bool TryGet<T>(string key, out T value);
        public T GetSingleton<T>() where T : class;
        public bool HasKey(string key);
    }
}