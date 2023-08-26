using Common.Localization.Speaking;
using GameScene.Data.Handlers;
using GameScene.Data.Handlers.Dependencies;
using UnityEngine;

public class SawLeverTrigger : MonoBehaviour
{
    private AudioClip _sawLeversAudioClip;

    private void Start()
    {
        _sawLeversAudioClip =  Resources.Load<AudioClip>("Audio/saw-levers");
        _sawLeversAudioClip.LoadAudioData();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (AssignmentsDataHandler.Instance.Exists(35))
            {
                Destroy(gameObject);
                return;
            }

            AssignmentsDataHandler.Instance.Complete(39);
            var assignment = new Assignment(35, "Look for clues about levers", null, true, false);
            AssignmentsDataHandler.Instance.Create(assignment);
            SpeakManager.Instance.SpeakSingle(Localization.LocalizationManager.Instance.GetContent("215"), _sawLeversAudioClip, 0.5f);
        }
    }
}