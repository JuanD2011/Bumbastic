using UnityEngine;
using UnityEngine.EventSystems;
using Photon.Pun;
using UnityEngine.Playables;

public class Joystick : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    [Header("Options")]
    [Range(0f, 2f)] public float handleLimit = 1f;
    public JoystickMode joystickMode = JoystickMode.AllAxis;
    public JoystickType type;

    protected Vector2 inputVector = Vector2.zero;

    [Header("Components")]
    public RectTransform background;
    public RectTransform handle;

    protected bool isMoving;

    private Bummie p_Bummie;
    private PhotonView pV;

    public float Horizontal { get { return inputVector.x; } }
    public float Vertical { get { return inputVector.y; } }
    public Vector2 Direction { get { return new Vector2(Horizontal, Vertical); } }
    public bool IsMoving { get => isMoving; private set => isMoving = value; }

    public delegate void DelJoystick();
    public DelJoystick OnResetTime;

    public delegate void DelJoystickAim(bool _show);
    public DelJoystickAim OnPathShown;


    protected virtual void Start()
    {
        enabled = false;
        p_Bummie = GetComponentInParent<Bummie>();
        pV = GetComponentInParent<PhotonView>();

        if (pV.IsMine)
        {
            p_Bummie.OnDisableJoystick += DisableJoystick;
            GameManager.instance.Director.stopped += InitJoystick;
        }
    }

    private void InitJoystick(PlayableDirector obj)
    {
        Debug.Log("Prendo los joysticks");
        background.gameObject.SetActive(true);
        enabled = true;
    }

    protected virtual void DisableJoystick()
    {
        enabled = false;
        handle.anchoredPosition = Vector2.zero;
        inputVector = Vector2.zero;
        background.gameObject.SetActive(false);
    }

    public virtual void OnDrag(PointerEventData eventData)
    {

    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        isMoving = true;
        background.gameObject.SetActive(true);
        background.position = eventData.position;
        handle.anchoredPosition = Vector2.zero;
    }

    public virtual void OnPointerUp(PointerEventData eventData)
    {
        isMoving = false;
        handle.anchoredPosition = Vector2.zero;
        inputVector = Vector2.zero;
    }

    protected void ClampJoystick()
    {
        if (joystickMode == JoystickMode.Horizontal)
            inputVector = new Vector2(inputVector.x, 0f);
        if (joystickMode == JoystickMode.Vertical)
            inputVector = new Vector2(0f, inputVector.y);
    }
}

public enum JoystickMode { AllAxis, Horizontal, Vertical}