using UnityEngine;
using UnityEngine.UIElements;

namespace VJUI {

[UxmlElement]
public partial class VJToggle : BaseField<bool>
{
    #region Static public property

    public static readonly new string ussClassName = "vj-toggle";
    public static readonly new string labelUssClassName = "vj-toggle__label";
    public static readonly new string inputUssClassName = "vj-toggle__input";

    #endregion

    #region Style properties

    static CustomStyleProperty<int> _lineWidthProp
      = new CustomStyleProperty<int>("--line-width");

    int _lineWidth = 10;

    #endregion

    #region Visual element implementation

    VisualElement _input;

    public VJToggle() : this(null) {}

    public VJToggle(string label) : base(label, new())
    {
        // This element
        AddToClassList(ussClassName);
        focusable = false;

        // Label element
        labelElement.AddToClassList(labelUssClassName);

        // Input element
        _input = this.Q(className: BaseField<bool>.inputUssClassName);
        _input.AddToClassList(inputUssClassName);
        Add(_input);

        // Input element callbacks
        _input.RegisterCallback<CustomStyleResolvedEvent>(UpdateCustomStyles);
        _input.generateVisualContent += GenerateVisualContent;
    }

    void UpdateCustomStyles(CustomStyleResolvedEvent evt)
    {
        if (evt.customStyle.TryGetValue(_lineWidthProp, out _lineWidth))
            MarkDirtyRepaint();
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

        painter.strokeColor = context.visualElement.resolvedStyle.color;
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
