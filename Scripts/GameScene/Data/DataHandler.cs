using System;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

namespace GameScene.Data
{
    public class DataHandler : MonoBehaviour
    {

        private bool _loaded;
        private string path;
        
        /// <summary>
        /// Deserialize a JSON file located at the given path into an object of the specified type.
        /// </summary>
        /// <typeparam name="T">The type of object to deserialize the JSON into.</typeparam>
        /// <param name="path">The path to the JSON file.</param>
        /// <returns>The deserialized object of type T.</returns>
        protected static T FetchData<T>(string path)
        {
            try
            {
                var jsonString = "";
                using (var streamReader = File.OpenText(path))
                {
                    while (streamReader.ReadLine() is { } line)
                        jsonString += (line);
                    streamReader.Close();
                }
                
                return JsonConvert.DeserializeObject<T>(jsonString);
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
                return default;
            }
        }

        /// <summary>
        /// Serialize an object of the specified type into a JSON file located at the given path.
        /// </summary>
        /// <typeparam name="T">The type of object to serialize into JSON.</typeparam>
        /// <param name="path">The path to the JSON file.</param>
        /// <param name="data">The object to serialize into JSON and write to the file.</param>
        protected static void UpdateData<T>(string path, T data)
        {
            var json = JsonConvert.SerializeObject(data);
            using var streamWriter = File.CreateText(path);
            streamWriter.WriteLine(json);
            streamWriter.Close();
        }

        protected void SetPath(string fileName)
        {
            path = Application.persistentDataPath + "/" + fileName;
        }
        
        protected string GetPath()
        {
            return path;
        }
        
        public bool IsLoaded() => _loaded;
        protected void MarkLoaded(bool state=true) => _loaded = true;
    }
}