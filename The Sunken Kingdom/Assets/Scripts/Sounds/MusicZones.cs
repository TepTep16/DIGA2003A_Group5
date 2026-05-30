using UnityEngine;

public class MusicZones : MonoBehaviour
{
    public string zoneMusicName;
    public string exitMusicName;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(AudioManager.Instance.FadeToMusic(zoneMusicName, 1f));
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(AudioManager.Instance.FadeToMusic(exitMusicName, 1f));
        }
    }
}
