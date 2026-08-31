using UnityEngine;
using UnityEngine.Video;

namespace UI.Other
{
    public class TutorialVideoPlayer : MonoBehaviour
    {
        [SerializeField] private VideoPlayer _videoPlayer;

        void OnEnable()
        {
            _videoPlayer.Play();
        }

        void OnDisable()
        {
            _videoPlayer.Stop();
        }
    }
}