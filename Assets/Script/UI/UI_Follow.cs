using UnityEngine;

public class UI_Follow : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset = new Vector3(0, 1.5f, 0);
    [SerializeField] private Camera cam;

    private void Awake()
    {
        if (cam == null) cam = Camera.main;
    }

    public void SetTarget(Transform t)
    {
        target = t;
        Debug.Log("Å¸°Ù ¼³Á¤µÊ: " + t.name);
    }

    private void LateUpdate()
    {
        if (target == null)
        {
            Debug.LogWarning("target ¾øÀ½!");
            return;
        }

        if (cam == null)
        {
            Debug.LogWarning("camera ¾øÀ½!");
            return;
        }

        Vector3 pos = cam.WorldToScreenPoint(target.position + offset);
        transform.position = pos;
    }
}
