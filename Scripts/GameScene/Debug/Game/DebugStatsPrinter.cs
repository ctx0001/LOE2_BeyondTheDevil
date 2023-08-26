using TMPro;
using UnityEngine;

namespace DebugFolder.Game
{
    public class DebugStatsPrinter : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI debugFps;
        [SerializeField] private TextMeshProUGUI debugQualityMode;
        [SerializeField] private TextMeshProUGUI debugLanguage;

        // Variables for fps debugging
        private int _frameCounter;
        private float _timeCounter;
        private float _lastFrameRate;

        private void Start()
        {
            var qualityLevel = QualitySettings.GetQualityLevel();
            debugQualityMode.text = "Graphic Mode: " + QualitySettings.names[qualityLevel];

            debugLanguage.text = "Language: " + PlayerPrefs.GetString("Language");
        }
        
        private void ShowFps()
        {
            _frameCounter++;
            _timeCounter += Time.deltaTime;

            if (!(_timeCounter >= 1.0f)) return;
            _lastFrameRate = _frameCounter / _timeCounter;
            debugFps.text = Mathf.Round(_lastFrameRate) + " FPS";
        
            _frameCounter = 0;
            _timeCounter = 0f;
        }

        private void Update()
        {
            ShowFps();
        }
    }
}