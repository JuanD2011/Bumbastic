using UnityEngine;
using UnityEngine.EventSystems;

public enum JoystickType
{
    Movement,
    Aiming
}

public class FloatingJoystick : Joystick
{
    Vector2 joystickCenter = Vector2.zero;
    RectTransform m_RectTransform;
    Vector2 m_initPos;
    float magnitude;
    Vector2 pointerPos;
    float velMov = 4f;

    [SerializeField] Settings settings;

    protected override void Start()
    {
        base.Start();
        m_RectTransform = transform.GetChild(0).GetComponent<RectTransform>();
        if (m_RectTransform != null)
            m_initPos = m_RectTransform.anchoredPosition;
    }

    protected override void DisableJoystick()
    {
        base.DisableJoystick();
        m_RectTransform.anchoredPosition = m_initPos;
    }

    public override void OnDrag(PointerEventData eventData)
    {
        Vector2 direction = eventData.position - joystickCenter;
        inputVector = (direction.magnitude > background.sizeDelta.x / 2f) ? direction.normalized : direction / (background.sizeDelta.x / 2f);
        ClampJoystick();
        handle.anchoredPosition = (inputVector * background.sizeDelta.x / 2f) * handleLimit;
        magnitude = Direction.magnitude;

        pointerPos = eventData.position;
    }

    public void Update()
    {
        if (!settings.isJoysitckLocked)
        {
            if (type == JoystickType.Movement)
            {
                Vector2 distance = pointerPos - joystickCenter;
                if (Vector2.Distance(pointerPos, joystickCenter) > 2f && Direction.magnitude > handleLimit)
                {
                    background.transform.Translate(distance * Time.deltaTime * velMov);
                    joystickCenter = Vector2.MoveTowards(joystickCenter, pointerPos, distance.magnitude * Time.deltaTime * velMov);
                }
            }
        }
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);
        if (type == JoystickType.Aiming) OnPathShown?.Invoke(true);
        else if (type == JoystickType.Movement) OnResetTime?.Invoke();
        joystickCenter = eventData.position;
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        base.OnPointerUp(eventData);
        if (type == JoystickType.Aiming) OnPathShown?.Invoke(false);
        m_RectTransform.anchoredPosition = m_initPos;
        if (type == JoystickType.Aiming && magnitude >= 0.2f)
        {
            gameObject.GetComponentInParent<Bummie>().ThrowBomb();
        }
    }

}