using Photon.Pun;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Bummie : MonoBehaviour
{
    private Vector2 input, inputDirection, inputAiming, inputAim;
    private float targetRotation;

    protected float speedSmooothTime = 0.075f, animationSpeedPercent;

    [SerializeField] float moveSpeed, turnSmooth, powerUpSpeed;
    float turnSmoothVel, currentSpeed, speedSmoothVel, targetSpeed;

    [SerializeField] private bool speedPU;
    bool hasBomb = false;
    float throwForce = 10f;

    private Joystick[] joysticks;
    private Joystick joystickMovement;
    private Joystick joystickAiming;

    private PhotonView pV;

    Vector3 movement;

    public bool SpeedPU
    {
        get
        {
            return speedPU;
        }

        set
        {
            speedPU = value;
        }
    }
    public bool HasBomb { get => hasBomb; set => hasBomb = value; }

    private void Start()
    {
        pV = GetComponent<PhotonView>();
        joysticks = FindObjectsOfType<FloatingJoystick>();
        foreach(FloatingJoystick joystick in joysticks)
        {
            if(joystick.type == JoystickType.Movement)
            {
                joystickMovement = joystick;
            }
            else if(joystick.type == JoystickType.Aiming)
            {
                joystickAiming = joystick;
            }
        }
    }

    private void Update()
    {
        if (pV.IsMine)
        {
            input = new Vector2(joystickMovement.Horizontal, joystickMovement.Vertical);
            inputAiming = new Vector2(joystickAiming.Horizontal, joystickAiming.Vertical); 
        }
    }

    private void FixedUpdate()
    {
        Move();
        if (Input.GetKeyDown(KeyCode.Space))
            ThrowBomb();
    }

    private void Move()
    {
        inputDirection = input.normalized;
        inputAim = inputAiming.normalized;

        if (inputAim != Vector2.zero && joystickAiming.Direction.magnitude >= 0.2f) {
            targetRotation = Mathf.Atan2(inputAiming.x, inputAiming.y) * Mathf.Rad2Deg;
            transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref turnSmoothVel, turnSmooth);
        }
        else if(inputDirection != Vector2.zero)
        {
            targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.y) * Mathf.Rad2Deg;
            transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref turnSmoothVel, turnSmooth);
        }

        targetSpeed = ((SpeedPU) ? powerUpSpeed : moveSpeed) * inputDirection.magnitude;
        currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref speedSmoothVel, speedSmooothTime);

        movement = new Vector3(inputDirection.x, 0, inputDirection.y);
        transform.Translate( movement * currentSpeed * Time.deltaTime, Space.World);
        animationSpeedPercent = ((SpeedPU) ? 1 : 0.5f) * inputDirection.magnitude;
        //m_Animator.SetFloat("speed", animationSpeedPercent, speedSmooothTime, Time.deltaTime); 
    }

    public void ThrowBomb() {
        if (HasBomb)
        {
            GameManager.instance.bomb.transform.parent = null;
            GameManager.instance.bomb.RigidBody.constraints = RigidbodyConstraints.None;
            GameManager.instance.bomb.RigidBody.AddForce(transform.forward * throwForce, ForceMode.Impulse);
            hasBomb = false;
        }
        else {
            //The player has not the bomb
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<PowerUp>() != null && !hasBomb && GetComponent<PowerUp>() == null)
        { 
            IPowerUp powerUp = collision.gameObject.GetComponent<IPowerUp>();
            powerUp.PickPowerUp(GetComponent<Bummie>());
            collision.gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //This is when they trhow the bomb
        if (other.gameObject.GetComponent<Bomb>() != null && !hasBomb)
        {
            hasBomb = true;
            PassBomb();
        }
        //When a player touches another player
        else if (other.gameObject.GetComponentInChildren<Bomb>() != null && !hasBomb)
        {
            hasBomb = true;
            other.gameObject.GetComponent<Bummie>().HasBomb = false;
            PassBomb();
        }
    }

    private void PassBomb() {
        GameManager.instance.bombHolder = this;
        GameManager.instance.bomb.transform.parent = null;
        GameManager.instance.bomb.transform.SetParent(transform);
        GameManager.instance.bomb.transform.position = transform.GetChild(1).transform.position;
        GameManager.instance.bomb.RigidBody.constraints = RigidbodyConstraints.FreezeAll;
    }
}
