using TMPro;
using UnityEngine;

namespace Common.Localization
{
    public class DetectCharSetToUseDropdownPatch : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI textComponent;
        [SerializeField] private TMP_FontAsset chineseFont;
        [SerializeField] private TMP_FontAsset koreanFont;

        private void Awake()
        {
            if (textComponent.text == "한국어")
            {
                textComponent.font = koreanFont;
                Debug.Log("Is korean");
            }
            else if (textComponent.text == "中文简体" || textComponent.text == "やまと")
            {
                Debug.Log("Is chinise");
                textComponent.font = chineseFont;
            }
        }
    }
}