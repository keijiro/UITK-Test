using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using Unity.Properties;

public sealed class KnobTest : MonoBehaviour
{
    [CreateProperty]
    public float Parameter1 { get; set; }

    [CreateProperty]
    public float Parameter2 { get; set; }

    [CreateProperty]
    public float Parameter3 { get; set; }

    [field:SerializeField]
    public Transform Target { get; set; } = null;

    void Start()
      => GetComponent<UIDocument>().rootVisualElement.dataSource = this;

    [InitializeOnLoadMethod]
    public static void RegisterConverters()
    {
        var grp = new ConverterGroup("Knob 0-100");
        grp.AddConverter((ref float v) => $"{v:0.00}");
        ConverterGroups.RegisterConverterGroup(grp);
    }

    void LateUpdate()
      => Target.localRotation =
           Quaternion.Euler(Parameter1, Parameter2, Parameter3);
}
