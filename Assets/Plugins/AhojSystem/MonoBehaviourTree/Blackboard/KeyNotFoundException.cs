namespace Plugins.AhojSystem.Tools.MonoBehaviourTree.Blackboard {
    public class KeyNotFoundException : System.Exception {
        public KeyNotFoundException(string message) : base(message) { }
        public KeyNotFoundException(string message, System.Exception innerException) : base(message, innerException) { }
    }
}