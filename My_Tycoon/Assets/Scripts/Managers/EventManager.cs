using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Managers
{
    public class EventManager : Singleton<EventManager>
    {
        public void TriggerRandomEvent()
        {
        }
        
        protected override void Awake()
        {
            Debug.Log("Event manager Initialized");
            base.Awake();
        }
    }
}