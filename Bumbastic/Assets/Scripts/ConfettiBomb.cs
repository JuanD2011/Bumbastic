using UnityEngine;

public class ConfettiBomb : MonoBehaviour
{
    private ParticleSystem particleSystem;
    void Start()
    {
        particleSystem = GetComponent<ParticleSystem>();
        Invoke("Bum", 3f);
    }

    private void Bum()
    {
        particleSystem.Play();
    }
}
