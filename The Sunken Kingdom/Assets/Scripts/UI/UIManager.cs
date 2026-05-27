using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject list;

    public AudioClip openListSound;
    public AudioClip closeListSound;

    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void OpenList()
    {
        list.SetActive(true);

        if (openListSound != null)
        {
            audioSource.PlayOneShot(openListSound);
        }
    }

    public void CloseList()
    {
        list.SetActive(false);

        if (closeListSound != null)
        {
            audioSource.PlayOneShot(closeListSound);
        }
    }

}
