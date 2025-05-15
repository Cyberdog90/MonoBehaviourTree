using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Plugins.AhojSystem.Tools.MonoBehaviourTree.TerminalNode {
    public class Log : TerminalNode {
        private readonly string _logText;

        public Log(string logText) {
            _logText = logText;
        }

        protected override async UniTask<NodeResult> EvaluateInternal(CancellationToken token) {
            var cts = new CancellationTokenSource();
            token.Register(() => {
                cts.Cancel();
                Debug.Log("Log: Operation Cancelled!");
            });

            try {
                await UniTask.Yield(cts.Token);
                Debug.Log(_logText);
                return NodeResult.Success;
            }
            catch (OperationCanceledException) {
                Debug.Log("Log: OperationCanceledException");
                throw;
            }
            catch (Exception e) {
                Debug.LogException(e);
                return NodeResult.Failure;
            }
        }
    }
}