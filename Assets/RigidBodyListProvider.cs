using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using Unity.Properties;

public sealed class RigidBodyListProvider : MonoBehaviour
{
    [CreateProperty]
    public List<string> ObjectList => EnumerateRigidBodies().ToList();

    [field:SerializeField]
    public UIDocument Document { get; set; }

    [field:SerializeField]
    public string Element { get; set; }

    IEnumerable<string> EnumerateRigidBodies()
      => FindObjectsByType<Rigidbody>(FindObjectsSortMode.InstanceID).Select(x => x.name);

    void Start()
      => Document.rootVisualElement.Q(Element).dataSource = this;
}