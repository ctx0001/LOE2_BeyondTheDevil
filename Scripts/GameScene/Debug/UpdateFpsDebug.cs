using TMPro;
using UnityEngine;

namespace DebugFolder
{
    public class UpdateFpsDebug : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI display;
        
        private int _frameCounter;
        private float _timeCounter;
        private float _lastFrameRate;

        private void Update()
        {
            _frameCounter++;
            _timeCounter += Time.deltaTime;

            if (!(_timeCounter >= 1.0f)) return;
            _lastFrameRate = _frameCounter / _timeCounter;
            display.text = Mathf.Round(_lastFrameRate) + " FPS";
        
            _frameCounter = 0;
            _timeCounter = 0f;
        }
    }
}