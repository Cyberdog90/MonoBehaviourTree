using System;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace Plugins.AhojSystem.Tools.MonoBehaviourTree.CompositeNode {
    public class Parallel : CompositeNode {
        public Parallel Add(Node node) {
            Children.Add(node);
            return this;
        }

        protected override async UniTask<NodeResult> EvaluateInternal(CancellationToken token) {
            var cts = new CancellationTokenSource();
            token.Register(() => cts.Cancel());
            var tasks = Enumerable.Select(Children, child => child.Evaluate(cts)).ToList();

            try {
                var results = await UniTask.WhenAll(tasks);
                return results.Any(result => result != NodeResult.Success) ? NodeResult.Failure : NodeResult.Success;
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