using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject list;

    public AudioClip openListSound;
    public AudioClip closeListSound;

    public AudioClip buttonClick;

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

    public void PlayButtonClick()
    {
        if (buttonClick != null)
        {
            audioSource.PlayOneShot(buttonClick);
        }
    }

}
