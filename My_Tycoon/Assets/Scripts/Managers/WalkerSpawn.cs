using System.Collections;
using UnityEngine;

namespace Managers
{
    public class WalkerSpawner : MonoBehaviour
    {
        [SerializeField] private float spawnInterval = 3f;
        [SerializeField] private float spawnHeight = 1f;

        private void Start()
        {
            StartCoroutine(SpawnLoop());
        }

        private IEnumerator SpawnLoop()
        {
            while (true)
            {
                yield return new WaitForSeconds(spawnInterval);

                Vector3 spawnPosition = GetRandomRoadPosition();
                if (spawnPosition != Vector3.zero)
                {
                    var walker = WalkerPoolManager.Instance.GetFreeWalker();
                    if (walker != null)
                    {
                        Vector3 direction = Random.value > 0.5f ? Vector3.right : Vector3.left;
                        walker.Activate(spawnPosition, direction);
                    }
                }
            }
        }

        private Vector3 GetRandomRoadPosition()
        {
            Collider[] roadColliders = Physics.OverlapSphere(transform.position, 100f, LayerMask.GetMask("Road"));

            if (roadColliders.Length > 0)
            {
                Collider randomRoad = roadColliders[Random.Range(0, roadColliders.Length)];
                Vector3 spawnPos = randomRoad.ClosestPointOnBounds(transform.position);
                spawnPos.y = spawnHeight;
                return spawnPos;
            }

            return Vector3.zero;
        }
    }
}