using System;
using System.Collections;
using System.Collections.Generic;
using GameScene.Data.Handlers;
using GameScene.Data.Handlers.Dependencies;
using Localization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameScene.Pc
{
    public class DesktopHandler : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI computerTime;
        [SerializeField] private TextMeshProUGUI computerDateTime;
        [SerializeField] private GameObject loginScreen;
        [SerializeField] private TMP_InputField passwordField;

        [Header("Apps")] 
        [SerializeField] private GameObject documentReaderPrefab;
        [SerializeField] private GameObject explorerPrefab;
        [SerializeField] private GameObject imageViewerPrefab;
        [SerializeField] private GameObject audioPlayerPrefab;
        [SerializeField] private AudioSource audioOutput;
        [SerializeField] private GameObject sheetViewerPrefab; 
        [SerializeField] private GameObject cellPrefab;

        [Header("Dependencies")] 
        [SerializeField] private GameObject documentFilePrefab;
        [SerializeField] private GameObject folderPrefab;
        [SerializeField] private GameObject imageFilePrefab;
        [SerializeField] private GameObject audioFilePrefab;
        [SerializeField] private GameObject sheetFilePrefab;

        [Header("Audio")] 
        [SerializeField] private GameObject StartupSound;
        [SerializeField] private GameObject errorSound;

        [SerializeField] private TextMeshProUGUI hintText;

        
        [Header("Fonts")]
        [SerializeField] private TMP_FontAsset chinaAndJapanFont;
        [SerializeField] private TMP_FontAsset koreanFont;
        [SerializeField] private TMP_FontAsset russianFont;
        
        private string _currentTime;
        private bool _isOn;

        private bool _sawImportantFile;
        private bool _sawHintImage;
        
        private bool _needsCj;
        private bool _needsKr;
        private bool _needsRu;

        public void ViewSheetPreview(string fileName, string content)
        {
            content = content.ToString()[..(content.Length-1)];
            
            string[] rows = content.Split(';');
            int numColumns = rows[0].Split(',').Length;
            
            // Creating the sheetViewer
            var sheetViewerGameObject = Instantiate(sheetViewerPrefab, transform.position, Quaternion.identity);
            sheetViewerGameObject.transform.SetParent(transform);
            
            // Changing the size of the window to fit the sheet
            var sheetRectTransform = sheetViewerGameObject.GetComponent<RectTransform>();
            sheetRectTransform.anchoredPosition = new Vector2(0, 0);
            sheetRectTransform.sizeDelta = new Vector2(numColumns * 100 + 25, sheetRectTransform.sizeDelta.y);
            
            // Setting the name and size of viewport
            sheetViewerGameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = fileName;
            var contentRectTransform = sheetViewerGameObject.transform.GetChild(1).GetComponent<RectTransform>();
            contentRectTransform.sizeDelta = new Vector2(numColumns * 100, contentRectTransform.sizeDelta.y);
            
            //creating and filling cells
            var contentTransform = sheetViewerGameObject.transform.GetChild(1).GetComponent<Transform>();
            foreach (var row in rows)
            {
                foreach (var cell in row.Split(","))
                {
                    var cellGameObject = Instantiate(cellPrefab, transform.position, Quaternion.identity);
                    cellGameObject.transform.SetParent(contentTransform);
                    var textComponent = cellGameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>();

                    if (_needsCj)
                    {
                        textComponent.font = chinaAndJapanFont;
                    }else if (_needsKr)
                    {
                        textComponent.font = koreanFont;
                    }else if (_needsRu)
                    {
                        textComponent.font = koreanFont;
                    }
                    
                    textComponent.text = cell;
                }
            }
            
            sheetViewerGameObject.transform.GetChild(2).GetComponent<Button>().onClick.AddListener(() => Destroy(sheetViewerGameObject));
        }
        
        public void ViewImagePreview(string fileName, Sprite image)
        {
            var imageViewerGameObject = Instantiate(imageViewerPrefab, transform.position, Quaternion.identity);
            imageViewerGameObject.transform.SetParent(transform);
            imageViewerGameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
            imageViewerGameObject.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
            
            
            imageViewerGameObject.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(() => Destroy(imageViewerGameObject));
            imageViewerGameObject.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = fileName;
            imageViewerGameObject.transform.GetChild(2).GetComponent<Image>().sprite = image;
            
            if (fileName.Equals("hint"))
            {
                _sawHintImage = true;
                CheckIfSawAllContent();
            }
        }

        private void CheckIfSawAllContent()
        {
            if(_sawHintImage && _sawImportantFile)
            {
                var assignment = new Assignment(20, LocalizationManager.Instance.GetContent("200"), null, true, false);
                AssignmentsDataHandler.Instance.Create(assignment, 19);
            }
            
            UpdateHint();
        }

        private void UpdateHint()
        {
            var clues = 0;
            if (_sawHintImage)
                clues++;

            if (_sawImportantFile)
                clues++;

            var translation = LocalizationManager.Instance.GetContent("177");
            hintText.text = $"{translation} ({clues}/2)";
        }
        
        public void ViewFileContent(string fileName, string content)
        {
            var documentReader = Instantiate(documentReaderPrefab, transform.position, Quaternion.identity);
            documentReader.transform.SetParent(transform);
            documentReader.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
            documentReader.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
            
            documentReader.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = fileName;
            var textComponent = documentReader.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>();
            
            if (_needsCj)
            {
                textComponent.font = chinaAndJapanFont;
            }else if (_needsKr)
            {
                textComponent.font = koreanFont;
            }else if (_needsRu)
            {
                textComponent.font = koreanFont;
            }
            
            textComponent.text = content;
            documentReader.transform.GetChild(2).GetComponent<Button>().onClick.AddListener(() => Destroy(documentReader));

            if (fileName.Equals("important"))
            {
                _sawImportantFile = true;
                CheckIfSawAllContent();
            }
        }
        
        public void ViewAudio(string fileName, AudioClip audioClip)
        {
            var audioPlayer = Instantiate(audioPlayerPrefab, transform.position, Quaternion.identity);
            audioPlayer.transform.SetParent(transform);
            audioPlayer.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
            audioPlayer.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
            audioPlayer.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(() =>
            {
                audioOutput.Stop();
                Destroy(audioPlayer);
            });
            audioPlayer.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = fileName;
            
            // calculate wave form
            var audioData = new float[audioClip.samples * audioClip.channels];
            audioClip.GetData(audioData, 0);

            var samplesPerPixel = audioClip.samples / 1000;
            var samplesPerPixelArray = new float[1000];
            for (var i = 0; i < 1000; i++) {
                var startSample = i * samplesPerPixel;
                var endSample = startSample + samplesPerPixel;
                var sum = 0f;
                for (var j = startSample; j < endSample; j++) {
                    sum += Mathf.Abs(audioData[j]);
                }
                samplesPerPixelArray[i] = sum / samplesPerPixel;
            }

            var waveformTexture = new Texture2D(1000, 100, TextureFormat.RGBA32, false);
            for (var x = 0; x < 1000; x++) {
                for (var y = 0; y < 100; y++) {
                    waveformTexture.SetPixel(x, y, new Color32(85, 85, 85, 255));
                }
            }
            waveformTexture.Apply();
            for (var x = 0; x < 1000; x++) {
                var yValue = (int)(samplesPerPixelArray[x] * 50f);
                for (var y = 50 - yValue; y <= 50 + yValue; y++) {
                    waveformTexture.SetPixel(x, y, new Color32(32, 201, 151, 255));
                }
            }
            waveformTexture.Apply();
            waveformTexture.name = "WaveForm";
            audioPlayer.transform.GetChild(2).GetComponent<RawImage>().texture = waveformTexture;
            
            audioPlayer.transform.GetChild(3).GetComponent<Button>().onClick.AddListener(() =>
            {
                audioOutput.clip = audioClip; 
                audioOutput.Play();
                var line = audioPlayer.transform.Find("Line").GetComponent<Image>();
                StartCoroutine(UpdateLinePosition(line, audioClip.length));
            });
            
            audioPlayer.transform.GetChild(4).GetComponent<Button>().onClick.AddListener(() =>
            {
                audioOutput.Stop(); 
                var line = audioPlayer.transform.Find("Line").GetComponent<Image>();
                line.rectTransform.anchoredPosition = new Vector2(3, line.rectTransform.anchoredPosition.y);
            });

        }
        
        private IEnumerator UpdateLinePosition(Image lineImage, float duration)
        {
            var timeElapsed = 0f;
            while (audioOutput.isPlaying)
            {
                timeElapsed += Time.deltaTime;
                var t = Mathf.Clamp01(timeElapsed / duration);
                var xPos = Mathf.Lerp(3f, 357f, t);

                lineImage.rectTransform.anchoredPosition = new Vector2(xPos, lineImage.rectTransform.anchoredPosition.y);

                yield return null;
            }
        }
        
        public void ViewFolder(string folderName, string folderPath, List<ComputerFolder.DocumentFile> documentFiles,
            List<ComputerFolder.Folder> folders, List<ComputerFolder.ImageFile> imageFiles, List<ComputerFolder.AudioFile> audioFiles,
            List<ComputerFolder.SheetFile> sheetFiles)
        {
            var explorer = Instantiate(explorerPrefab, transform.position, Quaternion.identity);
            explorer.transform.SetParent(transform);
            explorer.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
            explorer.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
            
            explorer.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(() => Destroy(explorer));
            explorer.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text =
                $"/home/robert-desktop/{folderPath}/{folderName}";
            var explorerView = explorer.transform.GetChild(2).transform;
            
            // Creating folders inside the folder
            foreach (var folder in folders)
            {
                var folderGameObject = Instantiate(folderPrefab, transform.position, Quaternion.identity);
                folderGameObject.transform.SetParent(explorerView);
                var computerFolder = folderGameObject.GetComponent<ComputerFolder>();
                
                computerFolder.SetName(folder.GetName());
                computerFolder.Initialize(folder.GetName(), folder.GetPath(), folder.GetDocumentFiles(), folder.GetFolders(), folder.GetImageFiles()
                    , folder.GetAudiOFiles(), folder.GetSheetFiles());
            }
            
            // Creating files inside the folder
            foreach (var documentFile in documentFiles)
            {
                var file = Instantiate(documentFilePrefab, transform.position, Quaternion.identity);
                file.transform.SetParent(explorerView);
                
                var computerFile = file.GetComponent<ComputerFile>();
                computerFile.SetName(documentFile.GetName());
                computerFile.SetContent(documentFile.GetContent());
                computerFile.Initialize();
            }
            
            foreach (var sheetFile in sheetFiles)
            {
                var file = Instantiate(sheetFilePrefab, transform.position, Quaternion.identity);
                file.transform.SetParent(explorerView);
                
                var computerSheet = file.GetComponent<ComputerSheet>();
                computerSheet.SetName(sheetFile.GetName());
                computerSheet.SetContent(sheetFile.GetContentId());
                computerSheet.Initialize();
            }
            
            foreach (var imageFile in imageFiles)
            {
                var imageFileGameObject = Instantiate(imageFilePrefab, transform.position, Quaternion.identity);
                imageFileGameObject.transform.SetParent(explorerView);
                
                var computerImage = imageFileGameObject.GetComponent<ComputerImage>();
                computerImage.SetName(imageFile.GetName());
                computerImage.SetImage(imageFile.GetImage());
                computerImage.Initialize();
            }
            
            foreach (var audioFile in audioFiles)
            {
                var audioFileGameObject = Instantiate(audioFilePrefab, transform.position, Quaternion.identity);
                audioFileGameObject.transform.SetParent(explorerView);
                
                var computerFile = audioFileGameObject.GetComponent<ComputerAudio>();
                computerFile.SetName(audioFile.GetName());
                computerFile.SetClip(audioFile.GetAudioClip());
                computerFile.Initialize();
            }
        }
        
        // to change with "private void Awake()"
        private void Start()
        {
            if (PlayerPrefs.GetString("Language").Equals("zh") || PlayerPrefs.GetString("Language").Equals("jp"))
            {
                _needsCj = true;
            }else if (PlayerPrefs.GetString("Language").Equals("ko"))
            {
                _needsKr = true;
            }else if (PlayerPrefs.GetString("Language").Equals("ru"))
            {
                _needsRu = true;
            }
            
            UpdateHint();
            
            var dayOfWeek = DateTime.Now.ToString("dddd");
            var firstIteration = true;
            var newString = "";
            
            foreach (var ch in dayOfWeek)
            {
                if (firstIteration)
                    newString += ch.ToString().ToUpper();
                else
                    newString += ch;
                
                firstIteration = false;
            }
            
            computerDateTime.text = newString + " " + DateTime.Now.ToString("dd/MM/yyyy");;
        }

        public IEnumerator UpdateComputerTime()
        {
            while (_isOn)
            {
                yield return new WaitForSeconds(1f);
                _currentTime = DateTime.Now.ToString("HH:mm");

                if (_currentTime != computerTime.text)
                    computerTime.text = _currentTime;   
            }
        }

        public void Login()
        {
            var password = passwordField.text;

            if (password.Equals("Robert2012"))
            {
                loginScreen.SetActive(false);
                Instantiate(StartupSound, transform.position, Quaternion.identity);
            }
            else
            {
                passwordField.text = "";
                Instantiate(errorSound, transform.position, Quaternion.identity);
            }
        }
    }
}