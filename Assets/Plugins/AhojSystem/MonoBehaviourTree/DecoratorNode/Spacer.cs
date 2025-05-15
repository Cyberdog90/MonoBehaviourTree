using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Plugins.AhojSystem.MonoBehaviourTree.DecoratorNode {
    public class Spacer : DecoratorNode {
        protected override async UniTask<NodeResult> EvaluateInternal(CancellationToken token) {
            var cts = new CancellationTokenSource();
            token.Register(() => cts.Cancel());

            try {
                return await Child.Evaluate(cts);
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