using UnityEngine;
using UnityEngine.UIElements;
using Unity.Properties;

public sealed class TransformDataProvider : MonoBehaviour
{
    [CreateProperty]
    public string XformAsString
      => $"{name}:\n - Position {transform.position}\n - Rotation {transform.eulerAngles}";

    [field:SerializeField]
    public UIDocument Document { get; set; }

    [field:SerializeField]
    public string Element { get; set; }

    void Start()
      => Document.rootVisualElement.Q<Label>(Element).dataSource = this;
}
