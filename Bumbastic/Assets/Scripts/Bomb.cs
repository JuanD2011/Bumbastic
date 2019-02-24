using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Bomb : MonoBehaviour
{
    float timer;
    float t = 0f;
    bool exploded = false;
    Rigidbody m_rigidBody;
    float minTime = 20f, maxTime = 28f;

    Animator m_Animator;
    bool oneState = false;

    public float Timer { get => timer; set => timer = value; }
    public Rigidbody RigidBody { get => m_rigidBody; set => m_rigidBody = value; }

    private void Start()
    {
        m_rigidBody = GetComponent<Rigidbody>();
        m_Animator = GetComponent<Animator>();
        timer = Random.Range(minTime, maxTime);
    }

    private void Update()
    {
        if(!exploded && transform.parent != null)
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
