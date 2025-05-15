namespace Plugins.AhojSystem.MonoBehaviourTree.DecoratorNode {
    public abstract class DecoratorNode : Node {
        protected Node Child;

        public DecoratorNode Attach(Node child) {
            Child = child;
            return this;
        }
    }
}