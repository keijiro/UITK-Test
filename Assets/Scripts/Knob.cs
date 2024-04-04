using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;

[UxmlElement]
public partial class Knob : BaseField<float>
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

    public static readonly new string ussClassName = "knob";
    public static readonly new string inputUssClassName = "knob__input";

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
    VisualElement _input;

    #endregion

    #region Mouse drag callbacks

    (float position, float value)? _draggedFrom;

    void OnMouseDown(MouseDownEvent evt)
    {
        MouseCaptureController.CaptureMouse(this);
        _draggedFrom = (evt.mousePosition.y, value);
    }

    void OnMouseMove(MouseMoveEvent evt)
    {
        if (_draggedFrom == null) return;
        var (origin, baseValue) = ((float, float))_draggedFrom;
        var delta = (origin - evt.mousePosition.y) * sensitivity / 100;
        var range = highValue - lowValue;
        this.value = math.clamp(baseValue + delta * range, lowValue, highValue);
        MarkDirtyRepaint();
    }

    void OnMouseUp(MouseUpEvent evt)
    {
        if (_draggedFrom == null) return;
        MouseCaptureController.ReleaseMouse(this);
        _draggedFrom = null;
    }

    #endregion

    #region Visual element implementation

    public Knob() : this(null) {}

    public Knob(string label) : base(label, new())
    {
        AddToClassList(ussClassName);

        _input = this.Q(className: BaseField<float>.inputUssClassName);
        _input.AddToClassList(inputUssClassName);
        Add(_input);

        labelElement.AddToClassList("knob__label");

        RegisterCallback<CustomStyleResolvedEvent>(UpdateCustomStyles);
        RegisterCallback<MouseDownEvent>(OnMouseDown);
        RegisterCallback<MouseMoveEvent>(OnMouseMove);
        RegisterCallback<MouseUpEvent>(OnMouseUp);

        generateVisualContent += GenerateVisualContent;
    }

    void UpdateCustomStyles(CustomStyleResolvedEvent _)
    {
        var dirty = false;
        dirty |= customStyle.TryGetValue(_trackWidthProp, out _trackWidth);
        dirty |= customStyle.TryGetValue(_trackColorProp, out _trackColor);
        dirty |= customStyle.TryGetValue(_valueColorProp, out _valueColor);
        if (dirty) MarkDirtyRepaint();
    }

    void GenerateVisualContent(MeshGenerationContext context)
    {
        var rect = _input.contentRect;
        var center = (float2)_input.ChangeCoordinatesTo(this, rect.center);
        var radius = math.min(rect.width, rect.height) / 2 - _trackWidth / 2;

        var tip_deg = 136 + 269 * (value - lowValue) / (highValue - lowValue);
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
    }

    #endregion
}
