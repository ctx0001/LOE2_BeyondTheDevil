using Localization;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Common.Localization
{
    internal class ReportSceneTranslator : MonoBehaviour
    {
        [Header("UI Elements Report")]
        [SerializeField] private TextMeshProUGUI title;
        [SerializeField] private TextMeshProUGUI description;
        [SerializeField] private Text createReportButtonText;
        [SerializeField] private Text submittingReportText;
        [SerializeField] private Text errorReportText;
        [SerializeField] private Text errorReportFormWindowTitle;
        [SerializeField] private Text errorReportFormSummaryPlaceholder;
        [SerializeField] private Text errorReportFormDescriptionPlaceholder;
        [SerializeField] private Text errorReportFormSubmitButton;
        [SerializeField] private Text errorReportFormCancelButton;
        [SerializeField] private TextMeshProUGUI backToMenuText;

        [SerializeField] private List<Text> allTexts;
        [SerializeField] private List<TextMeshProUGUI> allTexts2;

        [SerializeField] private Font chiniseFont;
        [SerializeField] private Font koreanFont;
        [SerializeField] private Font russianFont;

        [SerializeField] private TMP_FontAsset chiniseFontTMP;
        [SerializeField] private TMP_FontAsset koreanFontTMP;
        [SerializeField] private TMP_FontAsset russianFontTMP;

        private IEnumerator Start()
        {
            while (!LocalizationManager.Instance.IsLoaded())
            {
                yield return null;
            }
            
            switch (LocalizationManager.Instance.GetCurrentLoadedLanguage())
            {
                case "zh" or "jp":
                    ChangeFontAssetForChinaJapan();
                    break;
                case "ko":
                    ChangeFontAssetForKorea();
                    break;
                case "ru":
                    ChangeFontAssetForRussia();
                    break;
            }

            Translate();
        }

        private void Translate()
        {
            title.text = LocalizationManager.Instance.GetContent("246");
            description.text = LocalizationManager.Instance.GetContent("247");

            createReportButtonText.text = LocalizationManager.Instance.GetContent("248");
            submittingReportText.text = LocalizationManager.Instance.GetContent("249");
            errorReportText.text = LocalizationManager.Instance.GetContent("250");

            errorReportFormWindowTitle.text = LocalizationManager.Instance.GetContent("251");
            errorReportFormSummaryPlaceholder.text = LocalizationManager.Instance.GetContent("252");
            errorReportFormDescriptionPlaceholder.text = LocalizationManager.Instance.GetContent("253");
            errorReportFormSubmitButton.text = LocalizationManager.Instance.GetContent("254");
            errorReportFormCancelButton.text = LocalizationManager.Instance.GetContent("255");

            backToMenuText.text = LocalizationManager.Instance.GetContent("16");
        }

        private void ChangeFontAssetForChinaJapan()
        {
            Debug.Log("All texts: " + allTexts.Count);
            foreach (var textComponent in allTexts)
            {
                textComponent.font = chiniseFont;
            }

            foreach (var textComponent in allTexts2)
            {
                textComponent.font = chiniseFontTMP;
            }
        }

        private void ChangeFontAssetForKorea()
        {
            foreach (var textComponent in allTexts)
            {
                textComponent.font = koreanFont;
            }

            foreach (var textComponent in allTexts2)
            {
                textComponent.font = koreanFontTMP;
            }
        }

        private void ChangeFontAssetForRussia()
        {
            foreach (var textComponent in allTexts)
            {
                textComponent.font = russianFont;
            }

            foreach (var textComponent in allTexts2)
            {
                textComponent.font = russianFontTMP;
            }
        }
    }
}
