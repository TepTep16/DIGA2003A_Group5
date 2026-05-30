using UnityEngine;

public class BasiliskZone : MonoBehaviour
{
    public Transform player;

    public Collider2D zoneCollider;

    [SerializeField] private string zoneMusicName;
    [SerializeField] private AudioSource zoneMusic;


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        AudioManager.Instance.PlayMusic("BasiliskMusic");

    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        AudioManager.Instance.musicSource.Stop();
    }

}
