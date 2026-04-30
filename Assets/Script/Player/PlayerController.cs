using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PlayerController : MonoBehaviour
{
    [Header("Ref")]
    [SerializeField] private SPUM_Prefabs spum;
    [SerializeField] private PlayerMove2D playerMove2D;
    [SerializeField] private PlayerInputReader inputReader;
    [SerializeField] private MeleeHitBoxAttack meleeHitBoxAttack;
    [SerializeField] private HPManager hpManager;
    [SerializeField] private GameObject hurtbox;
    [SerializeField] private GameObject hpBarPrefab;

    [Header("Tuning")]
    [Tooltip("이 값보다 작은 입력은 정지로 간주한다.")]
    [SerializeField] private float inputDeadZone = 0.1f;

    [Header("Ghost")]
    [SerializeField] private float respawnTime = 3f;
    private bool isGhost = false;
    private PlayerState currentState;
    private GameObject hpBarInstance;

    public Vector2 CurrentInputDirection { get; private set; }

    public bool IsMoving { get; private set; }




    private void Awake()
    {
        if (spum == null) spum = GetComponentInChildren<SPUM_Prefabs>();
        if (playerMove2D == null) playerMove2D = GetComponent<PlayerMove2D>();
        if (inputReader == null) inputReader = GetComponent<PlayerInputReader>();
        if (meleeHitBoxAttack == null) meleeHitBoxAttack = GetComponent<MeleeHitBoxAttack>();
        if (hpManager == null) hpManager = GetComponent<HPManager>();
        if (hurtbox == null)
        {
            Transform t = transform.Find("HurtBox");
            if (t != null) hurtbox = t.gameObject;
        }

        Time.timeScale = 1f;
    }


    private void Start()
    {
        spum.PopulateAnimationLists();
        spum.OverrideControllerInit();

        hpManager.GetFullHP();

        CreateHPBar();
    }

    private void Update()
    {
        if (spum == null || inputReader == null || playerMove2D == null) return;

        Vector2 rawInput = inputReader.MoveVector;
        CurrentInputDirection = rawInput;

        playerMove2D.Move(CurrentInputDirection);

        IsMoving = rawInput.sqrMagnitude >= inputDeadZone * inputDeadZone;

        if (IsMoving)
        {
            spum.transform.localScale = new Vector3(rawInput.x > 0 ? -1 : 1, 1, 0);
            ChangeState(PlayerState.MOVE, 0);
        }
        else
        {
            ChangeState(PlayerState.IDLE, 0);
        }
    }

    private void OnEnable()
    {
        if (meleeHitBoxAttack != null)
        {
            meleeHitBoxAttack.OnAttack += Attack;
        }

        if (hpManager != null)
        {
            hpManager.OnDamaged += Damaged;
            hpManager.OnDead += Dead;
        }
    }

    private void OnDisable()
    {
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

    private void Attack()
    {
        ChangeState(PlayerState.ATTACK, 0);
    }

    private void Damaged()
    {
        ChangeState(PlayerState.DAMAGED, 0);
    }

    private void Dead()
    {
        if (isGhost) return;

        if (hpBarInstance != null)
        {
            hpBarInstance.SetActive(false);
        }

        ChangeState(PlayerState.DEATH, 0);
        hurtbox.SetActive(false);
        isGhost = true;

        StartCoroutine(GhostState());
    }


    IEnumerator GhostState()
    {
        meleeHitBoxAttack.enabled = false;

        yield return new WaitForSeconds(respawnTime);

        Revive();
    }

    private void Revive()
    {
        hpManager.GetFullHP();

        if (hpBarInstance != null)
        {
            hpBarInstance.SetActive(true);
        }

        hurtbox.SetActive(true);
        meleeHitBoxAttack.enabled = true;

        isGhost = false;
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


    //2차 빌드 임시코드
    public void ApplyUpgrade(UpgradeData data)
    {
        switch (data.type)
        {
            case UpgradeType.Damage:
                meleeHitBoxAttack.GetComponent<DamageManager>().IncreaseDamage((int)data.value);
                break;

            case UpgradeType.AttackSpeed:
                meleeHitBoxAttack.GetComponent<AttackManager>().ReduceCooldown(data.value);
                break;

            case UpgradeType.MaxHP:
                hpManager.IncreaseMaxHP((int)data.value);
                break;
        }
    }

    private void CreateHPBar()
    {
        Transform uiCanvas = GameObject.Find("Canvas_Game").transform;

        hpBarInstance = Instantiate(hpBarPrefab, uiCanvas);


        hpBarInstance.transform.localScale = new Vector3(1.5f, 1.5f, 1f);

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
