using GameScene.Data.Handlers;
using System.Collections.Generic;
using UnityEngine;

public class LeverManager : MonoBehaviour
{
    [SerializeField] private List<Lever> levers;
    [SerializeField] private Animator drawerAnimator;
    [SerializeField] private GameObject openDrawerSound;
    [SerializeField] private GameObject jumpscareTrigger;

    private List<string> _leversSelection;
    private bool _isLeverUnlocked;

    private void Start()
    { 
        _leversSelection = new List<string>();
    }

    public bool IsLeverUnlocked()
    {
        return _isLeverUnlocked;
    }

    private bool CanTryCombination()
    {
        if(_leversSelection.Count == 3)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    internal void AddLeverSymbolToSelection(string symbolName)
    {
        _leversSelection.Add(symbolName);

        if(CanTryCombination())
        {
            if(CheckCombination())
            {
                OpenDrawer();
                _isLeverUnlocked = true;
                Instantiate(openDrawerSound, transform.position, Quaternion.identity);
                AssignmentsDataHandler.Instance.Complete(36);
                jumpscareTrigger.SetActive(true);
            }
            else
            {
                _leversSelection.Clear();
                foreach(var lever in levers)
                {
                    StartCoroutine(lever.ResetLever());
                }
            }
        }
    }

    internal void OpenDrawer()
    {
        drawerAnimator.SetTrigger("Open");
    }

    private bool CheckCombination()
    {
        if (_leversSelection[0] == "sun" && _leversSelection[1] == "tree" && _leversSelection[2] == "moon")
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}