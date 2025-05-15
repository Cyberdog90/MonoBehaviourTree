using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Plugins.AhojSystem.Tools.Utils.ExtensionMethods;
using UnityEngine;

namespace Plugins.AhojSystem.MonoBehaviourTree.CompositeNode {
    public class RandomSelector : CompositeNode {
        public  RandomSelector Add(Node node) {
            Children.Add(node);
            return this;
        }

        protected override async UniTask<NodeResult> EvaluateInternal(CancellationToken token) {
            var cts = new CancellationTokenSource();
            token.Register(() => cts.Cancel());

            try {
                foreach (var child in Children.Shuffle()) {
                    var state = await child.Evaluate(cts);
                    if (state == NodeResult.Success) return NodeResult.Success;
                }

                return NodeResult.Failure;
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