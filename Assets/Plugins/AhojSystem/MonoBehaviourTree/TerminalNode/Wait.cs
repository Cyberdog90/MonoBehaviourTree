using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Plugins.AhojSystem.MonoBehaviourTree.TerminalNode {
    public class Wait : TerminalNode {
        private readonly float _waitingTime;

        public Wait(float waitingTime) {
            _waitingTime = waitingTime;
        }

        protected override async UniTask<NodeResult> EvaluateInternal(CancellationToken token) {
            var cts = new CancellationTokenSource();
            token.Register(() => {
                cts.Cancel();
                Debug.Log("Wait: Operation Cancelled!");
            });

            try {
                await UniTask.WaitForSeconds(_waitingTime, cancellationToken: token);
                return NodeResult.Success;
            }
            catch (OperationCanceledException) {
                Debug.Log("Wait: OperationCanceledException");
                throw;
            }
            catch (Exception e) {
                Debug.LogException(e);
                return NodeResult.Failure;
            }
        }
    }
}