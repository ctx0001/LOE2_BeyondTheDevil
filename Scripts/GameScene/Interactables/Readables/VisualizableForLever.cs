using Common.Localization.Speaking;
using GameScene.Data.Handlers;
using GameScene.Data.Handlers.Dependencies;
using GameScene.Interactables.Readables;
using System.Collections;
using UnityEngine;

public class VisualizableForLever : Visualizable
{
    [SerializeField] private LeverManager leverManager;
    [SerializeField] private MultipleDialogue sawLeverLetter;
    [SerializeField] private MultipleDialogue leverHint1;
    [SerializeField] private MultipleDialogue leverHint2;
    [SerializeField] private MultipleDialogue leverHint3;

    private AudioClip _leverHintFinal;
    private bool _firstTime = true;
    private float WaitSecondsFromHints = 35f;

    private void Start()
    {
        _leverHintFinal = Resources.Load<AudioClip>("Audio/lever-hint-final");
        _leverHintFinal.LoadAudioData();
    }

    protected override void OnClose()
    {
        if (_firstTime && !AssignmentsDataHandler.Instance.Exists(36))
        {
            var assignment = new Assignment(36, "Check the levers", null, true, false);
            AssignmentsDataHandler.Instance.Create(assignment, 35);
            sawLeverLetter.Play();
            StartCoroutine(SpeakRoutine());
        }
    }

    private IEnumerator SpeakRoutine()
    {
        yield return new WaitForSeconds(WaitSecondsFromHints);
        if (leverManager.IsLeverUnlocked()) yield break;
        leverHint1.Play();
        yield return new WaitForSeconds(WaitSecondsFromHints);
        if (leverManager.IsLeverUnlocked()) yield break;
        leverHint2.Play();
        yield return new WaitForSeconds(WaitSecondsFromHints);
        if (leverManager.IsLeverUnlocked()) yield break;
        leverHint3.Play();
        yield return new WaitForSeconds(WaitSecondsFromHints);
        if (leverManager.IsLeverUnlocked()) yield break;
        SpeakManager.Instance.SpeakSingle(Localization.LocalizationManager.Instance.GetContent("224"), _leverHintFinal, 0.5f);
    }
}