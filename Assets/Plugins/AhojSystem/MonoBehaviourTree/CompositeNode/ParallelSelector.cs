using System;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace Plugins.AhojSystem.MonoBehaviourTree.CompositeNode {
    public class ParallelSelector : CompositeNode {
        public ParallelSelector Add(Node node) {
            Children.Add(node);
            return this;
        }

        protected override async UniTask<NodeResult> EvaluateInternal(CancellationToken token) {
            var cts = new CancellationTokenSource();
            token.Register(() => cts.Cancel());
            var tasks = Children.Select(child => child.Evaluate(cts)).ToList();
            try {
                var results = await UniTask.WhenAll(tasks);
                return results.Any(result => result == NodeResult.Success) ? NodeResult.Success : NodeResult.Failure;
            }
            catch (OperationCanceledException) {
                throw;
            }
            catch {
                return NodeResult.Failure;
            }
        }
    }
}