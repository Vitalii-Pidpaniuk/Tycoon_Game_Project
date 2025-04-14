using UnityEngine;
using System.Collections;

namespace NPC
{
    public class Walker : MonoBehaviour
    {
        public float speed = 1.5f;
        public float walkDuration = 5f;
        public float pauseChance = 0.3f;
        public float pauseDuration = 2f;
        public float disappearChance = 0.5f;

        private Vector3 direction;
        private Coroutine walkRoutine;
        private Animator _animator;

        private static readonly Vector3[] possibleDirections = new Vector3[]
        {
            Vector3.forward,
            Vector3.back,
            Vector3.left,
            Vector3.right
        };

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        public void Activate(Vector3 startPosition, Vector3 moveDirection)
        {
            gameObject.SetActive(true);

            transform.position = startPosition + Vector3.up * 0.1f;
            direction = moveDirection.normalized;

            RotateToDirection(direction);

            if (walkRoutine != null)
                StopCoroutine(walkRoutine);

            walkRoutine = StartCoroutine(Walk());
        }

        private IEnumerator Walk()
        {
            float timer = 0f;

            if (_animator != null)
                _animator.SetBool("IsWalking", true);

            while (timer < walkDuration)
            {
                Vector3 nextPosition = transform.position + direction * speed * Time.deltaTime;

                if (IsOnRoad(nextPosition))
                {
                    transform.position = nextPosition;
                }
                else
                {
                    bool turned = false;

                    foreach (var dir in ShuffleDirections())
                    {
                        Vector3 checkPos = transform.position + dir * 0.5f;
                        if (IsOnRoad(checkPos))
                        {
                            direction = dir;
                            RotateToDirection(direction);
                            turned = true;
                            break;
                        }
                    }

                    if (!turned)
                        break;
                }

                timer += Time.deltaTime;

                if (timer > 1f && Random.value < pauseChance * Time.deltaTime)
                {
                    if (_animator != null)
                        _animator.SetBool("IsWalking", false);

                    yield return new WaitForSeconds(pauseDuration);

                    if (Random.value < disappearChance)
                    {
                        if (_animator != null)
                            _animator.SetBool("IsWalking", false);

                        Deactivate();
                        yield break;
                    }

                    if (_animator != null)
                        _animator.SetBool("IsWalking", true);
                }

                yield return null;
            }

            if (_animator != null)
                _animator.SetBool("IsWalking", false);

            Deactivate();
        }

        private bool IsOnRoad(Vector3 position)
        {
            Ray ray = new Ray(position + Vector3.up * 3f, Vector3.down);
            if (Physics.Raycast(ray, out RaycastHit hit, 10f))
            {
                return hit.collider.gameObject.layer == LayerMask.NameToLayer("Road");
            }
            return false;
        }

        private void RotateToDirection(Vector3 dir)
        {
            if (dir != Vector3.zero)
            {
                Quaternion lookRotation = Quaternion.LookRotation(dir);
                transform.rotation = Quaternion.Euler(0f, lookRotation.eulerAngles.y, 0f);
            }
        }

        private Vector3[] ShuffleDirections()
        {
            Vector3[] shuffled = (Vector3[])possibleDirections.Clone();
            for (int i = 0; i < shuffled.Length; i++)
            {
                int rnd = Random.Range(i, shuffled.Length);
                var temp = shuffled[i];
                shuffled[i] = shuffled[rnd];
                shuffled[rnd] = temp;
            }
            return shuffled;
        }

        public void Deactivate()
        {
            gameObject.SetActive(false);
        }
    }
}
