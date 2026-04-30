using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDetect : MonoBehaviour
{
    [Header("Ref")]
    [SerializeField] private Collider2D Collider;

    [Header("Target Layer")]
    [SerializeField] private LayerMask targetLayer;

    private Transform currentTargetPosition;
    private List<GameObject> targets = new List<GameObject>();

    public event Action<Transform> OnTargetChanged;

    public Transform GetCurrentTargetPosition => currentTargetPosition;

    private void Awake()
    {
        if (Collider == null) Collider = GetComponent<Collider2D>();
    }

    private void Start()
    {
        targets.Clear();

        FindClosestTarget();

        if (currentTargetPosition == null) currentTargetPosition = GameManager.Instance.mainBuilding.transform;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & targetLayer) == 0) return;

        targets.Add(collision.gameObject);

        FindClosestTarget();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision == null) return;

        if (((1 << collision.gameObject.layer) & targetLayer) == 0) return;

        if (targets.Contains(collision.gameObject))
        {
            targets.Remove(collision.gameObject);
        }
        else
        {
            Debug.LogWarning("[EnemyDetect] OnTriggerExit2D targets not contain collision");
        }

        FindClosestTarget();
    }

    private void FindClosestTarget()
    {
        targets.RemoveAll(t => t == null);

        if (targets.Count == 0)
        {
            if (GameManager.Instance != null && GameManager.Instance.mainBuilding != null)
            {
                currentTargetPosition = GameManager.Instance.mainBuilding.transform;
                OnTargetChanged?.Invoke(currentTargetPosition);
            }
            return;
        }

        Transform closestTarget = null;
        float minDist = Mathf.Infinity;

        foreach (GameObject target in targets)
        {
            if (target == null) continue;

            float distance = (target.transform.position - transform.position).sqrMagnitude;

            if (distance < minDist)
            {
                minDist = distance;
                closestTarget = target.transform;
            }
        }

        currentTargetPosition = closestTarget;

        OnTargetChanged?.Invoke(currentTargetPosition);
    }
}
