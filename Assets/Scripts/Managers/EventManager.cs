using System;
using UnityEngine;

namespace Managers
{
    public class EventManager : MonoBehaviour
    {
        public static EventManager _eventManager;

        public event Action<Managers.TeamManager, bool> OnDeathTrigger;

        private void Awake()
        {
            if (_eventManager == null)
            {
                _eventManager = this;
            }
            else if (_eventManager != this)
            {
                Destroy(this);
            }
        }

        public void DeathTrigger(Managers.TeamManager team, bool controlling)
        {
            OnDeathTrigger?.Invoke(team, controlling);
        }
    }
}