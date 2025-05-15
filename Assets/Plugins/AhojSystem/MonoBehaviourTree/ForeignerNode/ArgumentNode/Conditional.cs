using System.Threading;
using Cysharp.Threading.Tasks;

namespace Plugins.AhojSystem.MonoBehaviourTree.ForeignerNode.ArgumentNode {
    public abstract class Conditional {
        protected abstract UniTask<bool> Execute(CancellationToken token);
    }
}