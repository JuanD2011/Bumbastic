using EZCameraShake;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Bomb : MonoBehaviour
{
    float timer;
    float t = 0f;
    bool exploded = false;
    Rigidbody m_rigidBody;
    float minTime = 25f, maxTime = 35f;

    public float Timer { get => timer; set => timer = value; }
    public Rigidbody RigidBody { get => m_rigidBody; set => m_rigidBody = value; }

    private void Start()
    {
        m_rigidBody = GetComponent<Rigidbody>();
        timer = Random.Range(minTime, maxTime);
        Debug.Log(timer);
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
        CameraShaker.Instance.ShakeOnce(4f, 2.5f, 0.1f, 1f);
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
