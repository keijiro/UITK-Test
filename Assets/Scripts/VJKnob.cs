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

    static CustomStyleProperty<Color> _primaryColorProp
      = new CustomStyleProperty<Color>("--primary-color");

    static CustomStyleProperty<Color> _secondaryColorProp
      = new CustomStyleProperty<Color>("--secondary-color");

    Color _primaryColor = Color.white;
    Color _secondaryColor = Color.gray;
    int _lineWidth = 10;

    #endregion

    #region Visual element implementation

    VisualElement _input;

    public VJKnob() : this(null) {}

    public VJKnob(string label) : base(label, new())
    {
        // This element
        AddToClassList(ussClassName);
        focusable = false;

        // Label element
        labelElement.AddToClassList(labelUssClassName);

        // Input element
        _input = this.Q(className: BaseField<float>.inputUssClassName);
        _input.AddToClassList(inputUssClassName);
        Add(_input);

        // Input element callbacks
        _input.RegisterCallback<CustomStyleResolvedEvent>(UpdateCustomStyles);
        _input.RegisterCallback<PointerDownEvent>(OnPointerDown);
        _input.RegisterCallback<PointerMoveEvent>(OnPointerMove);
        _input.RegisterCallback<PointerUpEvent>(OnPointerUp);
        _input.generateVisualContent += GenerateVisualContent;
    }

    void UpdateCustomStyles(CustomStyleResolvedEvent evt)
    {
        var (style, dirty) = (evt.customStyle, false);
        dirty |= style.TryGetValue(_lineWidthProp, out _lineWidth);
        dirty |= style.TryGetValue(_primaryColorProp, out _primaryColor);
        dirty |= style.TryGetValue(_secondaryColorProp, out _secondaryColor);
        if (dirty) MarkDirtyRepaint();
    }

    #endregion

    #region Pointer callbacks

    (float position, float value)? _draggedFrom;

    void OnPointerDown(PointerDownEvent evt)
    {
        PointerCaptureHelper.CapturePointer(_input, evt.pointerId);
        _draggedFrom = (evt.position.y, value);
    }

    void OnPointerMove(PointerMoveEvent evt)
    {
        if (_draggedFrom == null) return;
        var (origin, baseValue) = ((float, float))_draggedFrom;
        var delta = (origin - evt.position.y) * sensitivity / 100;
        var range = highValue - lowValue;
        value = Mathf.Clamp(baseValue + delta * range, lowValue, highValue);
        _input.MarkDirtyRepaint();
    }

    void OnPointerUp(PointerUpEvent evt)
    {
        if (_draggedFrom == null) return;
        PointerCaptureHelper.ReleasePointer(_input, evt.pointerId);
        _draggedFrom = null;
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

        painter.strokeColor = _primaryColor;
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
