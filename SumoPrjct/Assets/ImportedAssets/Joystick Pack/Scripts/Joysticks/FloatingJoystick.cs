using UnityEngine.EventSystems;

public class FloatingJoystick : Joystick
{
    public bool IsWorking { get; private set; }
    protected override void Start()
    {
        base.Start();
        background.gameObject.SetActive(false);
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        IsWorking = true;
        background.anchoredPosition = ScreenPointToAnchoredPosition(eventData.position);
        background.gameObject.SetActive(true);
        base.OnPointerDown(eventData);
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        IsWorking = false;
        background.gameObject.SetActive(false);
        base.OnPointerUp(eventData);
    }
}