using UnityEngine;
using UnityEngine.UIElements;

namespace VJUI {

[UxmlElement]
public partial class VJButton : BaseField<bool>
{
    #region Static public property

    public static readonly new string ussClassName = "vj-button";
    public static readonly new string labelUssClassName = "vj-button__label";
    public static readonly new string inputUssClassName = "vj-button__input";

    #endregion

    #region Style properties

    static CustomStyleProperty<Color> _secondaryColorProp
      = new CustomStyleProperty<Color>("--secondary-color");

    Color _secondaryColor = Color.gray;

    #endregion

    #region Visual element implementation

    VisualElement _input;

    public VJButton() : this(null) {}

    public VJButton(string label) : base(label, new())
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
        if (evt.customStyle.TryGetValue(_secondaryColorProp, out _secondaryColor))
            MarkDirtyRepaint();
    }

    #endregion

    #region Visual construction callback

    void GenerateVisualContent(MeshGenerationContext context)
    {
        var center = context.visualElement.contentRect.center;
        var radius = Mathf.Min(center.x, center.y);

        var painter = context.painter2D;

        painter.fillColor = _secondaryColor;
        painter.BeginPath();
        painter.Arc(center, radius, 0, 360);
        painter.Fill();
    }

    #endregion
}

} // namespace VJUI
