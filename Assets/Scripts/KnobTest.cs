using UnityEngine;
using UnityEngine.UIElements;
using Unity.Properties;

#if UNITY_EDITOR
using UnityEditor;
#endif

public sealed class KnobTest : MonoBehaviour
{
    [CreateProperty]
    public float Parameter1 { get; set; }

    [CreateProperty]
    public float Parameter2 { get; set; }

    [CreateProperty]
    public float Parameter3 { get; set; }

    [CreateProperty]
    public bool Toggle1 { get; set; }

    [field:SerializeField]
    public Transform Target { get; set; } = null;

    void Start()
      => GetComponent<UIDocument>().rootVisualElement.dataSource = this;

    void LateUpdate()
      => Target.localRotation =
           Quaternion.Euler(Parameter1, Parameter2, Parameter3);

#if UNITY_EDITOR
    [InitializeOnLoadMethod]
#else
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
#endif
    public static void RegisterConverters()
    {
        var grp = new ConverterGroup("Knob 0-100");
        grp.AddConverter((ref float v) => $"{v:0.00}");
        ConverterGroups.RegisterConverterGroup(grp);
    }
}
