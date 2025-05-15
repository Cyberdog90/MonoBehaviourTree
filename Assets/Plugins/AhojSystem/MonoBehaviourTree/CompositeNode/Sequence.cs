using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Plugins.AhojSystem.MonoBehaviourTree.CompositeNode {
    public class Sequence : CompositeNode {
        public Sequence Add(Node node) {
            Children.Add(node);
            return this;
        }

        protected override async UniTask<NodeResult> EvaluateInternal(CancellationToken token) {
            var cts = new CancellationTokenSource();
            token.Register(() => {
                cts.Cancel();
                Debug.Log("Sequence: Operation Cancelled!");
            });

            try {
                foreach (var child in Children) {
                    var state = await child.Evaluate(cts);
                    if (state == NodeResult.Failure) return NodeResult.Failure;
                }

                return NodeResult.Success;
            }
            catch (OperationCanceledException) {
                Debug.Log("Sequence: OperationCanceledException");
                throw;
            }
            catch (Exception e) {
                Debug.LogException(e);
                return NodeResult.Failure;
            }
        }
    }
}