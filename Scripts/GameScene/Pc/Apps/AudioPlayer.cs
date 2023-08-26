using UnityEngine;
using UnityEngine.UI;

namespace GameScene.Pc.Apps
{
    public class AudioPlayer : MonoBehaviour
    {
        private AudioSource _audioOutput;
        
        [SerializeField] private Image lineImage;
        [SerializeField] private RectTransform lineTransform;
        
        float[] spectrumData = new float[1000];

        private void Update()
        {
            float playbackTime = _audioOutput.time;

            float positionX = (playbackTime / _audioOutput.clip.length) * lineTransform.rect.width;
            Vector3 position = lineTransform.localPosition;
            position.x = positionX;
            lineTransform.localPosition = position;

            _audioOutput.GetSpectrumData(spectrumData, 0, FFTWindow.BlackmanHarris);
        }
    }
}