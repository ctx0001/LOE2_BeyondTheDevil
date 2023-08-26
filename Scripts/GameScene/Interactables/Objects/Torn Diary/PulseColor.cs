using System;
using UnityEngine;

namespace GameScene.Interactables.Objects.Torn_Diary
{
    public class PulseColor : MonoBehaviour
    {
        public float fadeDuration = 1f;
        private float _lastColorChangeTime;

        private readonly Color _color1 = Color.grey;
        private readonly Color _color2 = Color.white;

        private Color _startColor;
        private Color _endColor;
        private Material _material;

        private void Start()
        {
            _material = GetComponent<Renderer>().material;
            _startColor = _color1;
            _endColor = _color2;
        }

        private void PulseColorEffect()
        {
            var ratio = (Time.time - _lastColorChangeTime) / fadeDuration;
            ratio = Mathf.Clamp01(ratio);
            _material.color = Color.Lerp(_startColor, _endColor, Mathf.Sqrt(ratio)); 

            if (Math.Abs(ratio - 1f) > 0.1f) return;
            _lastColorChangeTime = Time.time;

            (_startColor, _endColor) = (_endColor, _startColor);
        }
    
        private void Update()
        {
            PulseColorEffect();
        }
    }
}