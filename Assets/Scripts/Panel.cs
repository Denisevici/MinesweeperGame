using UnityEngine;
using UnityEngine.UI;

public class Panel : MonoBehaviour
{
    [SerializeField]
    private GameObject easyButton;

    [SerializeField]
    private GameObject normalButton;

    [SerializeField]
    private GameObject hardButton;

    [SerializeField]
    private GameObject restartButton;

    [SerializeField]
    private GameObject settingsButton;

    [SerializeField]
    private GameObject quitButton;

    [SerializeField]
    private GameObject closeSettingsButton;

    [SerializeField]
    private GameObject closeStartTextButton;

    [SerializeField]
    private GameObject gameOverText;

    [SerializeField]
    private GameObject winText;

    [SerializeField]
    private GameObject bombText;

    [SerializeField]
    private GameObject backgroundStartInfoText;

    [SerializeField]
    private GameObject backgroundGameOver;

    [SerializeField]
    private GameObject backgroundWin;

    [SerializeField]
    private Text amountOfBombsText;

    [SerializeField]
    private Text startInfoText;

    private readonly float heightUI = Screen.height * 0.05f;

    private readonly float widthUI = Screen.width * 0.25f;

    private GameStates gameState;

    private void Start()
    {
        MoveUI();
        SizeUI();
        easyButton.SetActive(false);
        normalButton.SetActive(false);
        hardButton.SetActive(false);
        restartButton.SetActive(false);
        quitButton.SetActive(false);
        settingsButton.SetActive(false);
        closeSettingsButton.SetActive(false);
        gameOverText.SetActive(false);
        winText.SetActive(false);
        backgroundGameOver.SetActive(false);
        backgroundWin.SetActive(false);

        startInfoText.text = string.Format(
            "Double click on cell to open it \n " +
            "Long press to flag it \n " +
            "Change the level of difficulty in settings");
    }

    private void MoveUI()
    {
        Move(easyButton.transform, new Vector2(Screen.width * 0.75f, Screen.height * 0.8f));
        Move(normalButton.transform, new Vector2(Screen.width * 0.75f, Screen.height * 0.7f));
        Move(hardButton.transform, new Vector2(Screen.width * 0.75f, Screen.height * 0.6f));
        Move(restartButton.transform, new Vector2(Screen.width * 0.5f, Screen.height * 0.45f));
        Move(settingsButton.transform, new Vector2(Screen.width * 0.75f, Screen.height* 0.9f));
        Move(quitButton.transform, new Vector2(Screen.width * 0.75f, Screen.height * 0.4f));
        Move(closeSettingsButton.transform, new Vector2(Screen.width * 0.75f, Screen.height * 0.5f));
        Move(closeStartTextButton.transform, new Vector2(Screen.width * 0.5f, Screen.height * 0.4f));
        Move(bombText.transform, new Vector2(Screen.width * 0.5f, Screen.height * 0.9f));
        Move(backgroundStartInfoText.transform, new Vector2(Screen.width * 0.5f, Screen.height * 0.55f));
        Move(backgroundGameOver.transform, new Vector2(Screen.width * 0.5f, Screen.height * 0.55f));
        Move(backgroundWin.transform, new Vector2(Screen.width * 0.5f, Screen.height * 0.55f));
    }

    private void SizeUI()
    {
        easyButton.GetComponent<RectTransform>().sizeDelta = new Vector2(widthUI, heightUI);
        normalButton.GetComponent<RectTransform>().sizeDelta = new Vector2(widthUI, heightUI);
        hardButton.GetComponent<RectTransform>().sizeDelta = new Vector2(widthUI, heightUI);
        restartButton.GetComponent<RectTransform>().sizeDelta = new Vector2(widthUI, heightUI);
        settingsButton.GetComponent<RectTransform>().sizeDelta = new Vector2(widthUI, heightUI);
        quitButton.GetComponent<RectTransform>().sizeDelta = new Vector2(widthUI, heightUI);
        closeSettingsButton.GetComponent<RectTransform>().sizeDelta = new Vector2(widthUI, heightUI);
        closeStartTextButton.GetComponent<RectTransform>().sizeDelta = new Vector2(widthUI, heightUI);
        bombText.GetComponent<RectTransform>().sizeDelta = new Vector2(widthUI, heightUI);
    }

    private void Move(Transform target, Vector2 screenPosition)
    {
        target.position = screenPosition;
    }

    public void GameOver()
    {
        backgroundGameOver.SetActive(true);
        gameOverText.SetActive(true);
        restartButton.SetActive(true);
        bombText.SetActive(false);
        gameState = GameStates.gameOver;
    }

    public void StartGame()
    {
        backgroundGameOver.SetActive(false);
        backgroundWin.SetActive(false);
        gameOverText.SetActive(false);
        winText.SetActive(false);
        restartButton.SetActive(false);
        settingsButton.SetActive(true);
        bombText.SetActive(true);
        gameState = GameStates.play;
    }

    public void WinGame()
    {
        backgroundWin.SetActive(true);
        winText.SetActive(true);
        restartButton.SetActive(true);
        bombText.SetActive(false);
        gameState = GameStates.win;
    }

    public void ChangeBombText(int amount)
    {
        amountOfBombsText.text = string.Format("Bombs: {0}", amount);
    }

    public void SettingsOn()
    {
        easyButton.SetActive(true);
        normalButton.SetActive(true);
        hardButton.SetActive(true);
        closeSettingsButton.SetActive(true);
        quitButton.SetActive(true);
        settingsButton.SetActive(false);
        winText.SetActive(false);
        backgroundWin.SetActive(false);
        gameOverText.SetActive(false);
        backgroundGameOver.SetActive(false);
        restartButton.SetActive(false);
    }

    public void SettingsOff()
    {
        easyButton.SetActive(false);
        normalButton.SetActive(false);
        hardButton.SetActive(false);
        closeSettingsButton.SetActive(false);
        quitButton.SetActive(false);
        settingsButton.SetActive(true);

        if (gameState == GameStates.win)
        {
            winText.SetActive(true);
            backgroundWin.SetActive(true);
            restartButton.SetActive(true);
        }
        else if (gameState == GameStates.gameOver)
        {
            gameOverText.SetActive(true);
            backgroundGameOver.SetActive(true);
            restartButton.SetActive(true);
        }
    }

    public void StartInfoOff()
    {
        closeStartTextButton.SetActive(false);
        backgroundStartInfoText.SetActive(false);
    }

    public enum GameStates
    {
        gameOver,
        win,
        play
    }
}