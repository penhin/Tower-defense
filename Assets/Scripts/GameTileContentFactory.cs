using UnityEngine;

[CreateAssetMenu]
public class GameTileContentFactory : GameObjectFactory
{

    [SerializeField]
    private GameTileContent destinationPrefab = default!;

    [SerializeField]
    private Tower towerPrefab = default!;

    [SerializeField]
    private GameTileContent emptyPrefab = default!;

    [SerializeField]
    private GameTileContent wallPrefab = default!;  

    [SerializeField]
    private GameTileContent spawnPointPrefab = default!;  

    public void Reclaim(GameTileContent content)
    {
        Debug.Assert(content.OriginFactory == this, "Wrong factory reclaimed!");
        Destroy(content.gameObject);
    }
    
    public GameTileContent Get(GameTileContentType type)
    {
        switch(type)
        {
            case GameTileContentType.Destination: return Get(destinationPrefab);
            case GameTileContentType.SpawnPoint: return Get(spawnPointPrefab);
            case GameTileContentType.Empty: return Get(emptyPrefab);
            case GameTileContentType.Tower: return Get(towerPrefab);
            case GameTileContentType.Wall: return Get(wallPrefab);                                   
        }
        Debug.Assert(false, "Unsupported type: " + type);
        return null;
    }

    private GameTileContent Get(GameTileContent prefab)
    {
        GameTileContent instance = Instantiate(prefab);
        instance.OriginFactory = this;
        return instance;
    }
}
