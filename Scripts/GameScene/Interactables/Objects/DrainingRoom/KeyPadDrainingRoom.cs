using System.Collections;
using Assets.Scripts.GameScene.Interactables.Objects.DrainingRoom;
using Assets.Scripts.GameScene.Interactables.Openables.Doors;
using Common.Localization.Speaking;
using GameScene.Data.Handlers;
using Localization;
using TMPro;
using UnityEngine;

namespace GameScene.Interactables.SecurityKeyPad
{
    public class KeyPadDrainingRoom : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI codeText;
        [SerializeField] private JoinedDrainingRoomTrigger joinedDrainingRoomTrigger;
        [SerializeField] private string code = "4672";

        private bool _canInsert = true;
        private string _currentCode = "";
        private bool _opened;
        private static readonly int Open = Animator.StringToHash("Open");

        private void TryCode()
        {
            if (_canInsert)
                StartCoroutine(TryOpen());
        }

        public void AddNumber(string num)
        {
            if (_currentCode.Length < 4)
            {
                _currentCode += num;
                UpdateCode();   
            }

            if(_currentCode.Length == 4)
                TryCode();
        }

        private void UpdateCode()
        {
            codeText.text = _currentCode;
        }
        
        private IEnumerator TryOpen()
        {
            _canInsert = false;
            if (codeText.text == code)
            {
                codeText.text = LocalizationManager.Instance.GetContent("105");
                _opened = true;
            }
            else
            {
                codeText.text = LocalizationManager.Instance.GetContent("106");
            }
            yield return new WaitForSeconds(3f);

            if (!_opened)
            {
                _canInsert = true;
                _currentCode = "";
                codeText.text = "0";   
            }
            else
            {
                Debug.Log("Called");
                StartCoroutine(joinedDrainingRoomTrigger.OnRiddleSolved());
            }
        }
    }
}