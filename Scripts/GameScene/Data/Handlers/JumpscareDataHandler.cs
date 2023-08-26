using System;
using System.Collections.Generic;
using System.Linq;
using Jumpscares;
using UnityEngine;

namespace GameScene.Data.Handlers
{
    public class JumpscareDataHandler : DataHandler
    {
        [SerializeField] private List<Jumpscare> _jumpScares;
        private List<Root> _jumpscaresData = new List<Root>();

        private void Start()
        {
            SetPath("JumpscaresData.json");
            _jumpscaresData = FetchData<List<Root>>(GetPath());
            UpdateJumpscares();
        }

        public void UpdateJumpScareStatus(int id, bool status)
        {
            foreach (var jumpScare in _jumpscaresData)
            {
                if (jumpScare.id == id)
                    jumpScare.active = status;
            }
            
            UpdateData(GetPath(), _jumpscaresData);
        }

        private void UpdateJumpscares()
        {
            foreach (var jumpScare in _jumpScares.Where(jumpScare => !SearchJumpScareStatus(jumpScare.GetId())))
            {
                jumpScare.Disable();
            }
        }

        // returns status of object
        public bool SearchJumpScareStatus(int id)
        {
            foreach (var jumpScare in _jumpscaresData)
            {
                if (jumpScare.id == id)
                    return jumpScare.active;
            }
            return false;
        }
    }

    [Serializable]
    public class Root
    {
        public int id { get; set; }
        public bool active { get; set; }
    }
}

