using System.Collections.Generic;
using UnityEngine;

namespace SO.Game
{
    [CreateAssetMenu(menuName = "Game/Target Ball Configs/Target Ball Sprites Config", fileName = "TargetBallSpritesConfig")]
    public class TargetBallSpritesConfig : ScriptableObject
    {
        [field: SerializeField] public List<Sprite> TargetBallSprites { get; private set; } = new();

        public Sprite GetRandomSprite()
        {
            var randomIndex = Random.Range(0, TargetBallSprites.Count);

            var sprite = TargetBallSprites[randomIndex];

            return sprite;
        }
    }
}