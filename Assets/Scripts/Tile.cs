using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField]
    private Material[] material = new Material[12];

    [SerializeField]
    private Material closedMaterial;

    [SerializeField]
    private Renderer rend;

    [SerializeField]
    private Animation tileAnimation;

    private int content;
    public int Content
    {
        get { return content; }
        set
        {
            if (0 <= value && value <= 10)
                content = value;
        }
    }

    public bool IsClosed { get; private set; } = true;

    public bool IsChecked { get; set; } = false;

    public bool FlagOn { get; private set; } = false;
    
    public Vector2Int BoardCoordinates { get; set; }

    public Vector3 WorldCoordinates
    {
        get { return transform.position; }
    }

    public void OpenTile()
    {
        IsClosed = false;
        IsChecked = true;
        tileAnimation.Play();
    }

    public void ChangeMaterial()
    {
        rend.material = material[content];
    }

    public void Flag()
    {
        if (FlagOn)
        {
            rend.material = closedMaterial;
        }
        else 
        {
            rend.material = material[11];
        }
        FlagOn = !FlagOn;
    }

    public void DestroyTile()
    {
        Destroy(gameObject);
    }
}