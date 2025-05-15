using System.Threading;
using Cysharp.Threading.Tasks;

namespace Plugins.AhojSystem.Tools.MonoBehaviourTree {
    public abstract class Node {
        private CancellationTokenSource _token;

        public async UniTask<NodeResult> Evaluate(CancellationTokenSource cts) {
            _token = cts;
            return await EvaluateInternal(_token.Token);
        }

        public void Cancel() {
            _token.Cancel();
        }

        protected abstract UniTask<NodeResult> EvaluateInternal(CancellationToken token);
    }
}