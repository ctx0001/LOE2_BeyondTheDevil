using System;
using System.Collections;
using System.Collections.Generic;
using Localization;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace Interactable.Utility
{
    public class InteractableTranslator : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI amuletLetter;
        [SerializeField] private TextMeshProUGUI bookExplainPage1;
        [SerializeField] private TextMeshProUGUI bookExplainPage2;

        [Header("MazeElectronicGame Canvas")] 
        [SerializeField] private TextMeshProUGUI mazeTitleText;
        [SerializeField] private TextMeshProUGUI mazeHintText;
        [SerializeField] private TextMeshProUGUI mazeSuccessText;

        [Header("CipherBreakerGame Canvas")] 
        [SerializeField] private TextMeshProUGUI cipherTitleText;
        [SerializeField] private TextMeshProUGUI cipherHintText;
        [SerializeField] private TextMeshProUGUI cipherSuccessText;
        [SerializeField] private TextMeshProUGUI cipherErrorText;
        [SerializeField] private TextMeshProUGUI cipherFailedText;
        [SerializeField] private TextMeshProUGUI cipherRetryErrorText;
        [SerializeField] private TextMeshProUGUI cipherRetryFailedText;

        [Header("Diary Pages")] 
        [SerializeField] private TextMeshProUGUI page1;
        [SerializeField] private TextMeshProUGUI page2;
        [SerializeField] private TextMeshProUGUI page3;
        [SerializeField] private TextMeshProUGUI page4;
        [SerializeField] private TextMeshProUGUI page5;
        [SerializeField] private TextMeshProUGUI page6;
        [SerializeField] private TextMeshProUGUI foundPageText1;
        [SerializeField] private TextMeshProUGUI foundPageText2;
        
        [Header("Last Doctor Letter")]
        [SerializeField] private TextMeshProUGUI page1LetterDoctor;
        [SerializeField] private TextMeshProUGUI page2LetterDoctor;
        [SerializeField] private TextMeshProUGUI page3LetterDoctor;
        [SerializeField] private TextMeshProUGUI page4LetterDoctor;
        [SerializeField] private TextMeshProUGUI page5LetterDoctor;
        
        [Header("MiniGames Common Texts")] 
        [SerializeField] private List<TextMeshProUGUI> exitText;

        [Header("HintIncenseAndCandles")] 
        [SerializeField] private TextMeshProUGUI moreSpecificCandleHint;
        [SerializeField] private TextMeshProUGUI moreSpecificIncenseBurnerHint;

        [Header("Tutorial WIndow")]
        [SerializeField] private TextMeshProUGUI tutorialWindowTextHint;
        [SerializeField] private TextMeshProUGUI tutorialWindowContinueText;

        [Header("Sewers")]
        [SerializeField] private TextMeshProUGUI leverCombinationHint;
        [SerializeField] private TextMeshProUGUI journalTitle;
        [SerializeField] private TextMeshProUGUI journalDescription;

        private IEnumerator WaitForDataLoad()
        {
            while (!LocalizationManager.Instance.IsLoaded())
            {
                yield return null;
            }
            
            Translate();
        }

        private void Start()
        {
            StartCoroutine(WaitForDataLoad());
        }

        private void Translate()
        {
            // Letters
            amuletLetter.text = LocalizationManager.Instance.GetContent("50");
            bookExplainPage1.text = LocalizationManager.Instance.GetContent("51");
            bookExplainPage2.text = LocalizationManager.Instance.GetContent("52");
            
            // Diary Pages
            page1.text = LocalizationManager.Instance.GetContent("155");
            page2.text = LocalizationManager.Instance.GetContent("156");
            page3.text = LocalizationManager.Instance.GetContent("157");
            page4.text = LocalizationManager.Instance.GetContent("158");
            page5.text = LocalizationManager.Instance.GetContent("159");
            page6.text = LocalizationManager.Instance.GetContent("160");
            foundPageText1.text = LocalizationManager.Instance.GetContent("169");
            foundPageText2.text = LocalizationManager.Instance.GetContent("170");
            
            // Maze Electronic Game
            mazeTitleText.text = LocalizationManager.Instance.GetContent("107");
            mazeHintText.text = LocalizationManager.Instance.GetContent("108");
            mazeSuccessText.text = LocalizationManager.Instance.GetContent("109");
            
            // Cipher Breaker Game
            cipherTitleText.text = LocalizationManager.Instance.GetContent("110");
            cipherHintText.text = LocalizationManager.Instance.GetContent("111");
            cipherSuccessText.text = LocalizationManager.Instance.GetContent("116");
            cipherErrorText.text = LocalizationManager.Instance.GetContent("112");
            cipherFailedText.text = LocalizationManager.Instance.GetContent("114");
            cipherRetryErrorText.text = LocalizationManager.Instance.GetContent("113");
            cipherRetryFailedText.text = LocalizationManager.Instance.GetContent("115");

            // Last Doctor Letter
            page1LetterDoctor.text = LocalizationManager.Instance.GetContent("162");
            page2LetterDoctor.text = LocalizationManager.Instance.GetContent("163");
            page3LetterDoctor.text = LocalizationManager.Instance.GetContent("164");
            page4LetterDoctor.text = LocalizationManager.Instance.GetContent("165");
            page5LetterDoctor.text = LocalizationManager.Instance.GetContent("166");

            moreSpecificCandleHint.text = LocalizationManager.Instance.GetContent("201");
            moreSpecificIncenseBurnerHint.text = LocalizationManager.Instance.GetContent("202");

            // Tutorial Window
            tutorialWindowTextHint.text = LocalizationManager.Instance.GetContent("204");
            tutorialWindowContinueText.text = LocalizationManager.Instance.GetContent("205");

            // Sewers
            leverCombinationHint.text = LocalizationManager.Instance.GetContent("238");
            journalDescription.text = LocalizationManager.Instance.GetContent("239");
            journalTitle.text = LocalizationManager.Instance.GetContent("240");

            foreach (var exit in exitText)
            {
                exit.text = LocalizationManager.Instance.GetContent("171");
            }
        }
    }
}