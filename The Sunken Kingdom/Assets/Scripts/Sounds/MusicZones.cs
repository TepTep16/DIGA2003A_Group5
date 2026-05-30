using UnityEngine;

public class MusicZones : MonoBehaviour
{
    public Transform player;

    public Collider2D zoneCollider;

    public AudioSource audioSource; // will separate basislisk room audio from others
    public string musicName;
    private string currentMusic;

    public float fadeDistance = 5f;

    [Range(0f, 1f)]
    public float minimumVolume = 0f;

    [Range(0f, 1f)]
    public float maximumVolume = 1f;

    public bool invertFade = false;

    public bool playerInside;

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        playerInside = false;
    }

    private void Update()
    {
        if (player == null || zoneCollider ==null || audioSource == null) return;

        Bounds bounds = zoneCollider.bounds;
        Vector2 center = bounds.center;

        float maxDistance = Mathf.Max(bounds.extents.x, bounds.extents.y);
        float distance = Vector2.Distance(player.position, center);

        float t = Mathf.Clamp01(distance / (maxDistance = fadeDistance));

        float targetVolume;
        if (!invertFade)
        {
            targetVolume = Mathf.Lerp(maximumVolume, minimumVolume, t);
        }
        else
        {
            targetVolume = Mathf.Lerp(minimumVolume, maximumVolume, t);
        }

        AudioManager.Instance.musicSource.volume =
            Mathf.Lerp(AudioManager.Instance.musicSource.volume, targetVolume, Time.deltaTime * 3f);
    }
}
