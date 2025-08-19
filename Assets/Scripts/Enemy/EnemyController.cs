using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private GameObject canvasHP;
    [SerializeField] private NavMeshAgent navMeshAgent;
    [SerializeField] private GameObject deathEffect;

    private float findInterval = 3f;
    private float nextFindTime = 0f;

    private void Start()
    {
        navMeshAgent.speed = Random.Range(1f, 2f);
    }

    private void FixedUpdate()
    {
        if (PlayerController.Instance == null) return;
        if (Time.time - nextFindTime > findInterval)
        {
            navMeshAgent.SetDestination(PlayerController.Instance.transform.position);
            nextFindTime = Time.time;
        }
    }

    public void Die()
    {
        navMeshAgent.isStopped = true;
        canvasHP.SetActive(false);
        Instantiate(deathEffect, transform.position, Quaternion.identity);
        Destroy(gameObject, 3f);

        GameManager.Instance.EnemyKilled();
    }
}
