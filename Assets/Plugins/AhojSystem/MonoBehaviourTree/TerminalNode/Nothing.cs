using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Plugins.AhojSystem.Tools.MonoBehaviourTree.TerminalNode {
    public class Nothing : TerminalNode {
        protected override async UniTask<NodeResult> EvaluateInternal(CancellationToken token) {
            try {
                await UniTask.Yield(token);
                return NodeResult.Success;
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