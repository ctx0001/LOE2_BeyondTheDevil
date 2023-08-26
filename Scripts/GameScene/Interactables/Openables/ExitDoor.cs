using Common.Localization.Speaking;
using GameScene.Data.Handlers;
using GameScene.Interactables.Openables;
using Localization;
using Managers;
using System.Collections;
using UnityEngine;

public class ExitDoor : Openable
{
    [Header("Audio")]
    [Tooltip("Played when player try to open a door that is locked")]
    [SerializeField] private GameObject lockedDoorSound;

    [Header("References")]
    [SerializeField] private GameObject doorPlanks;
    [SerializeField] private DollDataHandler dollDataHandler;
    [SerializeField] private PauseManager pauseManager;

    private AudioClip _objHoldHand;
    private AudioClip _findCrowbarFirst;
    private AudioClip _cantExitNow;

    private bool _isBlocked;

    private IEnumerator Start()
    {
        while (!dollDataHandler.IsLoaded() && !AssignmentsDataHandler.Instance.IsLoaded())
        {
            yield return null;
        }

        if (dollDataHandler.GetDollStatus().Equals("active") || AssignmentsDataHandler.Instance.Exists(22))
        {
            ActivatePlanks();            
        }

        _objHoldHand = Resources.Load<AudioClip>("Audio/obj-hold-hand");
        _objHoldHand.LoadAudioData();

        _findCrowbarFirst = Resources.Load<AudioClip>("Audio/find-crowbar-first");
        _findCrowbarFirst.LoadAudioData();

        _cantExitNow = Resources.Load<AudioClip>("Audio/cant-exit-now");
        _cantExitNow.LoadAudioData();

    }

    protected override void Interact()
    {
        if (!CanInteract()) return;

        if(_isBlocked)
        {
            if (HasItem("crowbar"))
            {
                if (HasItemInHand("crowbar"))
                {
                    RemoveItem("crowbar");
                    PlayerPrefs.SetString("GameStatus", "Completed");
                    PlayerPrefs.SetInt("RedirectToReviewPage", 1);
                    pauseManager.SendToScene("Credits");
                }
                else
                {
                    SpeakManager.Instance.SpeakSingle(LocalizationManager.Instance.GetContent("7"), _objHoldHand, 0.5f);
                    UpdateCanInteract();
                }
            }
            else
            {
                SpeakManager.Instance.SpeakSingle(LocalizationManager.Instance.GetContent("213"), _findCrowbarFirst, 0.5f);
                Instantiate(lockedDoorSound, transform.position, Quaternion.identity);
                UpdateCanInteract();
            }
        }
        else
        {
            SpeakManager.Instance.SpeakSingle(LocalizationManager.Instance.GetContent("214"), _cantExitNow, 0.5f);
            Instantiate(lockedDoorSound, transform.position, Quaternion.identity);
            UpdateCanInteract();
        }
    }

    public void ActivatePlanks()
    {
        doorPlanks.SetActive(true);
        _isBlocked = true;
    }
}
