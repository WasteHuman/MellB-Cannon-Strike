using System.Threading;
using UnityEngine;

namespace Core.Common
{
    public abstract class BaseSceneEntryPoint : MonoBehaviour
    {
        [SerializeField] protected SceneController _controller;

        private CancellationTokenSource _cancellationTokenSource;

        private void Start()
        {
            _cancellationTokenSource = new();

            _controller.Enter();
            _controller.Initialize();
        }

        private void OnDestroy()
        {
            _controller.Exit();

            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource?.Dispose();
            _cancellationTokenSource = null;
        }
    }
}