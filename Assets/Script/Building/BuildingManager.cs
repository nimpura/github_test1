using System.Collections;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    [Header("Ref")]
    [SerializeField] private HPManager hpManager;
    [SerializeField] private GameObject Skin;
    [SerializeField] private GameObject Collider;

    [Header("DelayTime")]
    [SerializeField] private float destructDelayTime = 1f;

    private void Awake()
    {
        if(hpManager == null) hpManager = GetComponent<HPManager>();
    }

    private void Start()
    {
        hpManager.GetFullHP();
    }

    private void OnEnable()
    {
        if (hpManager != null)
        {
            hpManager.OnDead += Destruction;
        }
    }

    private void OnDisable()
    {
        if (hpManager != null)
        {
            hpManager.OnDead -= Destruction;
        }
    }

    private void Destruction()
    {
        StartCoroutine(DestructAndRoadScene());
    }

    IEnumerator DestructAndRoadScene()
    {
        Skin.SetActive(false);
        Collider.SetActive(false);

        //TODO: 건물이 파괴되는 소리 & 패배 후 해당 씬으로 넘어가기 전에 패배화면효과연출

        yield return new WaitForSeconds(destructDelayTime);

        GameSceneManager.Instance.LoadSceneByName("GameOver");
    }
}
