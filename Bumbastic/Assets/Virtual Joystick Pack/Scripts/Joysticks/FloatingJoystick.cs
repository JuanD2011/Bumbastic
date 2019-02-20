using UnityEngine;
using UnityEngine.EventSystems;
using Photon.Pun;

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
    private PhotonView pV;
    float magnitude;
    private bool joystickLocked;

    Vector2 pointerPos;

    void Start()
    {
        joystickLocked = false;
        pV = GetComponent<PhotonView>();

        m_RectTransform = transform.GetChild(0).GetComponent<RectTransform>();
        if (m_RectTransform != null)
            m_initPos = m_RectTransform.anchoredPosition;
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

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(pointerPos, joystickCenter);
    }

    public void Update()
    {
        if (!joystickLocked)
        {
            Vector2 magnitude = pointerPos - joystickCenter;
            if (Vector2.Distance(pointerPos,joystickCenter) > 2f && Direction.magnitude > handleLimit)
            {
                background.anchoredPosition += Direction.normalized;
                joystickCenter += Direction.normalized;
                Debug.Log(joystickCenter);
            } 
        }
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        if (type == JoystickType.Aiming) OnPathShown?.Invoke(true);
        else if (type == JoystickType.Movement) OnResetTime?.Invoke();
        isMoving = true;
        background.gameObject.SetActive(true);
        background.position = eventData.position;
        handle.anchoredPosition = Vector2.zero;
        joystickCenter = eventData.position;
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        if (type == JoystickType.Aiming) OnPathShown?.Invoke(false);
        isMoving = false;
        m_RectTransform.anchoredPosition = m_initPos;
        handle.anchoredPosition = Vector2.zero;
        inputVector = Vector2.zero;
        if (type == JoystickType.Aiming && magnitude >= 0.2f)
        {
            gameObject.GetComponentInParent<Bummie>().ThrowBomb();
        }
    }
}