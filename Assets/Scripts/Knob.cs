using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;

[UxmlElement]
public partial class Knob : VisualElement
{
    #region Public property

    [UxmlAttribute, Range(0, 1)]
    public float value { get; set; } = 0.5f;

    [UxmlAttribute]
    public string label { get; set; } = "Knob";

    #endregion

    #region Static public property

    static string ussClassName => "knob";
    static string ussLabelClassName => "knob__label";

    #endregion

    #region Style properties

    static CustomStyleProperty<int> _trackWidthProp
      = new CustomStyleProperty<int>("--track-width");

    static CustomStyleProperty<Color> _trackColorProp
      = new CustomStyleProperty<Color>("--track-color");

    static CustomStyleProperty<Color> _valueColorProp
      = new CustomStyleProperty<Color>("--value-color");

    #endregion

    #region Runtime members

    int _trackWidth = 10;
    Color _trackColor = Color.gray;
    Color _valueColor = Color.red;
    Label _label;

    #endregion

    #region Visual element implementation

    public Knob()
    {
        _label = new Label();
        _label.AddToClassList(ussLabelClassName);
        Add(_label);

        AddToClassList(ussClassName);

        RegisterCallback<CustomStyleResolvedEvent>
          (e => (e.currentTarget as Knob).UpdateCustomStyles());

        generateVisualContent += GenerateVisualContent;
    }

    void UpdateCustomStyles()
    {
        var dirty = false;
        dirty |= customStyle.TryGetValue(_trackWidthProp, out _trackWidth);
        dirty |= customStyle.TryGetValue(_trackColorProp, out _trackColor);
        dirty |= customStyle.TryGetValue(_valueColorProp, out _valueColor);
        if (dirty) MarkDirtyRepaint();
    }

    void GenerateVisualContent(MeshGenerationContext context)
    {
        var radius = math.min(contentRect.width, contentRect.height) / 2;
        var center = math.float2(contentRect.width / 2, radius);
        radius -= _trackWidth / 2;

        var tip_deg = 136 + 269 * value;
        var tip_rad = math.radians(tip_deg);
        var tip_vec = math.float2(math.cos(tip_rad), math.sin(tip_rad));

        var painter = context.painter2D;
        painter.lineWidth = _trackWidth;
        painter.lineCap = LineCap.Round;

        painter.strokeColor = _trackColor;
        painter.BeginPath();
        painter.Arc(center, radius, 135, 135 + 270);
        painter.Stroke();

        painter.strokeColor = _valueColor;
        painter.BeginPath();
        painter.Arc(center, radius, 135, tip_deg);
        painter.Stroke();

        painter.BeginPath();
        painter.MoveTo(center + tip_vec * radius / 2);
        painter.LineTo(center + tip_vec * radius);
        painter.Stroke();

        _label.text = label;
    }

    #endregion
}
