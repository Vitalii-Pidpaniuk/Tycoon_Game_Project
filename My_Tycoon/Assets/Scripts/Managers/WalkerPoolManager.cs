using System.Collections.Generic;
using UnityEngine;
using NPC;


namespace Managers
{
    public class WalkerPoolManager : MonoBehaviour
    {
        public static WalkerPoolManager Instance { get; private set; }

        [SerializeField] private GameObject walkerPrefab;
        [SerializeField] private int poolSize = 10;

        private List<Walker> walkers = new List<Walker>();

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;

            for (int i = 0; i < poolSize; i++)
            {
                GameObject obj = Instantiate(walkerPrefab);
                obj.SetActive(false);
                walkers.Add(obj.GetComponent<Walker>());
            }
        }

        public Walker GetFreeWalker()
        {
            foreach (var walker in walkers)
            {
                if (!walker.gameObject.activeInHierarchy)
                    return walker;
            }
            return null;
        }
    }
}   