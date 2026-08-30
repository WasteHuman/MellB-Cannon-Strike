using System.Collections.Generic;
using Core.Data;
using UnityEngine;

namespace Core.SO
{
    [CreateAssetMenu(menuName = "Game/Shop/Shop Entities Config", fileName = "ShopEntitiesConfig")]
    public class ShopEntitiesConfig : ScriptableObject
    {
        [field: SerializeField] public List<ShopEntityData> PlayerSkinsData { get; private set; } = new();
        [field: SerializeField] public List<ShopEntityData> PlayerBallSkinsData { get; private set; } = new();
        [field: SerializeField] public List<ShopEntityData> PlayerUpgradesData { get; private set; } = new();

        public Sprite GetPlayerSkinById(string skinId)
        {
            if(PlayerSkinsData.Count == 0)
            {
                Debug.LogWarning($"[Shop Entities] Player Skin Datas is empty!");
                return null;
            }

            var skin = PlayerSkinsData.Find(skin => skin.EntityID == skinId);

            if(skin == null)
            {
                Debug.LogWarning($"[Shop Entities] Skind id [{skinId}] not found! Returned default skin.");
                return PlayerSkinsData[0].EntityItemSprite;
            }

            var skinSprite = skin.EntitySkinSprite;

            return skinSprite;
        }

        public Sprite GetPlayerBallSkinById(string skinId)
        {
            if(PlayerBallSkinsData.Count == 0)
            {
                Debug.LogWarning($"[Shop Entities] Player Skin Datas is empty!");
                return null;
            }

            var skin = PlayerBallSkinsData.Find(skin => skin.EntityID == skinId);

            if(skin == null)
            {
                Debug.LogWarning($"[Shop Entities] Skind id [{skinId}] not found! Returned default skin.");
                return PlayerBallSkinsData[0].EntityItemSprite;
            }

            var skinSprite = skin.EntityItemSprite;

            return skinSprite;
        }
    }
}