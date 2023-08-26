using System.Collections;
using GameScene.Data.Handlers;
using UnityEngine;

namespace Interactable.Utility
{
    public class NotebookPatch : MonoBehaviour
    {
        [SerializeField] private GameObject notebook;
        [SerializeField] private GameObject officeDoll;
        
        private void Start()
        {
            StartCoroutine(WaitForDataLoad());
        }
        
        private IEnumerator WaitForDataLoad()
        {
            yield return new WaitForSeconds(5f);
            
            if (AssignmentsDataHandler.Instance.Exists(6))
            {
                notebook.SetActive(true);
                officeDoll.SetActive(false);
            }
        }
    }
}