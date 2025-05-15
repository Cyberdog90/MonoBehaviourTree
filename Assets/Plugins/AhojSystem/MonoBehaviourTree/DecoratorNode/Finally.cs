using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Plugins.AhojSystem.Tools.MonoBehaviourTree.DecoratorNode {
    public class Finally : DecoratorNode {
        private readonly Node _finally;

        public Finally(Node finallyAction) {
            _finally = finallyAction;
        }

        protected override async UniTask<NodeResult> EvaluateInternal(CancellationToken token) {
            var cts = new CancellationTokenSource();
            token.Register(() => cts.Cancel());

            try {
                var result = await Child.Evaluate(cts);
                try {
                    await _finally.Evaluate(cts);
                }
                catch (Exception e) {
                    Debug.LogException(e);
                }

                return result;
            }
            catch (OperationCanceledException) {
                throw;
            }
            catch (Exception e) {
                Debug.LogException(e);
                return NodeResult.Failure;
            }
        }
    }
}