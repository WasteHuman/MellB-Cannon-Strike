using UnityEngine;

namespace Core.Gameplay.Game.TargetSystem
{
    public class TargetSystemController : MonoBehaviour
    {
        [SerializeField] private TargetBallSpawner _spawner;

        public void Initialize()
        {
            _spawner.Init();
        }
    }
}