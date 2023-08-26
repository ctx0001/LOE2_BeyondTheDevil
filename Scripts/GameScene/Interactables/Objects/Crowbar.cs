using Common.Localization.Speaking;
using GameScene.Data.Handlers;
using GameScene.Data.Handlers.Dependencies;
using GameScene.Interactables;
using Localization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crowbar : InteractableObject
{
    [SerializeField] private MeshRenderer mesh;
    [SerializeField] private MultipleDialogue takeCrowBar;

    private void Start()
    {
        base.Initialize();
    }

    protected override void Interact()
    {
        base.Interact();
        takeCrowBar.Play();

        if (!AssignmentsDataHandler.Instance.Exists(24))
        {
            var assignment = new Assignment(24, LocalizationManager.Instance.GetContent("212"), null, true, false);
            AssignmentsDataHandler.Instance.Create(assignment, 23);
        }

        mesh.enabled = false;
        gameObject.tag = "Untagged";
    }
}
