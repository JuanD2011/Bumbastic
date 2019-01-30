using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour
{
    private Vector2 input, inputDirection, inputAiming, inputAim;
    private float targetRotation;

    protected float speedSmooothTime = 0.075f, animationSpeedPercent;

    [SerializeField] float moveSpeed, turnSmooth, powerUpSpeed;
    float turnSmoothVel, currentSpeed, speedSmoothVel, targetSpeed;

    [SerializeField] private bool speedPU;

    [SerializeField] Joystick joystickMovement;
    [SerializeField] Joystick joystickAiming;

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

    private void Update()
    {
        input = new Vector2(joystickMovement.Horizontal, joystickMovement.Vertical);
        inputAiming = new Vector2(joystickAiming.Horizontal, joystickAiming.Vertical);
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        inputDirection = input.normalized;
        inputAim = inputAiming.normalized;

        if (inputAim != Vector2.zero) {
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

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<PowerUp>() != null)
        { 
            IPowerUp powerUp = collision.gameObject.GetComponent<IPowerUp>();
            powerUp.PickPowerUp(GetComponent<Player>());
            collision.gameObject.SetActive(false);
        }
    }
}
