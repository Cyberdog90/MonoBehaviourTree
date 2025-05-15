using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Plugins.AhojSystem.MonoBehaviourTree.DecoratorNode {
    public class Repeater : DecoratorNode {
        private readonly uint _repeat;

        public Repeater(uint repeat) {
            _repeat = repeat;
        }

        protected override async UniTask<NodeResult> EvaluateInternal(CancellationToken token) {
            var cts = new CancellationTokenSource();
            token.Register(() => cts.Cancel());

            for (var i = 0; i < _repeat; i++) {
                try {
                    var result = await Child.Evaluate(cts);
                    if (result == NodeResult.Failure) return NodeResult.Failure;
                }
                catch (OperationCanceledException) {
                    throw;
                }
                catch (Exception e) {
                    Debug.LogException(e);
                    return NodeResult.Failure;
                }
            }
            return NodeResult.Success;
        }
    }
}