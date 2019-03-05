using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class Bummie : MonoBehaviour, IOnEventCallback
{
    #region Movement
    private float targetRotation;
    protected float speedSmooothTime = 0.075f, animationSpeedPercent;
    [SerializeField] float moveSpeed, turnSmooth, powerUpSpeed;
    float turnSmoothVel, currentSpeed, speedSmoothVel, targetSpeed;

    private Vector2 input, inputDirection, inputAiming, inputAim;
    private Joystick[] joysticks;
    private Joystick joystickMovement;
    private Joystick joystickAiming;
    Vector3 movement;

    LineRenderer m_AimPath;
    #endregion

    #region Move or bum
    float timeBeforeBum = 5f;
    float elapsedTime = 0f;
    bool exploded = false;
    #endregion

    #region Bomb
    [SerializeField] private bool hasBomb = false;
    float throwForce = 20f;
    #endregion

    [SerializeField] private bool speedPU;
    private PhotonView pV;

    bool canMove;
    Animator m_Animator;
    private Vector3 spawnPoint;

    public bool HasBomb { get => hasBomb; set => hasBomb = value; }
    public bool SpeedPU { get => speedPU; set => speedPU = value; }
    public bool CanMove { private get => canMove; set => canMove = value; }
    public Vector3 SpawnPoint { get => spawnPoint; set => spawnPoint = value; }

    public delegate void DelBummie();
    public DelBummie OnDisableJoystick;

    private void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }

    private void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
        Destroy(this);
    }

    private void Start()
    {
        canMove = false;

        pV = GetComponent<PhotonView>();
        joysticks = GetComponentsInChildren<FloatingJoystick>(true);
        m_Animator = GetComponentInChildren<Animator>();

        GameManager.instance.Director.stopped += Director_stopped;

        m_AimPath = transform.GetChild(2).GetComponent<LineRenderer>();
        m_AimPath.SetPosition(1, new Vector3(0, 0, throwForce/1.8f));

        foreach (FloatingJoystick joystick in joysticks)
        {
            if (joystick.type == JoystickType.Movement)
            {
                joystick.OnResetTime += ResetTime;
                joystickMovement = joystick;
            }
            else if (joystick.type == JoystickType.Aiming)
            {
                joystickAiming = joystick;
                joystickAiming.OnPathShown += SetPath;
            }
        }
    }

    private void Director_stopped(UnityEngine.Playables.PlayableDirector obj)
    {
        canMove = true;
    }

    private void ResetTime()
    {
        elapsedTime = 0f;
    }

    private void SetPath(bool _show)
    {
        m_AimPath.gameObject.SetActive(_show);
    }

    private void Update()
    {
        if (pV.IsMine)
        {
            if (CanMove)
            {
                input = new Vector2(joystickMovement.Horizontal, joystickMovement.Vertical);
                inputAiming = new Vector2(joystickAiming.Horizontal, joystickAiming.Vertical);
            }
            else
            {
                input = Vector2.zero;
                inputAiming = Vector2.zero;
            }
        }

        //Move Or Bum
        //if (!joystickMovement.IsMoving && !exploded)
        //{
        //    elapsedTime += Time.deltaTime;
        //    if (elapsedTime > timeBeforeBum)
        //    {
        //        exploded = true;
        //        EZCameraShake.CameraShaker.Instance.ShakeOnce(4f, 2.5f, 0.1f, 1f);
        //        gameObject.SetActive(false);
        //        GameManager.instance.PlayersInGame.Remove(this);
        //        if (hasBomb)
        //            GameManager.instance.GiveBomb();
        //    }
        //}
        //else if (joystickMovement.Direction.magnitude >= 0.2f && !isMoving){
        //    elapsedTime = 0f;
        //    isMoving = true;
        //    print(elapsedTime);
        //}
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        inputDirection = input.normalized;
        inputAim = inputAiming.normalized;

        if (inputAim != Vector2.zero)
        {
            if (joystickAiming.Direction.magnitude >= 0.2f)
            {
                m_AimPath.gameObject.SetActive(true);
                targetRotation = Mathf.Atan2(inputAiming.x, inputAiming.y) * Mathf.Rad2Deg;
                transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref turnSmoothVel, turnSmooth);
            }
            else m_AimPath.gameObject.SetActive(false);
        }
        else if (inputDirection != Vector2.zero)
        {
            targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.y) * Mathf.Rad2Deg;
            transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref turnSmoothVel, turnSmooth);
        }
        targetSpeed = ((SpeedPU) ? powerUpSpeed : moveSpeed) * inputDirection.magnitude;

        movement = new Vector3(inputDirection.x, 0, inputDirection.y);
        currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref speedSmoothVel, speedSmooothTime);
        transform.Translate(movement * currentSpeed * Time.deltaTime, Space.World);
        animationSpeedPercent = ((SpeedPU) ? 1 : 0.5f) * inputDirection.magnitude;

        m_Animator.SetFloat("speed", animationSpeedPercent, speedSmooothTime, Time.deltaTime);
    }

    public void ThrowBomb()
    {
        if (HasBomb && pV.IsMine)
        {
            m_Animator.SetTrigger("Launch");
            pV.RPC("RPC_ThrowBomb", RpcTarget.AllViaServer);
        }
    }

    [PunRPC]
    void RPC_ThrowBomb(PhotonMessageInfo info)
    {
        float lag = (float)(PhotonNetwork.Time - info.SentServerTime);
        GameManager.instance.bomb.transform.parent = null;
        GameManager.instance.bomb.RigidBody.constraints = RigidbodyConstraints.None;
        GameManager.instance.bomb.RigidBody.velocity = GameManager.instance.bombHolder.transform.forward * throwForce;
        GameManager.instance.bomb.RigidBody.position += GameManager.instance.bomb.RigidBody.velocity * lag;
        hasBomb = false;
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
        //This is when they throw the bomb
        if (other.gameObject.GetComponent<Bomb>() != null && !hasBomb)
        {
            PassBomb();
        }
        //When a player touches another player
        else if (other.gameObject.GetComponentInChildren<Bomb>() != null && !hasBomb)
        {
            other.gameObject.GetComponent<Bummie>().HasBomb = false;
            PassBomb();
        }
    }

    private void PassBomb()
    {
        GameManager.instance.bombHolder = this;
        pV.RPC("RPC_SyncBomb", RpcTarget.All, GameManager.instance.bombHolder.gameObject.GetComponent<PhotonView>().ViewID);
    }

    public IEnumerator CantMove(float _time)
    {
        WaitForSeconds wait = new WaitForSeconds(_time);
        CanMove = false;
        yield return wait;
        CanMove = true;
    }

    [PunRPC]
    void RPC_SyncBomb(int bombHolderID)
    {
        GameManager.instance.bombHolder = PhotonView.Find(bombHolderID).gameObject.GetComponent<Bummie>();
        foreach (Bummie bummie in GameManager.instance.PlayersInGame)
        {
            bummie.HasBomb = false;
        }
        GameManager.instance.bombHolder.HasBomb = true;
        GameManager.instance.bomb.transform.parent = null;
        GameManager.instance.bomb.transform.SetParent(GameManager.instance.bombHolder.transform);
        GameManager.instance.bomb.transform.position = GameManager.instance.bombHolder.transform.GetChild(1).transform.position;
        GameManager.instance.bomb.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
    }

    private void NewRound()
    {
        transform.position = spawnPoint;
        if (pV.IsMine)
        {
            OnDisableJoystick?.Invoke();//Joystick
            PhotonNetwork.RaiseEvent(GameManager.instance.onBummieReady, null, GameManager.instance.RaiseEventOptions, GameManager.instance.SendOptions); 
        }
        canMove = false;   
    }

    public void OnEvent(EventData photonEvent)
    {
        byte eventCode = photonEvent.Code;

        if (eventCode == GameManager.instance.bomb.onBombExploded)
        {
            Debug.Log("Escuché la bomba");
            NewRound();
        }
    }
}