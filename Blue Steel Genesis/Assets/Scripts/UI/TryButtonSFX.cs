using UnityEngine;

public class TryButtonSFX : MonoBehaviour
{
    public void playFailure() => audio_src.PlayOneShot(failure);
    public void playSuccess() => audio_src.PlayOneShot(success);

    private void Awake() {
        audio_src = GetComponent<AudioSource>();
        if (audio_src == null)
            audio_src = gameObject.AddComponent<AudioSource>();
        audio_src.spatialBlend = 0;
        audio_src.volume = .5f;
    }

    private AudioSource audio_src;
    [SerializeField] private AudioClip failure;
    [SerializeField] private AudioClip success;
}
