using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace GameScene.Pc
{
    public class ComputerFolder : MonoBehaviour
    {
        [SerializeField] private string folderName;
        [SerializeField] private string folderPath;
        [SerializeField] private List<DocumentFile> documentFiles = new List<DocumentFile>();
        [SerializeField] private List<Folder> folders = new List<Folder>();
        [SerializeField] private List<ImageFile> imageFiles = new List<ImageFile>();
        [SerializeField] private List<AudioFile> audioFiles = new List<AudioFile>();
        [SerializeField] private List<SheetFile> sheetFiles = new List<SheetFile>();

        [SerializeField] private TextMeshProUGUI folderNameText;
        
        private DesktopHandler _desktopHandler;

        public void Initialize(string folderNameParam, string folderPathParam, List<DocumentFile> documentFilesParam,
            List<Folder> foldersParam, List<ImageFile> imageFilesParam, List<AudioFile> audioFilesParam, List<SheetFile> sheetFilesParam)
        {
            folderName = folderNameParam;
            folderPath = folderPathParam;
            documentFiles = documentFilesParam;
            folders = foldersParam;
            imageFiles = imageFilesParam;
            audioFiles = audioFilesParam;
            sheetFiles = sheetFilesParam;
        }

        private void Start()
        {
            _desktopHandler = GameObject.Find("Desktop").GetComponent<DesktopHandler>();
            folderNameText.text = folderName;
        }

        public void SetName(string newName)
        {
            folderNameText.text = newName;
        }
        
        public void ViewContent()
        {
            _desktopHandler.ViewFolder(folderName, folderPath, documentFiles, folders, imageFiles, audioFiles, sheetFiles);
        }
        
        [Serializable]
        public class DocumentFile
        {
            
            [SerializeField] private string name;
            [SerializeField] private string content;

            public DocumentFile(string fileName, string fileContent)
            {
                name = fileName;
                content = fileContent;
            }

            public string GetName() => name;
            public string GetContent() => content;
        }
        
        [Serializable]
        public class SheetFile
        {
            
            [SerializeField] private string name;
            [SerializeField] private string csvContentId;

            public SheetFile(string fileName, string fileContentId)
            {
                name = fileName;
                csvContentId = fileContentId;
            }

            public string GetName() => name;
            public string GetContentId() => csvContentId;
        }
        
        [Serializable]
        public class AudioFile
        {
            
            [SerializeField] private string name;
            [SerializeField] private AudioClip audioClip;

            public AudioFile(string fileName, AudioClip audioClip)
            {
                name = fileName;
                this.audioClip = audioClip;
            }

            public string GetName() => name;
            public AudioClip GetAudioClip() => audioClip;
        }
        
        [Serializable]
        public class ImageFile
        {
            
            [SerializeField] private string name;
            [SerializeField] private Sprite image;

            public ImageFile(string fileName, Sprite sprite)
            {
                name = fileName;
                image = sprite;
            }

            public string GetName() => name;
            public Sprite GetImage() => image;
        }
        
        [Serializable]
        public class Folder
        {
            
            [SerializeField] private string name;
            [SerializeField] private string path;
            [SerializeField] private List<DocumentFile> documentFiles = new List<DocumentFile>();
            [SerializeField] private List<Folder> folders = new List<Folder>();
            [SerializeField] private List<ImageFile> imageFiles = new List<ImageFile>();
            [SerializeField] private List<AudioFile> audioFiles = new List<AudioFile>();
            [SerializeField] private List<SheetFile> sheetFiles = new List<SheetFile>();

            public Folder(string name, string path, List<DocumentFile> documentFiles, List<Folder> folders, List<ImageFile> imageFiles
                ,List<AudioFile> audioFiles, List<SheetFile> sheetFiles)
            {
                this.name = name;
                this.path = path;
                this.documentFiles = documentFiles;
                this.folders = folders;
                this.imageFiles = imageFiles;
                this.audioFiles = audioFiles;
                this.sheetFiles = sheetFiles;
            }

            public string GetName() => name;
            public string GetPath() => path;
            public List<DocumentFile> GetDocumentFiles() => documentFiles;
            public List<Folder> GetFolders() => folders;
            public List<ImageFile> GetImageFiles() => imageFiles;

            public List<AudioFile> GetAudiOFiles() => audioFiles;

            public List<SheetFile> GetSheetFiles() => sheetFiles;
        }
    }
}