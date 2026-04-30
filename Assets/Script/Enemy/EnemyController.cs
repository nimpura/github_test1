using System.Collections;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("Ref")]
    [SerializeField] private SPUM_Prefabs spum;
    [SerializeField] private EnemyDetect enemyDetect;
    [SerializeField] private HPManager hpManager;
    [SerializeField] private MeleeHitBoxAttack meleeHitBoxAttack;
    [SerializeField] private GameObject hpBarPrefab;

    [Header("Stats")]
    [SerializeField] private float moveSpeed = 5.0f;

    [Header("DeadAction DelayTime")]
    [SerializeField] private float DelayTime = 0.3f;

    private Transform moveTarget;
    private PlayerState currentState;
    private GameObject hpBarInstance;
    private Transform uiCanvas;

    private void Awake()
    {
        if (spum == null) spum = GetComponentInChildren<SPUM_Prefabs>();
        if (enemyDetect == null) enemyDetect = GetComponentInChildren<EnemyDetect>();
        if (hpManager == null) hpManager = GetComponent<HPManager>();
        if (meleeHitBoxAttack == null) meleeHitBoxAttack = GetComponent<MeleeHitBoxAttack>();
        if (uiCanvas == null) uiCanvas = GameObject.Find("Canvas_Game").transform;
    }

    private void Start()
    {
        spum.PopulateAnimationLists();
        spum.OverrideControllerInit();

        hpManager.GetFullHP();

        if (GameManager.Instance == null || GameManager.Instance.mainBuilding == null)
        {
            Debug.LogError("GameManager 또는 mainBuilding 없음");
            return;
        }

        CreateHPBar();
    }

    private void Update()
    {
        Move();
    }

    private void OnEnable()
    {
        if (enemyDetect != null)
        {
            enemyDetect.OnTargetChanged += MoveToTarget;
        }

        if (meleeHitBoxAttack != null)
        {
            meleeHitBoxAttack.OnAttack += Attack;
        }

        if(hpManager != null)
        {
            hpManager.OnDamaged += Damaged;
            hpManager.OnDead += Dead;
        }
    }

    private void OnDisable()
    {
        if (enemyDetect != null)
        {
            enemyDetect.OnTargetChanged -= MoveToTarget;
        }

        if (meleeHitBoxAttack != null)
        {
            meleeHitBoxAttack.OnAttack -= Attack;
        }

        if (hpManager != null)
        {
            hpManager.OnDamaged -= Damaged;
            hpManager.OnDead -= Dead;
        }
    }

    private void MoveToTarget(Transform target)
    {
        moveTarget = target;
    }

    private void Move()
    {
        Transform target = enemyDetect.GetCurrentTargetPosition;

        if (target == null) return;

        moveTarget = target;

        transform.position = Vector2.MoveTowards(transform.position, moveTarget.position, moveSpeed * Time.deltaTime);

        float dir = moveTarget.position.x - transform.position.x;
        spum.transform.localScale = new Vector3(dir > 0 ? -1 : 1, 1, 1);

        ChangeState(PlayerState.MOVE, 0);
    }

    private void Attack()
    {
        ChangeState(PlayerState.ATTACK, 0);
    }

    private void Dead()
    {
        WaveManager.Instance.OnEnemyDead();

        if (hpBarInstance != null)
        {
            Destroy(hpBarInstance);
        }

        StartCoroutine(DeadAction());
    }

    IEnumerator DeadAction()
    {
        ChangeState(PlayerState.DEATH, 0);

        yield return new WaitForSeconds(DelayTime);

        //TODO : 값을 초기화하고 처음 스폰된 것처럼 할 수 있으면... 

        Destroy(gameObject);
    }

    private void Damaged()
    {
        ChangeState(PlayerState.DAMAGED, 0);
    }

    private void ChangeState(PlayerState newState, int i)
    {
        if (currentState == newState) return;

        if (!spum.StateAnimationPairs.ContainsKey(newState.ToString()))
        {
            Debug.LogError($"Animation State 없음: {newState}");
            return;
        }

        currentState = newState;
        spum.PlayAnimation(newState, i);
    }

    private void CreateHPBar()
    {
        if (hpBarPrefab == null || uiCanvas == null)
        {
            Debug.LogError("hpBarPrefab or uiCanvas NULL");
            return;
        }

        hpBarInstance = Instantiate(hpBarPrefab, uiCanvas);

        UI_HPBar hpBar = hpBarInstance.GetComponent<UI_HPBar>();

        if (hpBar != null)
        {
            hpBar.SetTarget(hpManager);
        }

        UI_Follow follow = hpBarInstance.GetComponent<UI_Follow>();

        if (follow != null)
        {
            follow.SetTarget(transform);
        }
    }
}
