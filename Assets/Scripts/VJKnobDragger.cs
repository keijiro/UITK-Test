using UnityEngine;
using UnityEngine.UIElements;

namespace VJUI {

public class VJKnobDragger : PointerManipulator
{
    #region Private variables

    VJKnob _knob;
    int _pointerID;
    Vector3 _startPosition;
    float _startValue;

    #endregion

    #region PointerManipulator implementation

    public VJKnobDragger(VJKnob knob)
    {
        _knob = knob;
        _pointerID = -1;

        activators.Add(new ManipulatorActivationFilter
                         { button = MouseButton.LeftMouse });
    }

    protected override void RegisterCallbacksOnTarget()
    {
        target.RegisterCallback<PointerDownEvent>(OnPointerDown);
        target.RegisterCallback<PointerMoveEvent>(OnPointerMove);
        target.RegisterCallback<PointerUpEvent>(OnPointerUp);
    }

    protected override void UnregisterCallbacksFromTarget()
    {
        target.UnregisterCallback<PointerDownEvent>(OnPointerDown);
        target.UnregisterCallback<PointerMoveEvent>(OnPointerMove);
        target.UnregisterCallback<PointerUpEvent>(OnPointerUp);
    }

    protected void OnPointerDown(PointerDownEvent e)
    {
        if (_pointerID >= 0)
        {
            e.StopImmediatePropagation();
            return;
        }

        if (CanStartManipulation(e))
        {
            _startPosition = e.localPosition;
            _startValue = _knob.value;
            _pointerID = e.pointerId;
            target.CapturePointer(_pointerID);
            e.StopPropagation();
        }
    }

    protected void OnPointerMove(PointerMoveEvent e)
    {
        if (_pointerID < 0) return;
        if (!target.HasPointerCapture(_pointerID)) return;

        var diff = e.localPosition - _startPosition;
        var delta = (diff.x - diff.y) * _knob.sensitivity / 100;
        delta *= _knob.highValue - _knob.lowValue;
        _knob.value = _startValue + delta;

        e.StopPropagation();
    }

    protected void OnPointerUp(PointerUpEvent e)
    {
        if (_pointerID < 0) return;
        if (!target.HasPointerCapture(_pointerID)) return;
        if (!CanStopManipulation(e)) return;

        _pointerID = -1;
        target.ReleaseMouse();
        e.StopPropagation();
    }

    #endregion
}

} // namespace VJUI
