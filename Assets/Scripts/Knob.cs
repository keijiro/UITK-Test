using UnityEngine;
using UnityEngine.UIElements;

[UxmlElement]
public partial class Knob : VisualElement
{
    #region Public property

    [UxmlAttribute, Range(0, 1)]
    public float value { get; set; } = 0.5f;

    #endregion

    #region Static public property

    static string ussClassName => "knob";
    static string ussLabelClassName => "knob__label";

    #endregion

    #region Style properties

    static CustomStyleProperty<Color> _trackColorProp
      = new CustomStyleProperty<Color>("--track-color");

    static CustomStyleProperty<Color> _valueColorProp
      = new CustomStyleProperty<Color>("--value-color");

    #endregion

    #region Runtime members

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
        if (customStyle.TryGetValue(_valueColorProp, out _valueColor) ||
            customStyle.TryGetValue(_trackColorProp, out _trackColor))
            MarkDirtyRepaint();
    }

    void GenerateVisualContent(MeshGenerationContext context)
    {
        var (w, h) = (contentRect.width / 2, contentRect.height / 2);

        var painter = context.painter2D;
        painter.lineWidth = 10.0f;
        painter.lineCap = LineCap.Butt;

        painter.strokeColor = _trackColor;
        painter.BeginPath();
        painter.Arc(new Vector2(w, h), w, 0, 360);
        painter.Stroke();

        painter.strokeColor = _valueColor;
        painter.BeginPath();
        painter.Arc(new Vector2(w, h), w, 90, 360 * value - 90);
        painter.Stroke();
    }

    #endregion
}
