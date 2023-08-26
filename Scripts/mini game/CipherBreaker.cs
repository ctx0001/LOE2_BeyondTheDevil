using System.Collections;
using GameScene.Interactables.Openables;
using Managers;
using Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace mini_game
{
    public class CipherBreaker : MonoBehaviour
    {
        [SerializeField] private Transform defaultGrid;
        [SerializeField] private Transform endGrid;
        [SerializeField] private GameObject cellTemplate;
        [SerializeField] private TextMeshProUGUI encryptedText;
        [SerializeField] private GameObject typeWriterSound;
        
        [Header("Error")] 
        [SerializeField] private GameObject errorAlert;
        [SerializeField] private Slider errorTimeSlider;
        [SerializeField] private GameObject systemErrorSound;

        [Header("Timer")]
        [SerializeField] private Slider timerSlider;
        [SerializeField] private GameObject hackingFailedAlert;

        [Header("Success")] 
        [SerializeField] private GameObject successAlert;
        [SerializeField] private GameObject gameCanvas;
        [SerializeField] private GameObject correctSound;
        [SerializeField] private GameObject successSound;
        [SerializeField] private Safe safe;

        [Header("Round")] 
        [SerializeField] private GameObject passed1;
        [SerializeField] private GameObject passed2;
        [SerializeField] private GameObject passed3;

        [SerializeField] private AudioManager audioManager;
        [SerializeField] private AudioSource gameMusic;

        [SerializeField] private Visual visual;
        [SerializeField] private Movement movement;
        
        private string _encrypted;
        private string _userEncryptedString = "";
        private const string Chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789$#%@!&";
        private readonly System.Random _random = new System.Random();
        private int _round = 0;
        private bool _win;
        
        private void ResetGame()
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            errorAlert.SetActive(false);
            _encrypted = "";
            _userEncryptedString = "";
            _round = 0;
            _win = false;
        }
        
        private string GetRandomString(int length)
        {
            var stringChars = new char[length];

            for (var i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = Chars[_random.Next(Chars.Length)];
            }

            return new string(stringChars);
        }

        private IEnumerator AddCharactersToEncryptedText()
        {
            encryptedText.text = "";
            foreach (var character in _encrypted)
            {
                Instantiate(typeWriterSound, transform.position, Quaternion.identity);
                encryptedText.text += character.ToString();
                yield return new WaitForSeconds(0.1f);
            }
            
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }

        private IEnumerator StartRoundTimer()
        {
            var timer = 110f;
            timerSlider.maxValue = timer;
            
            while (timer > 0 && !_win)
            {
                timer -= 0.05f;
                timerSlider.value = timer;
                yield return new WaitForSeconds(0.05f);
            }

            if (!_win)
            {
                Lose();   
            }
        }

        private void Lose()
        {
            Instantiate(systemErrorSound, transform.position, Quaternion.identity);
            hackingFailedAlert.SetActive(true);
            _round = 0;
            _win = false;
        }

        private void CheckForWin()
        {
            if (_round == 2)
            {
                _round++;
                ResetRoundDesign();
                StartCoroutine(Win());
            }
            else
            {
                _round++;
                ResetRoundDesign();
                StartNewRound();
            }
        }

        private IEnumerator Win()
        {
            Instantiate(successSound, transform.position, Quaternion.identity);
            _win = true;
            successAlert.SetActive(true);
            yield return new WaitForSeconds(3f);
            gameCanvas.SetActive(false);
            gameMusic.Stop();
            audioManager.EnableAmbienceAudio();
            safe.OpenSafe();
        }

        private void ResetRoundDesign()
        {
            switch (_round)
            {
                case 0:
                    passed1.SetActive(false);
                    passed2.SetActive(false);
                    passed3.SetActive(false);
                    break;
                case 1:
                    passed1.SetActive(true);
                    break;
                case 2:
                    passed1.SetActive(true);
                    passed2.SetActive(true);
                    break;
                case 3:
                    passed1.SetActive(true);
                    passed2.SetActive(true);
                    passed3.SetActive(true);
                    break;
            }
        }
        
        public void ResetGrid(bool error)
        {
            if (error)
            {
                StartCoroutine(LaunchError());
                _userEncryptedString = "";
            }
            
            foreach (Transform child in defaultGrid.transform) {
                Destroy(child.gameObject);
            }
            
            foreach (Transform child in endGrid.transform) {
                Destroy(child.gameObject);
            }

            var allCharacters = _encrypted;
            
            for (var i = 0; i < 25; i++)
            {
                allCharacters += Chars[_random.Next(Chars.Length)];
            }

            var shuffledArray = allCharacters.ToCharArray();
            shuffledArray = Randomize(shuffledArray);
            var shuffledString = new string(shuffledArray);
            
            foreach (var character in shuffledString)
            {
                var cell = Instantiate(cellTemplate, transform.position, Quaternion.identity);
                // BUG: Check if is correct or revert
                //cell.transform.parent = defaultGrid.transform;
                cell.transform.SetParent(defaultGrid.transform, false);
                cell.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = character.ToString();
            }
        }

        private IEnumerator LaunchError()
        {
            Instantiate(systemErrorSound, transform.position, Quaternion.identity);
            errorAlert.SetActive(true);
            var errorTime = 3f;
            errorTimeSlider.maxValue = errorTime;
            
            while (errorTime > 0f)
            {
                errorTime -= 0.05f;
                errorTimeSlider.value = errorTime;
                yield return new WaitForSeconds(0.05f);
            }
            
            errorAlert.SetActive(false);
        }
        
        private void StartNewRound()
        {
            if(_round != 0)
                Instantiate(correctSound, transform.position, Quaternion.identity);
            
            _userEncryptedString = "";
            _encrypted = GetRandomString(8);
            StartCoroutine(AddCharactersToEncryptedText());
            ResetGrid(false);
            ResetRoundDesign();
        }

        public void StartNewGame()
        {
            if (!gameMusic.isPlaying)
            {
                audioManager.DisableAllAudio();
                gameMusic.Play();   
            }

            gameCanvas.SetActive(true);
            hackingFailedAlert.SetActive(false);
            StartNewRound();
            StartCoroutine(StartRoundTimer());
        }

        public void CheckForEqualityOfStrings()
        {
            if (_encrypted.Equals(_userEncryptedString))
            {
                CheckForWin();
            }
            else
            {
                StartCoroutine(LaunchError());
            }
        }

        public void AddCharacterToUserEncryptedString(char sym)
        {
            _userEncryptedString += sym;
        }

        public string GetUserEncryptedString()
        {
            return _userEncryptedString;
        }

        public string GetEncryptedString()
        {
            return _encrypted;
        }
        
        private char[] Randomize(char[] arr)
        {
            for (var i = arr.Length - 1; i > 0; i--)
            {
                var j = _random.Next(i + 1);
                (arr[i], arr[j]) = (arr[j], arr[i]);
            }
            return arr;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape) && !_win)
            {
                // Reset the game
                StopAllCoroutines();
                ResetGame();
                
                // Player can now move
                visual.SetRotateStatus(true);
                movement.SetMoveStatus(true);
                
                // Reset graphics and audio
                gameCanvas.SetActive(false);
                gameMusic.Stop();
                audioManager.EnableAmbienceAudio();
            }
        }
    }
}