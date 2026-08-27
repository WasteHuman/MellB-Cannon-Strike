using Core.Data;
using Newtonsoft.Json;
using System;
using System.IO;
using UnityEngine;

namespace Core.Services.SaveSystem
{
    public class SaveService
    {
        private PlayerData _playerData;
        private string SaveFilePath => Path.Combine(Application.persistentDataPath, "player_data.sav");

        public PlayerData PlayerData => _playerData;

        public void Init(bool isDebug = false)
        {
            if(isDebug)
                DeleteAllSaves();

            LoadPlayerData();
        }

        private void LoadPlayerData()
        {
            if (File.Exists(SaveFilePath))
            {
                try
                {
                    string json = File.ReadAllText(SaveFilePath);
                    _playerData = JsonConvert.DeserializeObject<PlayerData>(json);
                    Debug.Log("[SaveService] File loaded successfully.");
                }
                catch
                {
                    CreateNewPlayerData();
                }
            }
            else
                CreateNewPlayerData();
        }

        private void CreateNewPlayerData()
        {
            _playerData = new PlayerData();
            SavePlayerData();
            Debug.Log("[SaveService] New player data created.");
        }

        public void SavePlayerData()
        {
            if (_playerData == null) 
                return;

            try
            {
                string json = JsonConvert.SerializeObject(_playerData);

                string tempPath = SaveFilePath + ".tmp";
                File.WriteAllText(tempPath, json);

                if (File.Exists(SaveFilePath))
                    File.Delete(SaveFilePath);

                File.Move(tempPath, SaveFilePath);

                Debug.Log("[SaveService] File saved successfully.");
            }
            catch (Exception e)
            {
                Debug.LogError($"[SaveService] File save failed: {e.Message}");
            }
        }

        public void DeleteAllSaves()
        {
            if (File.Exists(SaveFilePath))
                File.Delete(SaveFilePath);

            Debug.Log("[SaveService] File deleted successfully.");
        }
    }
}