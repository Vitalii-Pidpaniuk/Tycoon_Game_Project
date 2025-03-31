using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Managers
{
    public class EconomyManager : Singleton<EconomyManager>
    {
        public float Money { get; private set; }

        public void AddMoney(float amount)
        {
        }

        public bool SpendMoney(float amount)
        {
            return false;
        }
    }

}