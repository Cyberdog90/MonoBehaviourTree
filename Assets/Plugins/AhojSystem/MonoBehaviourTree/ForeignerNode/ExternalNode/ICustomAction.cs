using System.Threading;
using Cysharp.Threading.Tasks;

namespace Plugins.AhojSystem.Tools.MonoBehaviourTree.ForeignerNode.ExternalNode {
    public interface ICustomAction {
        public UniTask<NodeResult> Execute(CancellationToken token);
    }
}
