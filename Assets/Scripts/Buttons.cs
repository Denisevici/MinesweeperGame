using UnityEngine;

public class Buttons : MonoBehaviour
{
    [SerializeField]
    private GameLogic game;

    [SerializeField]
    private Timer timer;

    [SerializeField]
    private Panel panel;

    public void Quit()
    {
        Application.Quit();
    }

    public void Restart()
    {
        game.StartGame();
    }

    public void HardSetting()
    {
        game.AmountOfBombs = 60;
        game.GameBoardSize = new Vector2Int(12, 21);
        game.StartGame();
    }

    public void NormalSetting()
    {
        game.AmountOfBombs = 30;
        game.GameBoardSize = new Vector2Int(9, 16);
        game.StartGame();
    }

    public void EasySetting()
    {
        game.AmountOfBombs = 10;
        game.GameBoardSize = new Vector2Int(6, 10);
        game.StartGame();
    }

    public void TimerOn()
    {
        timer.TimerOn(game.AmountOfBombs);
        game.TimerOn = true;
        game.StartGame();
    }

    public void TimerOff()
    {
        timer.TimerOff();
        game.TimerOn = false;
    }

    public void SettingsOn()
    {
        panel.SettingsOn();
        game.GameState = GameLogic.GameStates.pause;
    }

    public void SettingsOff()
    {
        panel.SettingsOff();
        game.GameState = GameLogic.GameStates.play;
    }

    public void StartInfoOff()
    {
        panel.StartInfoOff();
        game.StartGame();
    }
}
