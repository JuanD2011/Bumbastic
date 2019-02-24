using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Bomb : MonoBehaviour
{
    private float t = 0f;
    private bool exploded = false;
    private float minTime = 20f, maxTime = 28f;

    private float timer;
    private Rigidbody m_rigidBody;

    public float Timer { get => timer; set => timer = value; }
    public Rigidbody RigidBody { get => m_rigidBody; set => m_rigidBody = value; }

    private Animator m_Animator;

    private AnimationCurve animationCurve = new AnimationCurve();
    private float speed = 4f;

    private void Start()
    {
        m_rigidBody = GetComponent<Rigidbody>();
        m_Animator = GetComponent<Animator>();
        timer = Random.Range(minTime, maxTime);
        SetKeys();
    }

    private void SetKeys()
    {
        animationCurve.AddKey(0, 0f);
        animationCurve.AddKey(timer, 1f);
    }

    private void Update()
    {
        m_Animator.speed = animationCurve.Evaluate(t) * speed;

        if (!exploded && transform.parent != null)
        {
            t += Time.deltaTime;
        }

        if (t > Timer && !exploded)
        {
            Explode();
        }
    }

    void Explode()
    {
        exploded = true;
        StartCoroutine(CameraShake.instance.ShakeCamera(0.4f, 6f, 1.2f));
        GameManager.instance.bombHolder.gameObject.SetActive(false);
        GameManager.instance.PlayersInGame.Remove(GameManager.instance.bombHolder);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Floor")
        {
            GameManager.instance.bombHolder.HasBomb = true;
            transform.SetParent(GameManager.instance.bombHolder.transform);
            gameObject.transform.position = GameManager.instance.bombHolder.transform.GetChild(1).transform.position;
            RigidBody.constraints = RigidbodyConstraints.FreezeAll;
        }
    }
}
