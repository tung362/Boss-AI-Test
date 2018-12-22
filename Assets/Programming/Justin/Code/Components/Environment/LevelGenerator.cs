using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class LevelGenerator : MonoBehaviour {

    enum TileType { northWall, southWall, westWall, eastWall, regular, rune, playerEntrance, enemyEntrance, playerExit, center, corner }

    #region Prefabs

    public GameObject[] northWalls;
    public GameObject[] southWalls;
    public GameObject[] westWalls;
    public GameObject[] eastWalls;

    public GameObject[] regularTiles;
    public GameObject[] runeTiles;

    public GameObject[] playerEntrances;
    public GameObject[] enemyEntrances;
    public GameObject[] playerExits;

    public GameObject[] cornerPieces;
    public GameObject[] centerPieces;

    #endregion

    public int centerPieceDimensions;

    public int xExpansion;
    public int yExpansion;

    [Range(0, 1)]
    public float runeChance;

    #region Properties

    GameObject northWall
    { get { return northWalls[Random.Range(0, northWalls.Length - 1)]; } }
    GameObject southWall
    { get { return southWalls[Random.Range(0, southWalls.Length - 1)]; } }
    GameObject westWall
    { get { return westWalls[Random.Range(0, westWalls.Length - 1)]; } }
    GameObject eastWall
    { get { return eastWalls[Random.Range(0, eastWalls.Length - 1)]; } }

    GameObject regularTile
    { get { return regularTiles[Random.Range(0, regularTiles.Length - 1)]; } }
    GameObject runeTile
    { get { return runeTiles[Random.Range(0, runeTiles.Length - 1)]; } }

    GameObject playerEntrance
    { get { return playerEntrances[Random.Range(0, playerEntrances.Length - 1)]; } }
    GameObject enemyEntrance
    { get { return enemyEntrances[Random.Range(0, enemyEntrances.Length - 1)]; } }
    GameObject playerExit
    { get { return playerExits[Random.Range(0, playerExits.Length - 1)]; } }

    GameObject centerPiece
    { get { return centerPieces[Random.Range(0, centerPieces.Length - 1)]; } }
    GameObject cornerPiece
    { get { return cornerPieces[Random.Range(0, cornerPieces.Length - 1)]; } }

    #endregion

    // Use this for initialization
    void Start ()
    { GenerateLevel(); }
	
    void GenerateLevel()
    {
        int w = centerPieceDimensions + xExpansion * 2;
        int h = centerPieceDimensions + yExpansion * 2;

        for (int x = 0; x < w; ++x)
            for (int y = 0; y < h; ++y)
                if (!OuterCenterPiece(x, y, w, h))
                    Create(CheckType(x, y, w, h), Offset(x,y));

    }

    void Create(TileType type, Vector3 offset)
    {
        GameObject obj = null;

        switch (type)
        {
            case TileType.northWall:
                obj = northWall;
                obj = Instantiate(obj, offset, obj.transform.rotation);
                break;
            case TileType.southWall:
                obj = southWall;
                obj = Instantiate(obj, offset, obj.transform.rotation);
                break;
            case TileType.westWall:
                obj = westWall;
                obj = Instantiate(obj, offset, obj.transform.rotation);
                break;
            case TileType.eastWall:
                obj = eastWall;
                obj = Instantiate(obj, offset, obj.transform.rotation);
                break;
            case TileType.regular:
                obj = regularTile;
                obj = Instantiate(obj, offset, obj.transform.rotation);
                break;
            case TileType.rune:
                obj = runeTile;
                obj = Instantiate(obj, offset, obj.transform.rotation);
                break;
            case TileType.playerEntrance:
                obj = playerEntrance;
                obj = Instantiate(obj, offset, obj.transform.rotation);
                break;
            case TileType.enemyEntrance:
                obj = enemyEntrance;
                obj = Instantiate(obj, offset, obj.transform.rotation);
                break;
            case TileType.playerExit:
                obj = playerExit;
                obj = Instantiate(obj, offset, obj.transform.rotation);
                break;
            case TileType.center:
                obj = centerPiece;
                obj = Instantiate(obj, offset, obj.transform.rotation);
                break;
            case TileType.corner:
                obj = cornerPiece;
                obj = Instantiate(obj, offset, obj.transform.rotation);
                break;
        }

        if (obj != null)
            NetworkServer.Spawn(obj);
    }

    Vector3 Offset(int x, int y)
    { return new Vector3(x, 0, -y); }

    TileType CheckType(int x, int y, int w, int h)
    {
        bool left = x == 0;
        bool right = x == w - 1;
        bool top = y == 0;
        bool bottom = y == h - 1;

        bool outerX = x == 1 || x == w - 2;
        bool outerY = y == 1 || y == h - 2;

        bool centerX = x == (w - 1) / 2;
        bool centerY = y == (h - 1) / 2;

        bool center = centerX && centerY;

        if (outerX && centerY) return TileType.enemyEntrance;
        else if (center) return TileType.center;
        else if (left)
        {
            if (top || bottom) return TileType.corner;
            else return TileType.westWall;
        }
        else if (right)
        {
            if (top || bottom) return TileType.corner;
            else return TileType.eastWall;
        }
        else if (top)
        {
            if (centerX) return TileType.playerExit;
            return TileType.northWall;
        }
        else if (bottom)
        {
            if (centerX) return TileType.playerEntrance;
            return TileType.southWall;
        }
        else return Random.Range(0f, 1f) < runeChance ? TileType.rune : TileType.regular;
    }

    bool OuterCenterPiece(int x, int y, int w, int h)
    {
        return x >= ((w - 1) / 2 - (centerPieceDimensions - 1) / 2)
            && x <= ((w - 1) / 2 + (centerPieceDimensions - 1) / 2)
            && y >= ((h - 1) / 2 - (centerPieceDimensions - 1) / 2)
            && y <= ((h - 1) / 2 + (centerPieceDimensions - 1) / 2)
            && !(x == (w - 1) / 2
            &&   y == (h - 1) / 2);
    }
}
