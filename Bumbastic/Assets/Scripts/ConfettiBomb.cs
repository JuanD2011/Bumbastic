using UnityEngine;

public class ConfettiBomb : MonoBehaviour
{
    private ParticleSystem m_Particle;
    private Renderer m_Renderer;
    void Start()
    {
        m_Particle = GetComponentInChildren<ParticleSystem>();
        m_Renderer = GetComponent<Renderer>();
        Invoke("Bum", 3f);
    }

    private void Bum()
    {
        m_Renderer.enabled = false;
        m_Particle.Play();
    }
}
