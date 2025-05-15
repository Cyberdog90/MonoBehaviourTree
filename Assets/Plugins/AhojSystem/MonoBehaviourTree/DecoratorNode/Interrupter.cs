using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Plugins.AhojSystem.MonoBehaviourTree.ForeignerNode.ExternalNode;
using UnityEngine;

namespace Plugins.AhojSystem.MonoBehaviourTree.DecoratorNode {
    public class Interrupter : DecoratorNode {
        private readonly ICustomAction _interrupter;

        public Interrupter(ICustomAction interrupter) {
            _interrupter = interrupter;
        }

        protected override async UniTask<NodeResult> EvaluateInternal(CancellationToken token) {
            var childCts = new CancellationTokenSource();
            var interrupterCts = new CancellationTokenSource();

            token.Register(() => {
                childCts.Cancel();
                interrupterCts.Cancel();
                Debug.Log("Interrupter: Operation Cancelled!");
            });

            try {
                var childTask = Child.Evaluate(childCts);
                var interrupterTask = _interrupter.Execute(interrupterCts.Token);
                var (winArgumentIndex, childResult, _) = await UniTask.WhenAny(childTask, interrupterTask);

                if (winArgumentIndex == 0) {
                    // 子ノードが先に完了した場合
                    interrupterCts.Cancel();
                    Debug.Log("子ノードが先に完了しました");
                    return childResult;
                }

                // _interrupterタスクが先に完了した場合
                childCts.Cancel();
                Debug.Log("Interrupterタスクが先に完了しました");
                return NodeResult.Success;
            }
            catch (OperationCanceledException) {
                Debug.Log("Interrupter: OperationCanceledException");
                return NodeResult.Success;
            }
            catch (Exception e) {
                Debug.LogException(e);
                return NodeResult.Failure;
            }
        }
    }
}