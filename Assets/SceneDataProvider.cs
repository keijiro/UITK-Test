using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using Unity.Properties;

public sealed class SceneDataProvider : MonoBehaviour
{
    [CreateProperty]
    public List<string> ObjectList => EnumerateRigidBodies().ToList();

    [CreateProperty]
    public string ObjectInformation => GetObjectInformationString();

    [field:SerializeField]
    public UIDocument Document { get; set; }

    [field:SerializeField]
    public string DropdownName { get; set; }

    [field:SerializeField]
    public string LabelName { get; set; }

    GameObject _selected;

    IEnumerable<string> EnumerateRigidBodies()
      => FindObjectsByType<Rigidbody>(FindObjectsSortMode.InstanceID).Select(x => x.name);

    string GetObjectInformationString()
    {
        if (_selected == null) return "";
        return $"{_selected.name}:\n" +
               $" - Position {_selected.transform.position}\n" +
               $" - Rotation {_selected.transform.eulerAngles}";
    }

    void OnDropdownChange(ChangeEvent<string> evt)
      => _selected = GameObject.Find(evt.newValue);

    void Start()
    {
        var root = Document.rootVisualElement;
        var dropdown = root.Q(DropdownName);
        var label = root.Q(LabelName);

        dropdown.dataSource = this;
        label.dataSource = this;

        dropdown.RegisterCallback<ChangeEvent<string>>(OnDropdownChange);
    }
}
