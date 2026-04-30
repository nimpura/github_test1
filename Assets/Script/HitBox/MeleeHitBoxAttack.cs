using System;
using System.Collections.Generic;
using UnityEngine;

public class MeleeHitBoxAttack : MonoBehaviour
{
    [Header("Ref")]
    [SerializeField] private CircleCollider2D attackRange;
    [SerializeField] private MeleeHitbox hitboxPrefab;
    [SerializeField] private DamageManager damage;
    [SerializeField] private AttackManager attackCooldown;

    [Header("Target Layer")]
    [SerializeField] private LayerMask targetLayer;

    [Header("TargetUpdateTick")]
    [SerializeField] private float UpdateTick = 0.2f;

    private Transform targetTransform;
    private List<GameObject> targets = new List<GameObject>();
    private float spawnDistance;
    private float lastAttackTime = -999f;
    private float lastUpdateTime = -999f;
    
    public event Action OnAttack;

    private void Awake()
    {
        if (attackRange == null) attackRange = GetComponentInChildren<CircleCollider2D>();
        if (damage == null) damage = GetComponent<DamageManager>();
        if (attackCooldown == null) attackCooldown = GetComponent<AttackManager>();
    }

    private void Start()
    {
        targets.Clear();
        spawnDistance = attackRange.radius * transform.lossyScale.x; ;
    }

    private void Update()
    {
        if (Time.time > lastUpdateTime + UpdateTick)
        {
            lastUpdateTime = Time.time;

            FindOneTarget();
        }

        if (targetTransform != null)
        {
            TryAttack();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & targetLayer) == 0) return;

        targets.Add(collision.gameObject);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & targetLayer) == 0) return;

        if (targets.Contains(collision.gameObject))
        {
            targets.Remove(collision.gameObject);
        }
        else
        {
            Debug.LogWarning("[EnemyDetect] OnTriggerExit2D targets not contain collision");
        }
    }

    private void FindOneTarget()
    {
        if (targets == null || targets.Count == 0)
        {
            targetTransform = null;
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

        targetTransform = closestTarget;
    }

    private void TryAttack()
    {
        if (Time.time < lastAttackTime + attackCooldown.GetAttackCooldown) return;

        if (hitboxPrefab == null)
        {
            Debug.LogWarning("[MeleeHitBoxAttack] HitBox Prefab is not found");
            return;
        }

        lastAttackTime = Time.time;

        OnAttack?.Invoke();

        SpawnHitBox();
    }

    private void SpawnHitBox()
    {
        Vector3 dir = (targetTransform.position - transform.position).normalized;

        Vector3 spawnPosition = transform.position + dir * spawnDistance;

        MeleeHitbox spawnedHitbox = Instantiate(hitboxPrefab, spawnPosition, Quaternion.identity);

        spawnedHitbox.Initialize(damage.GetDamage, gameObject);
    }

}
