using UnityEngine;

public enum GameTileContentType
{
    Empty, Destination, Wall, Tower, SpawnPoint  
}

public class GameTileContent : MonoBehaviour
{
    [SerializeField]
    private GameTileContentType type = default!;
    private GameTileContentFactory originFactory;

    public GameTileContentType Type => type;
    
    public GameTileContentFactory OriginFactory
    {
        get => originFactory;
        set
        {
            Debug.Assert(originFactory == null, "Redefined origin factory!");
            originFactory = value;
        }
    }
    
    public void Recycle()
    {
        Debug.Log("Recycle " + name + " factory=" + originFactory);

        if (originFactory != null)
        {
            originFactory.Reclaim(this);
        }
        else
        {
            Debug.LogError("No factory, destroying directly");
            Destroy(gameObject);
        }
    }
}
