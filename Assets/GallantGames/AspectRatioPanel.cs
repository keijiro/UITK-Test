/*
This is free and unencumbered software released into the public
domain.

Anyone is free to copy, modify, publish, use, compile, sell, or
distribute this software, either in source code form or as a compiled
binary, for any purpose, commercial or non-commercial, and by any
means.

In jurisdictions that recognize copyright laws, the author or authors
of this software dedicate any and all copyright interest in the
software to the public domain. We make this dedication for the
benefit of the public at large and to the detriment of our heirs and
successors. We intend this dedication to be an overt act of
relinquishment in perpetuity of all present and future rights to this
software under copyright law.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS BE LIABLE FOR ANY
CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
*/

using Unity.Collections;
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