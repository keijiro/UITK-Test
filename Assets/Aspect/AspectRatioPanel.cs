// Aspect Ratio Preserving Panel originally written by pbhogan
// https://gist.github.com/pbhogan/2094a033c094ddd1b0b8f37a5dffd005
//
// Updated for Unity 6 by Keijiro Takahashi
//
// Released into the public domain

using UnityEngine;
using UnityEngine.UIElements;

namespace GallantGames.UI {

[UxmlElement]
public partial class AspectRatioPanel : VisualElement
{
    [UxmlAttribute, Range(1, 100)]
    public int aspectRatioX { get; set; } = 16;

    [UxmlAttribute, Range(1, 100)]
    public int aspectRatioY { get; set; } = 9;

    [UxmlAttribute, Range(0, 100)]
    public int balanceX { get; set; } = 50;

    [UxmlAttribute, Range(0, 100)]
    public int balanceY { get; set; } = 50;

    public AspectRatioPanel()
    {
        ApplyBaseStyle();
        RegisterCallback<AttachToPanelEvent>(OnAttachToPanelEvent);
    }

    void OnAttachToPanelEvent(AttachToPanelEvent e)
    {
        parent?.RegisterCallback<GeometryChangedEvent>(OnGeometryChangedEvent);
        FitToParent();
    }

    void OnGeometryChangedEvent(GeometryChangedEvent e)
      => FitToParent();

    void ApplyBaseStyle()
    {
        style.position = Position.Absolute;
        style.left = 0;
        style.top = 0;
        style.right = StyleKeyword.Undefined;
        style.bottom = StyleKeyword.Undefined;
    }

    void FitToParent()
    {
        ApplyBaseStyle();

        if (parent == null) return;
        var parentW = parent.resolvedStyle.width;
        var parentH = parent.resolvedStyle.height;
        var ratio = Mathf.Min(parentW / aspectRatioX, parentH / aspectRatioY);

        var targetW = Mathf.Floor(aspectRatioX * ratio);
        var targetH = Mathf.Floor(aspectRatioY * ratio);
        style.width = targetW;
        style.height = targetH;

        var marginX = parentW - targetW;
        var marginY = parentH - targetH;
        style.left = Mathf.Floor(marginX * balanceX / 100.0f);
        style.top = Mathf.Floor(marginY * balanceY / 100.0f);
    }
}

} // namespace GallantGames.UI