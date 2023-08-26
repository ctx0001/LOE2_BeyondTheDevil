using GameScene.Interactables.SecurityKeyPad;
using UnityEngine;

namespace GameScene.Interactables.Readables
{
    public class VisualizableCombinationVariation : Visualizable
    {
        [SerializeField] private KeyPad _keyPad;
        private bool _firstTimeSawDrawerCombination = true;
        
        protected override void OnClose()
        {
            if (!_firstTimeSawDrawerCombination) return;
            _firstTimeSawDrawerCombination = false;
            StartCoroutine(_keyPad.HintForDrawerCombination());
        }
    }
}