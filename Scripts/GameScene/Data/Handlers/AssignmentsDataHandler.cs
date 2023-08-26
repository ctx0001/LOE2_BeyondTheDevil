using System.Collections.Generic;
using System.Linq;
using DebugFolder;
using Exceptions;
using GameScene.Data.Handlers.Dependencies;
using Localization;
using Managers;
using UnityEngine;
using UnityEngine.Playables;

namespace GameScene.Data.Handlers
{
    public class AssignmentsDataHandler : DataHandler
    {
        [SerializeField] private PlayableDirector questCutscene;
        private List<Assignment> _assignments;
        public static AssignmentsDataHandler Instance;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);   
            }
            else
            {
                Instance = this;
            }
        }

        private void Start()
        {
            SetPath("AssignmentsData.json");
            _assignments = FetchData<List<Assignment>>(GetPath());
            MarkLoaded();            
            UpdateCanvas();

            if (Exists(0)) return;
            var assignment = new Assignment(0, LocalizationManager.Instance.GetContent("203"), null, true, false);
            Create(assignment);
        }

        private void UpdateCanvas()
        {
            ResetCanvas();
            foreach (var assignment in _assignments)
            {
                if (assignment.active)
                {
                    CanvasManager.Instance.UpdateQuest(assignment.description, assignment.hint);
                    return;
                }

                if (assignment.completed || assignment.active) continue;
                CanvasManager.Instance.UpdateQuest(assignment.description, assignment.hint);
                return;
            }
        }

        private static void ResetCanvas()
        {
            CanvasManager.Instance.UpdateQuest("", "");
        }

        public void SetAssignmentActive(Assignment assignmentRef)
        {
            foreach (var assignment in _assignments)
            {
                if (assignment.active)
                    assignment.active = false;
            }
            assignmentRef.active = true;
            UpdateCanvas();
            UpdateData(GetPath(), _assignments);
        }

        public void UpdateNewActiveAssignment()
        {
            foreach (var assignment in _assignments)
            {
                if (assignment.active && !assignment.completed)
                    UpdateCanvas();
            }
            UpdateCanvas();
        }
        
        public bool Exists(int id)
        {
            return _assignments.Where(assignment => assignment.id == id).Any(assignment => assignment.id == id);
        }
        
        public bool IsCompleted(int id)
        {
            if (IsLoaded())
            {
                return _assignments.Any(assignment => assignment.id == id && assignment.completed);
            }
            
            throw new NotLoadedContentRequestedException(
                    "Requested to check if assignment is completed, but resources is not loaded yet.");
        }

        public void Complete(int id)
        {
            foreach (var assignment in _assignments.Where(assignment => assignment.id == id))
            {
                assignment.completed = true;
                assignment.active = false;
            }
            
            UpdateCanvas();
            UpdateData(GetPath(), _assignments);
            
        }

        public void Create(Assignment assignment, int idToComplete=-1)
        {
            _assignments.Add(assignment);
            if(idToComplete != -1)
                Complete(idToComplete);
            
            UpdateData(GetPath(), _assignments);
            UpdateCanvas();
            questCutscene.Play();
        }
    }
}