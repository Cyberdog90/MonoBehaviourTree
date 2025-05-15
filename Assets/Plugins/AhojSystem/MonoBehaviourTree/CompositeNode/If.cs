using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Plugins.AhojSystem.Tools.Utils.ExtensionMethods;
using UnityEngine;

namespace Plugins.AhojSystem.MonoBehaviourTree.CompositeNode {
    public class If : CompositeNode {
        private readonly Func<bool> _ifExpr;
        private readonly List<Func<bool>> _elseIfExprs;

        private Node _thenNode;
        private readonly List<Node> _elseIfNodes;
        private Node _elseNode;

        private bool _isThenSet;
        private bool _isElseSet;

        public If(Func<bool> ifExpr) {
            _ifExpr = ifExpr;
            _elseIfExprs = new List<Func<bool>>();
            _elseIfNodes = new List<Node>();
        }

        public If Then(Node node) {
            if (_isThenSet) {
                Debug.LogError("ElseIf: Then node already set!");
                return this;
            }

            _thenNode = node;
            _isThenSet = true;
            return this;
        }

        public If ElseIf(Func<bool> elseIfExpr, Node node) {
            _elseIfExprs.Add(elseIfExpr);
            _elseIfNodes.Add(node);
            return this;
        }

        public If Else(Node node) {
            if (_isElseSet) {
                Debug.LogError("ElseIf: Else node already set!");
                return this;
            }

            _elseNode = node;
            _isElseSet = true;
            return this;
        }

        protected override async UniTask<NodeResult> EvaluateInternal(CancellationToken token) {
            var cts = new CancellationTokenSource();
            token.Register(() => cts.Cancel());
            try {
                if (_isThenSet.Negate() || _isElseSet.Negate()) {
                    Debug.LogError("IfElse: Node not added!");
                    return NodeResult.Failure;
                }

                if (_ifExpr()) return await _thenNode.Evaluate(cts);
                for (var i = 0; i < _elseIfExprs.Count; i++)
                    if (_elseIfExprs[i]())
                        return await _elseIfNodes[i].Evaluate(cts);
                return await _elseNode.Evaluate(cts);
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