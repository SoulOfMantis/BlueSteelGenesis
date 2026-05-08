using UnityEngine;

public class ModuleEntrySFX : MonoBehaviour
{
    public void playRemove() => audio_src.PlayOneShot(remove);
    public void playSell() => audio_src.PlayOneShot(sell);
    public void playUpgrade() => audio_src.PlayOneShot(upgrade);

    private void Awake() {
        audio_src = GetComponent<AudioSource>();
        if (audio_src == null)
            audio_src = gameObject.AddComponent<AudioSource>();
        audio_src.spatialBlend = 0;
        audio_src.volume = .5f;
    }

    private AudioSource audio_src;
    [SerializeField] private AudioClip remove;
    [SerializeField] private AudioClip sell;
    [SerializeField] private AudioClip upgrade;
}
