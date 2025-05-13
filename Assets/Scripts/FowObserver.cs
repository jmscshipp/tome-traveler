using UnityEngine;

public class FowObserver : MonoBehaviour
{
    void FowUpdate()
    {
        transform.localScale = transform.localScale.normalized * GameManager.GetFogOfWarRadius();
    }

    private void Awake()
    {
        GameManager.FoWUpdate += FowUpdate;
    }

    private void OnDestroy()
    {
        GameManager.FoWUpdate -= FowUpdate;
    }
}
