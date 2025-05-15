using System.Collections.Generic;

namespace Plugins.AhojSystem.Tools.MonoBehaviourTree.CompositeNode {
    public abstract class CompositeNode : Node {
        protected List<Node> Children { get; } = new();
    }
}