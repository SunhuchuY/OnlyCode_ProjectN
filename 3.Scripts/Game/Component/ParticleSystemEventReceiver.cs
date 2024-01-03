using UnityEngine;

public class ParticleSystemEventReceiver : MonoBehaviour
{
    public event System.Action OnParticleSystemStoppedEvent;

    private void OnParticleSystemStopped() => OnParticleSystemStoppedEvent?.Invoke();
}