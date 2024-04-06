using UnityEngine;
using UnityEngine.UIElements;

namespace VJUI {

[UxmlElement]
public partial class VJKnob : BaseField<float>
{
    #region Public property

    [UxmlAttribute]
    public float lowValue { get; set; } = 0;

    [UxmlAttribute]
    public float highValue { get; set; } = 100;

    [UxmlAttribute]
    public float sensitivity { get; set; } = 1;

    #endregion

    #region Static public property

    public static readonly new string ussClassName = "vj-knob";
    public static readonly new string labelUssClassName = "vj-knob__label";
    public static readonly new string inputUssClassName = "vj-knob__input";

    #endregion

    #region Style properties

    static CustomStyleProperty<int> _lineWidthProp
      = new CustomStyleProperty<int>("--line-width");

    static CustomStyleProperty<Color> _secondaryColorProp
      = new CustomStyleProperty<Color>("--secondary-color");

    int _lineWidth = 10;
    Color _secondaryColor = Color.gray;

    #endregion

    #region Visual element implementation

    VisualElement _input;

    public VJKnob() : this(null) {}

    public VJKnob(string label) : base(label, new())
    {
        // This element
        AddToClassList(ussClassName);

        // Label element
        labelElement.AddToClassList(labelUssClassName);

        // Input element
        _input = this.Q(className: BaseField<float>.inputUssClassName);
        _input.AddToClassList(inputUssClassName);

        // Input element callbacks
        _input.RegisterCallback<CustomStyleResolvedEvent>(UpdateCustomStyles);
        _input.generateVisualContent += GenerateVisualContent;

        // Manipulation by mouse drag
        _input.AddManipulator(new VJKnobDragger(this));
    }

    void UpdateCustomStyles(CustomStyleResolvedEvent evt)
    {
        var (style, dirty) = (evt.customStyle, false);
        dirty |= style.TryGetValue(_lineWidthProp, out _lineWidth);
        dirty |= style.TryGetValue(_secondaryColorProp, out _secondaryColor);
        if (dirty) MarkDirtyRepaint();
    }

    public override void SetValueWithoutNotify(float newValue)
    {
        newValue = Mathf.Clamp(newValue, lowValue, highValue);
        base.SetValueWithoutNotify(newValue);
        _input.MarkDirtyRepaint();
    }

    #endregion

    #region Visual construction callback

    void GenerateVisualContent(MeshGenerationContext context)
    {
        var center = context.visualElement.contentRect.center;
        var radius = Mathf.Min(center.x, center.y) - _lineWidth / 2;

        var tip_deg = 120 + 300 * (value - lowValue) / (highValue - lowValue);
        var tip_rad = Mathf.Deg2Rad * tip_deg;
        var tip_vec = new Vector2(Mathf.Cos(tip_rad), Mathf.Sin(tip_rad));

        var painter = context.painter2D;
        painter.lineWidth = _lineWidth;
        painter.lineCap = LineCap.Round;

        painter.strokeColor = _secondaryColor;
        painter.BeginPath();
        painter.Arc(center, radius, 120, 120 + 300);
        painter.Stroke();

        painter.strokeColor = context.visualElement.resolvedStyle.color;
        painter.BeginPath();
        painter.Arc(center, radius, 120, tip_deg);
        painter.Stroke();

        painter.BeginPath();
        painter.MoveTo(center + tip_vec * radius / 2);
        painter.LineTo(center + tip_vec * radius);
        painter.Stroke();
    }

    #endregion
}

} // namespace VJUI
