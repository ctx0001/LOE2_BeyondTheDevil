
using System;
using System.Collections;
using GameScene.Data.Handlers;
using GameScene.Data.Handlers.Dependencies;
using GameScene.Interactables.Objects.Rituals;
using Localization;
using UnityEngine;

namespace GameScene.Interactables.Readables
{
    public class VisualizableLastDoctorLetterVariation : MultipleVisualizable
    {
        [SerializeField] private RitualObjectController ritualObjectController;
        private bool _firstTimeSawRitualPage = true;
        [SerializeField] private GameObject bloodDeco;

        private IEnumerator Start()
        {
            while (!AssignmentsDataHandler.Instance.IsLoaded())
            {
                yield return null;
            }

            if (AssignmentsDataHandler.Instance.Exists(18))
            {
                bloodDeco.SetActive(true);
            }
        }

        protected override void OnClose()
        {
            if (!_firstTimeSawRitualPage) return;
            if (AssignmentsDataHandler.Instance.Exists(18)) return;
            
            var assignment = new Assignment(18, LocalizationManager.Instance.GetContent("181"), null, true, false);
            AssignmentsDataHandler.Instance.Create(assignment);
            bloodDeco.SetActive(true);

            
            ritualObjectController.StartSearching();
            _firstTimeSawRitualPage = false;
        }
    }
}