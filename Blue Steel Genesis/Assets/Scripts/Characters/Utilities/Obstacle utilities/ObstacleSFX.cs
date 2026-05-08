using UnityEngine;

public class ObstacleSFX : MonoBehaviour
{
    public void play(TriggerType trigger)
    {
        AudioClip clip = trigger switch {
            TriggerType.OnHealthLost => healthLost,
            TriggerType.OnHeal => heal,
            TriggerType.OnDamage => damage,
            TriggerType.OnDeath => death,
            _ => null
        };

        if (clip != null)
            audio_src.PlayOneShot(clip);
    }

    private void Awake() {
        audio_src = GetComponent<AudioSource>();
        if (audio_src == null)
            audio_src = gameObject.AddComponent<AudioSource>();
        audio_src.spatialBlend = 0;
    }

    private AudioSource audio_src;
    [SerializeField] private AudioClip healthLost;
    [SerializeField] private AudioClip damage;
    [SerializeField] private AudioClip heal;
    [SerializeField] private AudioClip death;
}
