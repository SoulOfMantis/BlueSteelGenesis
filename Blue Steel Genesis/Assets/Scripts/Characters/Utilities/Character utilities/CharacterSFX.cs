using UnityEngine;

public class CharacterSFX : MonoBehaviour
{
    public void play(TriggerType trigger)
    {
        AudioClip clip = trigger switch {
            TriggerType.OnHealthLost => healthLost,
            TriggerType.OnHealthDamage => healthDamage,
            TriggerType.OnDamageShielded => damageShielded,
            TriggerType.OnShieldBroken => shieldBroken,
            TriggerType.OnShieldGiven => shieldGiven,
            TriggerType.OnHeal => heal,
            TriggerType.OnEnergyDrain => energyDrain,
            TriggerType.OnEnergyRestore => energyRestore,
            TriggerType.OnMove => moveStep,
            TriggerType.OnStrike => strike,
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
    [SerializeField] private AudioClip healthDamage;
    [SerializeField] private AudioClip damageShielded;
    [SerializeField] private AudioClip shieldBroken;
    [SerializeField] private AudioClip shieldGiven;
    [SerializeField] private AudioClip heal;
    [SerializeField] private AudioClip energyDrain;
    [SerializeField] private AudioClip energyRestore;
    [SerializeField] private AudioClip moveStep;
    [SerializeField] private AudioClip strike;
    [SerializeField] private AudioClip death;
}
