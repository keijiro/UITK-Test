using UnityEngine;
using UnityEngine.UIElements;

namespace VJUI {

[UxmlElement]
public partial class VJToggle : BaseBoolField
{
    #region Static public property

    public static readonly new string ussClassName = "vj-toggle";
    public static readonly new string labelUssClassName = "vj-toggle__label";
    public static readonly new string inputUssClassName = "vj-toggle__input";
    public static readonly new string checkmarkUssClassName = "vj-toggle__checkmark";

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

    public VJToggle() : this(null) {}

    public VJToggle(string label) : base(label)
    {
        // This element
        AddToClassList(ussClassName);

        // Label element
        labelElement.AddToClassList(labelUssClassName);

        // Input element
        _input = this.Q(className: BaseField<bool>.inputUssClassName);
        _input.AddToClassList(inputUssClassName);

        // Checkmark element
        var checkmark = this.Q("unity-checkmark");
        checkmark.AddToClassList(checkmarkUssClassName);

        // Input element callbacks
        _input.RegisterCallback<CustomStyleResolvedEvent>(UpdateCustomStyles);
        _input.generateVisualContent += GenerateVisualContent;
    }

    void UpdateCustomStyles(CustomStyleResolvedEvent evt)
    {
        var (style, dirty) = (evt.customStyle, false);
        dirty |= style.TryGetValue(_lineWidthProp, out _lineWidth);
        dirty |= style.TryGetValue(_secondaryColorProp, out _secondaryColor);
        if (dirty) MarkDirtyRepaint();
    }

    #endregion

    #region Visual construction callback

    void GenerateVisualContent(MeshGenerationContext context)
    {
        var center = context.visualElement.contentRect.center;
        var outer = Mathf.Min(center.x, center.y) - _lineWidth / 2;
        var inner = outer - _lineWidth;

        var painter = context.painter2D;
        painter.lineWidth = _lineWidth;

        painter.strokeColor = _secondaryColor;
        painter.BeginPath();
        painter.Arc(center, outer, 0, 360);
        painter.Stroke();

        painter.fillColor = context.visualElement.resolvedStyle.color;
        painter.lineWidth = 0;
        painter.BeginPath();
        painter.Arc(center, inner, 0, 360);
        painter.Fill();
    }

    #endregion
}

} // namespace VJUI
