using UnityEngine;

public class GameBoard : MonoBehaviour
{
    [SerializeField]
    private Tile tilePrefab;

    [SerializeField]
    private Transform gameBoardGround;

    private Vector3 tilePrefabLocalScale;

    private Vector3 leftUpAngle;

    private Vector2Int size;

    private Tile[,] tiles = new Tile[0, 0];

    public void CreateGameBoard(Vector2Int size)
    {
        this.size = size;
        tiles = new Tile[size.y, size.x];
        gameBoardGround.localScale = new Vector3(size.x * tilePrefabLocalScale.x * 10f, 0.01f, size.y * tilePrefabLocalScale.z * 10f);
        gameBoardGround.transform.position = new Vector3(10, 0, 10 - tilePrefabLocalScale.z);
        leftUpAngle = gameBoardGround.position + new Vector3(-((size.x / 2 + 0.5f * (size.x % 2 - 1)) * tilePrefabLocalScale.x), 0f, -((size.y / 2 + 0.5f * (size.y % 2 - 1)) * tilePrefabLocalScale.z));

        for (int i = 0; i < size.x; i++)
        {
            for (int j = 0; j < size.y; j++)
            {
                tiles[j, i] = Instantiate(tilePrefab);
                tiles[j, i].transform.position = leftUpAngle + new Vector3(i * tilePrefabLocalScale.x, 0.01f, j * tilePrefabLocalScale.z);
                tiles[j, i].Content = 0;
                tiles[j, i].transform.localScale = tilePrefabLocalScale;
                tiles[j, i].BoardCoordinates = new Vector2Int(j, i);
            }
        }
    }

    public void CreateTileContent(int level, RaycastHit hit)
    {
        Tile tile = hit.collider.gameObject.GetComponentInParent<Tile>();
        Vector2Int coordinates = tile.BoardCoordinates;
        int count;
        bool flag;

        int amountOfBombs = level;
        while (amountOfBombs != 0) // creating bombs
        {
            int rndX = Random.Range(0, size.x);
            int rndY = Random.Range(0, size.y);
            count = 0;
            flag = true;
            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; flag && j < 2; j++)
                {
                    if (rndX == j + coordinates.y && rndY == i + coordinates.x)
                    {
                        flag = false;
                    }
                    else
                    {
                        count++;
                    }
                }
            }
            if (count == 9)
            {
                if (tiles[rndY, rndX].Content != 10)
                {
                    tiles[rndY, rndX].Content = 10;
                    amountOfBombs--;
                }
            }
        }

        int content;
        for (int y = 1; y < size.y - 1; y++) // set content in center part of board
        {
            for (int x = 1; x < size.x - 1; x++)
            {
                content = 0;
                if (tiles[y, x].Content != 10)
                {
                    for (int nearY = y - 1; nearY < y + 2; nearY++)
                    {
                        if (tiles[nearY, x - 1].Content == 10)
                            content++;
                        if (tiles[nearY, x + 1].Content == 10)
                            content++;
                    }
                    if (tiles[y - 1, x].Content == 10)
                        content++;
                    if (tiles[y + 1, x].Content == 10)
                        content++;
                    tiles[y, x].Content = content;
                }
            }
        }

        for (int x = 1; x < size.x - 1; x++) // set content in first and last rows of board
        {
            content = 0;
            if (tiles[0, x].Content != 10)
            {
                if (tiles[0, x - 1].Content == 10)
                    content++;
                if (tiles[0, x + 1].Content == 10)
                    content++;
                for (int nearX = x - 1; nearX < x + 2; nearX++)
                {
                    if (tiles[1, nearX].Content == 10)
                        content++;
                }
                tiles[0, x].Content = content;
            }

            content = 0;
            if (tiles[size.y - 1, x].Content != 10)
            {
                if (tiles[size.y - 1, x - 1].Content == 10)
                    content++;
                if (tiles[size.y - 1, x + 1].Content == 10)
                    content++;
                for (int nearX = x - 1; nearX < x + 2; nearX++)
                {
                    if (tiles[size.y - 2, nearX].Content == 10)
                        content++;
                }
                tiles[size.y - 1, x].Content = content;
            }
        }

        for (int y = 1; y < size.y - 1; y++) // set content in first and last columns of board
        {
            content = 0;
            if (tiles[y, 0].Content != 10)
            {
                if (tiles[y - 1, 0].Content == 10)
                    content++;
                if (tiles[y + 1, 0].Content == 10)
                    content++;
                for (int nearY = y - 1; nearY < y + 2; nearY++)
                {
                    if (tiles[nearY, 1].Content == 10)
                        content++;
                }
                tiles[y, 0].Content = content;
            }

            content = 0;
            if (tiles[y, size.x - 1].Content != 10)
            {
                if (tiles[y - 1, size.x - 1].Content == 10)
                    content++;
                if (tiles[y + 1, size.x - 1].Content == 10)
                    content++;
                for (int nearY = y - 1; nearY < y + 2; nearY++)
                {
                    if (tiles[nearY, size.x - 2].Content == 10)
                        content++;
                }
                tiles[y, size.x - 1].Content = content;
            }
        }

        if (tiles[0, 0].Content != 10) // set left up corner content
        {
            content = 0;
            if (tiles[0, 1].Content == 10)
                content++;
            if (tiles[1, 0].Content == 10)
                content++;
            if (tiles[1, 1].Content == 10)
                content++;
            tiles[0, 0].Content = content;
        }

        if (tiles[0, size.x - 1].Content != 10) // set right up corner content
        {
            content = 0;
            if (tiles[0, size.x - 2].Content == 10)
                content++;
            if (tiles[1, size.x - 1].Content == 10)
                content++;
            if (tiles[1, size.x - 2].Content == 10)
                content++;
            tiles[0, size.x - 1].Content = content;
        }

        if (tiles[size.y - 1, 0].Content != 10) // set left down corner content
        {
            content = 0;
            if (tiles[size.y - 2, 0].Content == 10)
                content++;
            if (tiles[size.y - 2, 1].Content == 10)
                content++;
            if (tiles[size.y - 1, 1].Content == 10)
                content++;
            tiles[size.y - 1, 0].Content = content;
        }

        if (tiles[size.y - 1, size.x - 1].Content != 10) // set right down corner content
        {
            content = 0;
            if (tiles[size.y - 2, size.x - 1].Content == 10)
                content++;
            if (tiles[size.y - 2, size.x - 2].Content == 10)
                content++;
            if (tiles[size.y - 1, size.x - 2].Content == 10)
                content++;
            tiles[size.y - 1, size.x - 1].Content = content;
        }
    }

    public void DestroyGameBoard()
    {
        if (tiles.Length != 0)
        {
            foreach (Tile tile in tiles)
            {
                tile.DestroyTile();
            }
        }
    }

    public void SetPrefabLocalScale(Vector3 localScale)
    {
        tilePrefabLocalScale = localScale;
    }

    public Vector3 GetLeftUpTilePosition()
    {
        return tiles[0, 0].transform.position;
    }

    public Tile[,] GetTilesArray()
    {
        return tiles;
    }
}