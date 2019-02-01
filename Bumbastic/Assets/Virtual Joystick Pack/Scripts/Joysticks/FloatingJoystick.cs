using UnityEngine;
using UnityEngine.EventSystems;

public class FloatingJoystick : Joystick
{
    Vector2 joystickCenter = Vector2.zero;
    RectTransform initTransform;
    RectTransform m_RectTransform;


    void Start()
    {
        m_RectTransform = GetComponent<RectTransform>();
        //initTransform.anchoredPosition = m_RectTransform.anchoredPosition;  
    }

    public override void OnDrag(PointerEventData eventData)
    {
        Vector2 direction = eventData.position - joystickCenter;
        inputVector = (direction.magnitude > background.sizeDelta.x / 2f) ? direction.normalized : direction / (background.sizeDelta.x / 2f);
        ClampJoystick();
        handle.anchoredPosition = (inputVector * background.sizeDelta.x / 2f) * handleLimit;
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        background.gameObject.SetActive(true);
        background.position = eventData.position;
        handle.anchoredPosition = Vector2.zero;
        joystickCenter = eventData.position;
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        //background.gameObject.SetActive(false);
        //m_RectTransform.anchoredPosition = initTransform.anchoredPosition;
        //handle.anchoredPosition = Vector2.zero;
        inputVector = Vector2.zero;
        if(gameObject.name == "AimingJoystick")
            GameManager.instance.bombHolder.ThrowBomb();
    }
}