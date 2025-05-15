using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Plugins.AhojSystem.Tools.MonoBehaviourTree {
    public class BehaviourTree {
        private readonly Node _child;
        private CancellationTokenSource _cts;

        public BehaviourTree(Node child) {
            _child = child;
            Application.quitting += OnApplicationQuit;
        }

        public async UniTask Run(CancellationToken token = default) {
            _cts = new CancellationTokenSource();
            token.Register(() => {
                _cts.Cancel();
                Debug.Log("BehaviourTree: Operation Cancelled!");
            });
            try {
                var result = await _child.Evaluate(_cts);
                Debug.Log($"result: {result}");
            }
            finally {
                _cts.Cancel();
                _cts = null;
            }
        }

        private void OnApplicationQuit() {
            Debug.Log("BehaviorTree: OnApplicationQuit");
            _cts?.Cancel();
        }
    }
}