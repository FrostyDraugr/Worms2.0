using System;
using UnityEngine;

namespace Managers
{
    public class EventManager : MonoBehaviour
    {
        public static EventManager _eventManager;

        public event Action<int, bool> OnDeathTrigger;

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

        public void DeathTrigger(int id, bool controlling)
        {
            OnDeathTrigger?.Invoke(id, controlling);
        }
    }
}