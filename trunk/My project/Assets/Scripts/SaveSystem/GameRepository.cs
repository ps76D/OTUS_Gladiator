using System;
using System.Collections.Generic;
using System.IO;
using AesEncrypt;
using Newtonsoft.Json;
using UnityEngine;
using Zenject;

namespace SaveSystem
{
    public class GameRepository : IGameRepository
    {
        [Inject]
        private AesEncryptComponent _encryptComponent;
        
        private const string GAME_STATE_KEY = "GameStateKey"; 
        
        private Dictionary<string, string> _gameState = new ();

        public void SetData<T>(T data)
        {
            var key = typeof(T).ToString();
            
            var jsonData = JsonConvert.SerializeObject(data);
            _gameState[key] = jsonData;
        }

        public bool TryGetData<T>(out T data)
        {
            var key = typeof(T).ToString();

            if (_gameState.TryGetValue(key, out var jsonData))
            {
                data = JsonConvert.DeserializeObject<T>(jsonData);
                return true;
            }
            
            data = default;
            return false;
        }

        public void SaveState()
        {
            var gameStateJson = JsonConvert.SerializeObject(_gameState);
            
            byte[] encryptedData = _encryptComponent.Encrypt(gameStateJson);
            
            EncryptedSaveStruct encryptedSaveStruct = new()
            {
                Data = encryptedData
            };
            
            string encryptedJson = JsonConvert.SerializeObject(encryptedSaveStruct);
            
            var savePath = Path.Combine(Application.persistentDataPath, "save.json");
            
            try
            {
                File.WriteAllText(savePath, contents: encryptedJson);
                Debug.Log(message: "Successfully Saved");
            }
            catch (Exception ex)
            {
                Debug.Log(message: "Save Failed"+ex);
            }
            
            /*PlayerPrefs.SetString(GAME_STATE_KEY, encryptedJson);*/
        }

        public void LoadState()
        {
            /*if (PlayerPrefs.HasKey(GAME_STATE_KEY))
            {
                var encryptedJson = PlayerPrefs.GetString(GAME_STATE_KEY);
                
                byte[] encryptedData = JsonConvert.DeserializeObject<EncryptedSaveStruct>(encryptedJson).Data;
                
                var gameStateJson = _encryptComponent.Decrypt(encryptedData);
                
                _gameState = JsonConvert.DeserializeObject<Dictionary<string, string>>(gameStateJson);
                Debug.Log("Game State Loaded");
            }
            else
            {
                Debug.Log("No Game State Loaded");
            }*/
            
            
            
            
            var savePath = Path.Combine(Application.persistentDataPath, "save.json");
            
            if (!File.Exists(savePath))
            {
                Debug.Log(message: "Save File Not Found");
                return;
            }

            try
            {
                string encryptedJson = File.ReadAllText(savePath);
                
                byte[] encryptedData = JsonConvert.DeserializeObject<EncryptedSaveStruct>(encryptedJson).Data;
                
                var gameStateJson = _encryptComponent.Decrypt(encryptedData);
                
                _gameState = JsonConvert.DeserializeObject<Dictionary<string, string>>(gameStateJson);
            }

            catch (Exception)
            {
                Debug.Log(message: "Save Data Not Read");
            }
        }
    }
}