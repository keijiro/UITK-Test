using UnityEngine;
using UnityEngine.UIElements;

namespace VJUI {

public sealed class VJClicker : PointerManipulator
{
    #region Private variables

    VJToggle _toggle;
    int _pointerID;

    bool IsActive => _pointerID >= 0;

    #endregion

    #region PointerManipulator implementation

    public VJClicker(VJToggle toggle)
    {
        (_toggle, _pointerID) = (toggle, -1);
        activators.Add(new ManipulatorActivationFilter{button = MouseButton.LeftMouse});
    }

    protected override void RegisterCallbacksOnTarget()
    {
        target.RegisterCallback<PointerDownEvent>(OnPointerDown);
        target.RegisterCallback<PointerUpEvent>(OnPointerUp);
    }

    protected override void UnregisterCallbacksFromTarget()
    {
        target.UnregisterCallback<PointerDownEvent>(OnPointerDown);
        target.UnregisterCallback<PointerUpEvent>(OnPointerUp);
    }

    #endregion

    #region Pointer callbacks

    void OnPointerDown(PointerDownEvent e)
    {
        if (IsActive)
        {
            e.StopImmediatePropagation();
        }
        else if (CanStartManipulation(e))
        {
            target.CapturePointer(_pointerID = e.pointerId);
            e.StopPropagation();
        }
    }

    void OnPointerUp(PointerUpEvent e)
    {
        if (!IsActive || !target.HasPointerCapture(_pointerID)) return;

        if (CanStopManipulation(e))
        {
            _toggle.value ^= true;
            _pointerID = -1;
            target.ReleaseMouse();
            e.StopPropagation();
        }
    }

    #endregion
}

} // namespace VJUI
