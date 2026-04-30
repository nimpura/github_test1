using System.Collections.Generic;
using UnityEngine;

public class MeleeHitbox : MonoBehaviour
{
    [Header("Ref")]
    [SerializeField] private BoxCollider2D boxCollider2D;

    [Header("HitBox LifeTime")]
    [SerializeField] private float lifeTime = 1f;

    [Header("Target Layer")]
    [SerializeField] private LayerMask targetLayer;

    [Header("Hitbox Size")]
    [SerializeField] private Vector2 hitBoxSize = new Vector2(0.5f, 1f);

    private int damage;

    private GameObject owner;

    private HashSet<Collider2D> hitTargets = new HashSet<Collider2D>();


    private void Awake()
    {
        if (boxCollider2D == null) boxCollider2D = GetComponent<BoxCollider2D>();

        boxCollider2D.isTrigger = true;
    }
    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == owner) return;

        HPManager hp = collision.GetComponentInParent<HPManager>();

        if (((1 << collision.gameObject.layer) & targetLayer) == 0) return;

        if (hitTargets.Contains(collision)) return;

        hitTargets.Add(collision);

        if (hp != null && !hp.GetIsDead)
        {
            hp.SetTakeDamage(damage);
        }
    }

    public void Initialize(int damage, GameObject owner)
    {
        this.damage = damage;
        this.owner = owner;

        HitBoxDirection();
    }

    private void HitBoxDirection()
    {
        boxCollider2D.size = hitBoxSize;
    }
}
