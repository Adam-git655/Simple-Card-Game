using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Text MainMenuText;
    [SerializeField] private float TextScaleSpeed = 2f;
    [SerializeField] private float TextScaleAmount = 0.2f;
    private Vector3 originalScaleOfText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        originalScaleOfText = MainMenuText.transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        float scale = 1 + Mathf.PingPong(Time.time * TextScaleSpeed, TextScaleAmount);
        MainMenuText.transform.localScale = originalScaleOfText * scale;
    }

    public void OnPlayButtonPressed()
    {
        SceneManager.LoadScene("MainScene");
    }
}
