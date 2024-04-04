using UnityEngine;
using UnityEngine.UIElements;
using Unity.Properties;

public sealed class KnobTest : MonoBehaviour
{
    [CreateProperty]
    public string Parameter1 { get; set; }

    [CreateProperty]
    public string Parameter2 { get; set; }

    [CreateProperty]
    public string Parameter3 { get; set; }

    void Start()
      => GetComponent<UIDocument>().rootVisualElement.dataSource = this;
}
