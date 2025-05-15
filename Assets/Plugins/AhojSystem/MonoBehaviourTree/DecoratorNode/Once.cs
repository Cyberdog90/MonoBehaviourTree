using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Plugins.AhojSystem.Tools.MonoBehaviourTree.DecoratorNode {
    public class Once : DecoratorNode {
        private readonly OnceMode _onceMode;
        private readonly ReturnValue _returnValue;
        private bool _onceFrag;
        private NodeResult _result;

        public Once(OnceMode onceMode = OnceMode.Execute, ReturnValue returnValue = ReturnValue.Result) {
            _onceMode = onceMode;
            _returnValue = returnValue;
        }

        protected override async UniTask<NodeResult> EvaluateInternal(CancellationToken token) {
            if (_onceFrag) {
                return _returnValue switch {
                    ReturnValue.Success => NodeResult.Success,
                    ReturnValue.Failure => NodeResult.Failure,
                    ReturnValue.Result => _result,
                    _ => NodeResult.Success
                };
            }

            var cts = new CancellationTokenSource();
            token.Register(() => cts.Cancel());
            try {
                switch (_onceMode) {
                    case OnceMode.Execute: {
                        _onceFrag = true;
                        _result = await Child.Evaluate(cts);
                        return _result;
                    }
                    case OnceMode.Success: {
                        var result = await Child.Evaluate(cts);
                        if (result != NodeResult.Success) return NodeResult.Failure;
                        _onceFrag = true;
                        _result = result;
                        return result;
                    }
                    case OnceMode.Failure:
                    default: {
                        var result = await Child.Evaluate(cts);
                        if (result != NodeResult.Failure) return NodeResult.Failure;
                        _onceFrag = true;
                        _result = result;
                        return result;
                    }
                }
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

    public enum OnceMode {
        Execute,
        Success,
        Failure
    }

    public enum ReturnValue {
        Success,
        Failure,
        Result
    }
}