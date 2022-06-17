using System.Collections.Generic;
using UnityEngine;

public class GameLogic : MonoBehaviour
{
    [SerializeField]
    private GameBoard gameBoard;

    [SerializeField]
    private Panel panel;

    [SerializeField]
    private Timer timer;

    private int amountOfBombs = 30;
    public int AmountOfBombs 
    {
        get { return amountOfBombs; }
        set
        {
            if (value == 10 || value == 30 || value == 60)
                amountOfBombs = value;
        }
    }

    private Vector2Int gameBoardSize = new Vector2Int(9, 16);
    public Vector2Int GameBoardSize
    {
        get { return gameBoardSize; }
        set
        {
            if (value == new Vector2Int(6, 10) || value == new Vector2Int(9, 16) || value == new Vector2Int(12, 21))
                gameBoardSize = value;
        }
    }
    public bool TimerOn { get; set; } = true;
    public GameStates GameState { get; set; }

    private int amountOfSafeTiles;

    private int amountOfEnableFlags;

    private float timeInSeconds = 0;

    private float realBoardWidth;

    private bool firstClick;

    private bool wasPressed = false;

    private Vector2Int firstCoordinates = new Vector2Int(100, 100);

    private readonly List<Vector3> tilesToOpen = new List<Vector3>();

    private Vector3 tilePrefabLocalScale, leftPoint, rightPoint;

    private void Start()
    {
        leftPoint = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 10));
        rightPoint = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, 10));
        realBoardWidth = rightPoint.x - leftPoint.x;
    }

    private void Update()
    {
        if (wasPressed)
        {
            timeInSeconds += Time.deltaTime;
        }
        if (GameState == GameStates.play && Input.GetMouseButtonDown(0))     
        {
            TryOpenTile();
        }
        else if (GameState == GameStates.play && Input.GetMouseButtonUp(0) && timeInSeconds > 0.2f)
        {
            TryPutFlag();
        }
        if (GameState == GameStates.play && timer.TimeRemainingInSeconds == 0 && TimerOn)
        {
            GameOver();
        }
    }

    private RaycastHit Click(Vector3 point)
    {
        Ray ray = Camera.main.ScreenPointToRay(point);
        Physics.Raycast(ray, out RaycastHit hit);
        return hit;
    }

    private void TryOpenTile()
    {
        RaycastHit hit = Click(Input.mousePosition);
        if (hit.collider.CompareTag("Tile"))
        {
            Tile tile = hit.collider.GetComponentInParent<Tile>();
            if (tile.BoardCoordinates == firstCoordinates)
            {
                if (timeInSeconds > 0.1f)
                {
                    if (firstClick)
                    {
                        firstClick = false;
                        gameBoard.CreateTileContent(amountOfBombs, hit);
                    }
                    CheckTile(tile);
                    timeInSeconds = 0f;
                    wasPressed = false;
                }
            }
            else
            {
                firstCoordinates = tile.BoardCoordinates;
                timeInSeconds = 0;
            }
            timeInSeconds += Time.deltaTime;
            wasPressed = true;
        }
    }

    private void TryPutFlag()
    {
        RaycastHit hit = Click(Input.mousePosition);
        if (hit.collider.CompareTag("Tile"))
        {
            Tile tile = hit.collider.GetComponentInParent<Tile>();
            if (firstCoordinates == tile.BoardCoordinates && firstClick == false)
            {
                PutFlag(tile);
                timeInSeconds = 0;
                wasPressed = false;
            }
        }
    }

    private void CheckTile(Tile tile)
    {
        if (tile.IsClosed)
        {
            if (tile.FlagOn == false)
            {
                if (tile.Content == 10)
                {
                    GameOver();
                }
                else
                {
                    tile.OpenTile();
                    amountOfSafeTiles--;
                    if (amountOfSafeTiles == 0)
                    {
                        Win();
                    }
                    tilesToOpen.Clear();
                    if (tile.Content == 0)
                    {
                        tilesToOpen.Add(tile.WorldCoordinates);
                    }
                    CheckNearestTiles(tile.WorldCoordinates);
                    OpenTiles();
                }
            }
        }
    }

    private void CheckNearestTiles(Vector3 point)
    {
        RaycastHit hit;
        for (int i = -1; i < 2; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                hit = ThrowRayNearPoint(point + new Vector3(i * tilePrefabLocalScale.x, 0f, j * tilePrefabLocalScale.z));
                if (hit.transform != null)
                {
                    if (hit.collider.CompareTag("Tile"))
                    {
                        Tile tile = hit.collider.gameObject.GetComponentInParent<Tile>();
                        if (tile.IsChecked == false)
                        {
                            tile.IsChecked = true;
                            if (tile.Content == 0)
                            {
                                tilesToOpen.Add(hit.point);
                                CheckNearestTiles(hit.point);
                            }
                        }
                    }
                }
            }
        }
    }

    private RaycastHit ThrowRayNearPoint(Vector3 point)
    {
        Physics.Raycast(point + new Vector3(0, 10, 0), -Vector3.up, out RaycastHit hit);
        return hit;
    }

    private void OpenTiles()
    {
        RaycastHit hit;
        for (int k = 0; k < tilesToOpen.Count; k++)
        {
            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    hit = ThrowRayNearPoint(tilesToOpen[k] + new Vector3(i * tilePrefabLocalScale.x, 0f, j * tilePrefabLocalScale.z));
                    if (hit.transform != null)
                    {
                        if (hit.collider.CompareTag("Tile"))
                        {
                            Tile tile = hit.collider.gameObject.GetComponentInParent<Tile>();
                            if (tile.IsClosed)
                            {
                                ChangeAmountOfFlags(tile);
                                amountOfSafeTiles--;
                                if (amountOfSafeTiles == 0)
                                {
                                    Win();
                                }
                                tile.OpenTile();
                            }
                        }
                    }
                }
            }
        }
    }

    private void OpenAllBombs()
    {
        Tile[,] tiles = gameBoard.GetTilesArray();
        for (int i = 0; i < gameBoardSize.x; i++)
        {
            for (int j = 0; j < gameBoardSize.y; j++)
            {
                if (tiles[j, i].Content == 10)
                {
                    tiles[j, i].OpenTile();
                }
            }
        }
    }

    private void PutFlag(Tile tile)
    {
        if (tile.IsClosed)
        {
            if (tile.FlagOn == false)
            {
                if (amountOfEnableFlags > 0)
                {
                    tile.Flag();
                    amountOfEnableFlags--;
                }
            }
            else
            {
                tile.Flag();
                amountOfEnableFlags++;
            }
            panel.ChangeBombText(amountOfEnableFlags);

        }
    }

    private void ChangeAmountOfFlags(Tile tile)
    {
        if (tile.FlagOn)
        {
            amountOfEnableFlags++;
            panel.ChangeBombText(amountOfEnableFlags);
        }
    }

    private void GameOver()
    {
        OpenAllBombs();
        GameState = GameStates.gameOver;
        panel.GameOver();
        timer.StopTimer();
    }

    private void Win()
    {
        GameState = GameStates.win;
        panel.WinGame();
        timer.StopTimer();
    }

    public void StartGame()
    {
        tilePrefabLocalScale = new Vector3(realBoardWidth / gameBoardSize.x, 0.01f, realBoardWidth / gameBoardSize.x);
        gameBoard.SetPrefabLocalScale(tilePrefabLocalScale);
        GameState = GameStates.play;
        firstClick = true;
        gameBoard.DestroyGameBoard();
        gameBoard.CreateGameBoard(gameBoardSize);
        amountOfSafeTiles = gameBoardSize.x * gameBoardSize.y - amountOfBombs;
        amountOfEnableFlags = amountOfBombs;
        panel.ChangeBombText(amountOfBombs);
        panel.StartGame();
        if (TimerOn)
        {
            timer.StartTimer(amountOfBombs);
        }
    }

    public enum GameStates
    {
        gameOver,
        win,
        play,
        pause
    }
}