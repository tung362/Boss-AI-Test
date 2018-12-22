using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class OldLevelGenerator : NetworkBehaviour
{

    enum TileType { Wall, Floor, Center, Spawn, Enemy, Exit };
    List<List<GameObject>> tiles;

    [Tooltip("Dimensions MUST be an odd number greater than 3!!!")]
    public int mapWidth;
    [Tooltip("Dimensions MUST be an odd number greater than 3!!!")]
    public int mapHeight;

    public GameObject[] regularFloorTiles;
    public GameObject[] runeFloorTiles;
    public GameObject[] wallTiles;
    public GameObject[] centerTiles;
    public GameObject[] enemyTiles;
    public GameObject[] spawnTiles;
    public GameObject[] exitTiles;

    [Range(0, 1)]
    public float runeChance;

    // Use this for initialization
    void Start()
    { Generate(); }

    void Generate()
    {
        if (!isServer) return;
        GameObject env = new GameObject("Environment");
        env.AddComponent<NetworkIdentity>();
        NetworkServer.Spawn(env);

        tiles = new List<List<GameObject>>();
        for (int x = 0; x < mapWidth + 2; ++x)
        {
            tiles.Add(new List<GameObject>());
            for (int y = 0; y < mapHeight + 2; ++y)
            {
                TileType type = TileType.Floor;
                if (x == 0 || y == 0 || x == mapWidth + 1 || y == mapHeight + 1)
                {
                    type = TileType.Wall;
                    if (x == (mapWidth - 1) / 2 + 1 && y == mapHeight + 1) type = TileType.Spawn;
                    else if (x == (mapWidth - 1) / 2 + 1 && y == 0) type = TileType.Exit;
                    else if ((x == 0 || x == mapWidth + 1) && y == (mapHeight - 1) / 2 + 1) type = TileType.Enemy;
                }
                else if ((x >= (mapWidth - 1) / 2 && x <= (mapWidth - 1) / 2 + 2) && (y >= (mapHeight - 1) / 2 && y <= (mapHeight - 1) / 2 + 2))
                {
                    if (x == (mapWidth - 1) / 2 + 1 && y == (mapHeight - 1) / 2 + 1) type = TileType.Center;
                    else
                    {
                        tiles[x].Add(new GameObject("Tile X" + x + " Y" + y));
                        tiles[x][y].AddComponent<NetworkIdentity>();
                        tiles[x][y].transform.parent = env.transform;
                        continue;
                    }
                }

                switch (type)
                {
                    case TileType.Wall:
                        tiles[x].Add(Instantiate(wallTiles[Random.Range(0, wallTiles.Length - 1)]));
                        if (x == 0)
                        {
                            if (y == 0 || y == mapHeight + 1) tiles[x][y].transform.rotation = Quaternion.Euler(0, 270, 0);
                            else tiles[x][y].transform.rotation = Quaternion.Euler(0, 90, 0);
                        }
                        else if (x == mapWidth + 1)
                        {
                            if (y == 0 || y == mapHeight + 1) tiles[x][y].transform.rotation = Quaternion.Euler(0, 90, 0);
                            else tiles[x][y].transform.rotation = Quaternion.Euler(0, 270, 0);
                        }
                        else if (y == mapHeight + 1) tiles[x][y].transform.rotation = Quaternion.Euler(0, 180, 0);
                        break;
                    case TileType.Floor:
                        if(Random.Range(0.0f, 1.0f) > runeChance)
                            tiles[x].Add(Instantiate(regularFloorTiles[Random.Range(0, regularFloorTiles.Length - 1)]));
                        else
                            tiles[x].Add(Instantiate(runeFloorTiles[Random.Range(0, runeFloorTiles.Length - 1)]));
                        break;
                    case TileType.Center:
                        tiles[x].Add(Instantiate(centerTiles[Random.Range(0, centerTiles.Length - 1)]));
                        tiles[x][y].transform.localScale = new Vector3(0.95f, 0.95f, 0.95f);
                        break;
                    case TileType.Spawn:
                        tiles[x].Add(Instantiate(spawnTiles[Random.Range(0, spawnTiles.Length - 1)]));
                        tiles[x][y].transform.rotation = Quaternion.Euler(0, 180, 0);
                        break;
                    case TileType.Exit:
                        tiles[x].Add(Instantiate(exitTiles[Random.Range(0, exitTiles.Length - 1)]));
                        break;
                    case TileType.Enemy:
                        tiles[x].Add(Instantiate(enemyTiles[Random.Range(0, enemyTiles.Length - 1)]));
                        if (x == 0) tiles[x][y].transform.rotation = Quaternion.Euler(0, 90, 0);
                        else tiles[x][y].transform.rotation = Quaternion.Euler(0, 270, 0);
                        break;
                }

                tiles[x][y].transform.position = new Vector3(x, 0, y);
                tiles[x][y].name = ("Tile X" + x + " Y" + y);
                tiles[x][y].transform.parent = env.transform;
            }
        }

        foreach (List<GameObject> x in tiles) foreach (GameObject y in x) { NetworkServer.Spawn(y); }

        ServerSpawnPlayers();
    }

    [Server]
    void ServerSpawnPlayers()
    {
        RpcSpawnPlayers();
    }

    [ClientRpc]
    void RpcSpawnPlayers()
    {
        ClientScene.AddPlayer(0);
    }

	// Update is called once per frame
	void Update () {
		
	}
}
