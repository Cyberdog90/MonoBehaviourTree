using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Plugins.AhojSystem.Tools.MonoBehaviourTree.DecoratorNode {
    public class Infinite : DecoratorNode {
        private readonly bool _unconditional;

        public Infinite(bool unconditional = false) {
            _unconditional = unconditional;
        }

        protected override async UniTask<NodeResult> EvaluateInternal(CancellationToken token) {
            do {
                try {
                    var cts = TokenIssuer(token);
                    var result = await Child.Evaluate(cts);
                    await UniTask.Yield(cts.Token);
                    cts.Cancel();
                    if (_unconditional) continue;
                    if (result == NodeResult.Failure) return NodeResult.Failure;
                }
                catch (OperationCanceledException) {
                    throw;
                }
                catch (Exception e) {
                    Debug.LogException(e);
                    return NodeResult.Failure;
                }
            } while (true);
        }

        private static CancellationTokenSource TokenIssuer(CancellationToken token) {
            var cts = new CancellationTokenSource();
            token.Register(() => {
                cts.Cancel();
                Debug.Log("Infinite: Operation Cancelled!");
            });

            return cts;
        }
    }
}