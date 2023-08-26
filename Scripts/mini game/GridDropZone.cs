using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace mini_game
{
    public class GridDropZone : MonoBehaviour, IDropHandler
    {
        [SerializeField] private CipherBreaker cipherBreaker;
        [SerializeField] private Transform defaultGridZone;
        
        public void OnDrop(PointerEventData eventData)
        {
            var draggedObject = eventData.pointerDrag;
            var textChildren = draggedObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            
            if (textChildren != null)
            {
                var value = Convert.ToChar(textChildren.text);
                var encryptedString = cipherBreaker.GetEncryptedString().ToCharArray();
                
                if (cipherBreaker.GetUserEncryptedString().ToCharArray().Length == 0)
                {
                    var nextLetter = encryptedString[0];
                    
                    if (nextLetter != value)
                    {
                        cipherBreaker.ResetGrid(true);
                    }
                    else
                    {
                        cipherBreaker.AddCharacterToUserEncryptedString(value);   
                    }
                }
                else
                {
                    var nextLetter = encryptedString[cipherBreaker.GetUserEncryptedString().ToCharArray().Length];

                    if (nextLetter != value)
                    {
                       cipherBreaker.ResetGrid(true);
                    }
                    else
                    {
                        cipherBreaker.AddCharacterToUserEncryptedString(value);
                        if(cipherBreaker.GetUserEncryptedString().ToCharArray().Length == 8)
                            cipherBreaker.CheckForEqualityOfStrings();
                    }
                }
                
            }
        }
    }
}