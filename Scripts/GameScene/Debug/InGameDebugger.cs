using System;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DebugFolder
{
    public class InGameDebugger : MonoBehaviour
    {
        public static InGameDebugger Instance;
        [SerializeField] private GameObject template;
        [SerializeField] private Transform logSpace;
        [SerializeField] private ScrollRect scrollrect;
        [SerializeField] private GameObject scrollView;
        
        private static readonly object _lock = new object();
        private static BufferedStream _bufferedStream;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
            }
            else
            {
                Instance = this;
                GenerateLogFile();
            }
        }

        private void GenerateLogFile()
        {
            string path;
            
            #if UNITY_EDITOR
                path = Application.dataPath + "/Logs/";
            #else
                path = Application.persistentDataPath + "/Logs/";
            #endif
            
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            
            var fileName = "logfile-" + DateTime.Now.ToString("yyyy-dd-M--HH-mm-ss") + ".txt";
            PlayerPrefs.SetString("SessionLogPath", path + fileName);
            
            using var sw = File.CreateText(path + fileName);
            Debug.Log(path + fileName);
            sw.WriteLine("Started log file");
            sw.Close();
        }

        private void Update()
        {
            if (!Input.GetKeyDown(KeyCode.H)) return;

            if (scrollView.gameObject.activeSelf)
            {
                scrollView.SetActive(false);
            }
            else
            {
                scrollView.SetActive(true);
                // scrollrect.normalizedPosition = new Vector2(0, 0);
            }
        }

        public void Log(string text)
        {
            var createdLog = Instantiate(template, transform.position, Quaternion.identity);
            createdLog.transform.SetParent(logSpace);
            createdLog.transform.SetAsFirstSibling();
            createdLog.transform.localScale = new Vector3(1, 1, 1);
            
            var logText = createdLog.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            logText.text = $"[{DateTime.Now:HH:mm:ss}] {text}";
            FileLogLine($"[{DateTime.Now:HH:mm:ss}] {text}");
            logText.color = Color.white;
            
            // scrollrect.normalizedPosition = new Vector2(0, 1);
        }
        
        public void LogSuccess(string text)
        {
            var createdLog = Instantiate(template, transform.position, Quaternion.identity);
            createdLog.transform.SetParent(logSpace);
            createdLog.transform.SetAsFirstSibling();
            createdLog.transform.localScale = new Vector3(1, 1, 1);
            
            var logText = createdLog.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            logText.text = $"[{DateTime.Now:HH:mm:ss}] {text}";
            FileLogLine($"[{DateTime.Now:HH:mm:ss}] {text}");
            logText.color = Color.green;
            
            // scrollrect.normalizedPosition = new Vector2(0, 1);
        }

        public void LogWarning(string text)
        {
            var createdLog = Instantiate(template, transform.position, Quaternion.identity);
            createdLog.transform.SetParent(logSpace);
            createdLog.transform.SetAsFirstSibling();
            createdLog.transform.localScale = new Vector3(1, 1, 1);

            var logText = createdLog.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            logText.text = $"[{DateTime.Now:HH:mm:ss}] {text}";
            FileLogLine($"[{DateTime.Now:HH:mm:ss}] {text}");
            logText.color = Color.yellow;   
            
            // scrollrect.normalizedPosition = new Vector2(0, 1);
        }

        public void LogError(string text)
        {
            var createdLog = Instantiate(template, transform.position, Quaternion.identity);
            createdLog.transform.SetParent(logSpace);
            createdLog.transform.SetAsFirstSibling();
            createdLog.transform.localScale = new Vector3(1, 1, 1);
            
            var logText = createdLog.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            logText.text = $"[{DateTime.Now:HH:mm:ss}] {text}";
            FileLogLine($"[{DateTime.Now:HH:mm:ss}] {text}");
            logText.color = Color.red;
            
            // scrollrect.normalizedPosition = new Vector2(0, 1);
        }

        
        private void FileLogLine(string text)
        {
            using var sw = File.AppendText(PlayerPrefs.GetString("SessionLogPath"));
            sw.WriteLine(text);
            sw.Close();
        }
        
        
    }
}