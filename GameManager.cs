using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public CameraCtrl cameraCtrl;
    public Timer timer;
    public Judge judge;
    public RotatePort rotatePort;

    public Button startButton;
    public Button retryButton;
    public Image signalImage;
    public Image redSignal;
    public Image greenSignal;
    public AudioSource audioSource;
    public AudioClip shortBeep;
    public AudioClip longBeep;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SetTitleState();
        startButton.onClick.AddListener(StartGame);
        retryButton.onClick.AddListener(RetryGame);
    }

    public void SetTitleState()
    {
        cameraCtrl.SetTitleState();
        startButton.gameObject.SetActive(true);
        signalImage.gameObject.SetActive(false);
    }

    public void StartGame()
    {
        rotatePort.PortRotate();
        startButton.gameObject.SetActive(false);
        cameraCtrl.SetCountdownState();
        StartCoroutine(StartSignalSepuence());
    }

    private IEnumerator StartSignalSepuence()
    {
        signalImage.gameObject.SetActive(true);
        redSignal.gameObject.SetActive(true);

        audioSource.PlayOneShot(shortBeep);

        float waitTime = Random.Range(1f, 3f);
        yield return new WaitForSeconds(waitTime);

        greenSignal.gameObject.SetActive(true);

        audioSource.PlayOneShot(longBeep);

        yield return new WaitForSeconds(0.5f);

        StartCompetition();
    }

    private void StartCompetition()
    {
        cameraCtrl.SetGameStartState();
        timer.StartTimer();
    }

    public void OnShoot()
    {
        cameraCtrl.SetAfterShootState();
        timer.StopTimer();
        greenSignal.gameObject.SetActive(false);
        signalImage.gameObject.SetActive(false);
    }

    public void RetryGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
