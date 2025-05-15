using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Plugins.AhojSystem.Tools.MonoBehaviourTree.ForeignerNode.ExternalNode;
using UnityEngine;

namespace Plugins.AhojSystem.Tools.MonoBehaviourTree.TerminalNode {
    public class ExternalJoint : TerminalNode {
        private readonly ICustomAction _customAction;

        public ExternalJoint(ICustomAction customAction) {
            _customAction = customAction;
        }

        protected override async UniTask<NodeResult> EvaluateInternal(CancellationToken token) {
            var cts = new CancellationTokenSource();
            token.Register(() => {
                cts.Cancel();
                Debug.Log("External Joint: Operation Cancelled!");
            });

            try {
                return await _customAction.Execute(cts.Token);
            }
            catch (OperationCanceledException) {
                Debug.Log("External Joint: OperationCanceledException");
                throw;
            }
            catch (Exception e) {
                Debug.LogException(e);
                throw;
            }
        }
    }
}