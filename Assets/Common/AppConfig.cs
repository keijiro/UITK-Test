using UnityEngine;

public sealed class AppConfig : MonoBehaviour
{
    void Start()
    {
#if UNITY_IOS && !UNITY_EDITOR
        Application.targetFrameRate = 60;
#endif
    }
}
