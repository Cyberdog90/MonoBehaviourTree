using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Plugins.AhojSystem.MonoBehaviourTree.DecoratorNode {
    public class LogResult : DecoratorNode {
        private readonly string _resultText;

        public LogResult(string resultText = "LogResult: ") {
            _resultText = resultText;
        }

        protected override async UniTask<NodeResult> EvaluateInternal(CancellationToken token) {
            var cts = new CancellationTokenSource();
            token.Register(() => cts.Cancel());

            try {
                var result = await Child.Evaluate(cts);
                Debug.Log($"{_resultText}{result}");
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