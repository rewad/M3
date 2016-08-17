using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System;

public class Grid : MonoBehaviour
{ 
    public int SizeGridColumn { get { return 8; } }
    public int SizeGridRow { get { return 8; } }
    public float SpeedOneCell { get { return 0.1f; } }
    private Tile[,] m_grid;
    private float SizeTile { get { return 3.0f; } }
    private float SpaceTile { get { return 0.5f; } }

    private bool m_isUpdateGrid;
    private bool m_waitingFind;
    private bool m_isFreeSwap;

    private TilesCache m_cacheTiles;
    private Sprite[] m_spriteTiles;

    private int m_numColors;

    void Awake()
    {
        m_cacheTiles = new TilesCache();
        m_cacheTiles.InitCache(SizeGridColumn * SizeGridRow);
        m_grid = new Tile[SizeGridRow, SizeGridColumn];

        m_spriteTiles = new Sprite[6];
        for (int i = 1; i <= 6; i++)
        {
            m_spriteTiles[i - 1] = Resources.Load<Sprite>("Sprites/Jelly" + i.ToString());
        }
    }
    void Update()
    {
        if (!m_waitingFind && m_isUpdateGrid)
        {
            StartCoroutine(FindAllMatch());
        }
    }
    private float GetSizeGridWidth()
    {
        return SizeGridColumn * SizeTile + (SizeGridColumn - 1) * SpaceTile;
    }
    private float GetSizeGridHeight()
    {
        return SizeGridRow * SizeTile + (SizeGridRow - 1) * SpaceTile;
    }
    public bool IsUpdateGrid()
    {
        return m_isFreeSwap;
    }

    #region GridEditor
    public void Clear()
    {
        for (int row = 0; row < SizeGridRow; row++)
        {
            for (int column = 0; column < SizeGridColumn; column++)
            {
                if (m_grid[row, column] != null)
                {
                    RemoveTile(row, column);
                }
            }
        }
        m_isFreeSwap = true;
    }
    public IEnumerator GeneratorRandomGrid(int num_colors)
    {
        m_numColors = num_colors;
        Debug.Log(m_numColors);
        for (int row = 0; row < SizeGridRow; row++)
        {
            for (int column = 0; column < SizeGridColumn; column++)
            {
                GetRandomTile(row, column);
            }
        }

        yield return new WaitForSeconds((SizeGridRow + 1) * SpeedOneCell);
        m_isUpdateGrid = true;
    }
    #endregion

    #region SpawnTile
    private Vector3 GetPositionTile(int row, int column)
    {
        float center_tile = SizeTile * 0.5f;
        Vector3 position = Vector3.zero;
        position.x = GetSizeGridWidth() * -0.5f + column * (SizeTile + SpaceTile) + center_tile;
        position.y = GetSizeGridHeight() * -0.5f + row * (SizeTile + SpaceTile) + center_tile;
        position.z = 0.0f;
        return position;
    }
    public Tile GetRandomTile(int row, int column)
    {
        Vector3 new_position = GetPositionTile(row, column);
        Tile tile = m_cacheTiles.GetTile();
        tile.SetPosition(GetPositionTile(SizeGridRow, column));

        SpriteRenderer sprite = tile.GetComponent<SpriteRenderer>();
        int type = UnityEngine.Random.Range(0, m_numColors);
        sprite.sprite = m_spriteTiles[type];

        AnimationComponent component = tile.GetComponent<AnimationComponent>();
        tile.GetComponent<Tile>().CreateTile(row, column, (ETileType)type, new_position);
        component.AnimationTo(new_position, (9 - row) * SpeedOneCell);
        m_grid[row, column] = tile.GetComponent<Tile>();
        return tile;
    }
    #endregion

    #region Swap
    public IEnumerator Swap(Tile a, Tile b)
    {
        Vector3 a_position;
        Swap_internal(a, b, out a_position);
        m_isFreeSwap = false;

        a.GetComponent<AnimationComponent>().AnimationTo(b.CTransform.position, 0.3f);
        b.GetComponent<AnimationComponent>().AnimationTo(a_position, 0.3f);
        yield return new WaitForSeconds(0.5f);

        if (IsGoodSwap(a, b))
        {
            m_isUpdateGrid = true;

        }
        else
        {
            Swap_internal(a, b, out a_position);
            a.GetComponent<AnimationComponent>().AnimationTo(b.CTransform.position, 0.3f);
            b.GetComponent<AnimationComponent>().AnimationTo(a_position, 0.3f);
            yield return new WaitForSeconds(0.5f);
            m_isFreeSwap = true;
        }
    }
    private void Swap_internal(Tile a, Tile b, out Vector3 a_position)
    {
        TileShortInfo a_info, b_info;
        a_position = a.CTransform.position;
        a_info = a.ToShortInfo();
        b_info = b.ToShortInfo();
        a.UpdateFromShortInfo(b_info);
        b.UpdateFromShortInfo(a_info);
        m_grid[a.GetRow(), a.GetColumn()] = a;
        m_grid[b.GetRow(), b.GetColumn()] = b;
    }
    public bool IsNeighbors(Tile a, Tile b)
    {
        int distance = Mathf.Abs(a.GetColumn() - b.GetColumn()) + Mathf.Abs(a.GetRow() - b.GetRow());
        return distance < 2;
    }
    public bool IsGoodSwap(Tile a, Tile b)
    {
        if (FindMatch(GetRowInfo(a.GetRow()), 3).Count >= 3) return true;
        if (FindMatch(GetColumnInfo(a.GetColumn()), 3).Count >= 3) return true;
        if (FindMatch(GetRowInfo(b.GetRow()), 3).Count >= 3) return true;
        if (FindMatch(GetColumnInfo(b.GetColumn()), 3).Count >= 3) return true;

        return false;


    }
    #endregion

    #region Match
    private List<TileShortInfo> FindMatch(List<TileShortInfo> tiles, int min_match)
    {
        int index = 0;
        int count = 1;
        ETileType type = tiles[0].TypeTile;

        List<TileShortInfo> result = new List<TileShortInfo>();

        for (int i = 1; i < tiles.Count; i++)
        {
            if (type != tiles[i].TypeTile)
            {
                if (count >= min_match)
                {
                    for (int j = 0; j < count; j++)
                        result.Add(tiles[j + index]);
                }
                count = 1;
                index = i;
                type = tiles[i].TypeTile;
            }
            else
            {
                count++;
            }
        }
        if (count >= min_match)
        {
            for (int i = 0; i < count; i++)
                result.Add(tiles[tiles.Count - i - 1]);
        }
        return result;
    }
    private List<TileShortInfo> GetRowInfo(int row)
    {
        List<TileShortInfo> info_list = new List<TileShortInfo>();
        for (int i = 0; i < SizeGridColumn; i++)
            info_list.Add(m_grid[row, i].ToShortInfo());
        return info_list;
    }
    private List<TileShortInfo> GetColumnInfo(int column)
    {
        List<TileShortInfo> info_list = new List<TileShortInfo>();
        for (int i = 0; i < SizeGridRow; i++) info_list.Add(m_grid[i, column].ToShortInfo());
        return info_list;
    }
    private IEnumerator FindAllMatch()
    {
        m_waitingFind = true;
        HashSet<TileShortInfo> info = GetAllMatch();
        int min_index = 0;

        if (info.Count != 0)
        {
            min_index = 8;
            for (int i = 0; i < SizeGridColumn; i++)
            {
                min_index = Mathf.Min(min_index, DawnShift(i));
            }
        }
        yield return new WaitForSeconds((SizeGridRow - min_index) * SpeedOneCell + 0.2f);
        m_isUpdateGrid = info.Count > 0;
        m_waitingFind = false;

        if (!m_isUpdateGrid)
            m_isFreeSwap = true;
    }
    private void RemoveTile(int row, int column)
    {
        Tile tile = m_grid[row, column];
        m_cacheTiles.ReleaseTile(tile);
        m_grid[row, column] = null;
    }
    private HashSet<TileShortInfo> GetAllMatch()
    {
        HashSet<TileShortInfo> info = FindMatchTiles();
        foreach (var tile in info)
        {
            if (m_grid[tile.Row, tile.Column])
            {
                GameInstance.Get().UpdateScore(1);
                RemoveTile(tile.Row, tile.Column);
            }
        }
        return info;
    }
    private HashSet<TileShortInfo> FindMatchTiles()
    {
        HashSet<TileShortInfo> info = new HashSet<TileShortInfo>();
        for (int i = 0; i < SizeGridRow; i++)
        {
            var tiles = FindMatch(GetRowInfo(i), 3);
            info = new HashSet<TileShortInfo>(info.Union(tiles));
        }
        for (int i = 0; i < SizeGridColumn; i++)
        {
            var tiles = FindMatch(GetColumnInfo(i), 3);
            info = new HashSet<TileShortInfo>(info.Union(tiles));
        }
        return info;
    }
    private int DawnShift(int column)
    {
        Tile[] life_tile = new Tile[SizeGridRow];
        int num_life_tile = 0;

        for (int row = 0; row < SizeGridRow; row++)
        {
            if (m_grid[row, column] != null)
            {
                life_tile[num_life_tile] = m_grid[row, column];
                num_life_tile++;
            }
        }
        if (num_life_tile != SizeGridRow)
        {
            for (int i = 0; i < num_life_tile; i++)
            {
                if (life_tile[i].GetRow() != i)
                {
                    life_tile[i].CTransform.position = GetPositionTile(i, column);
                    life_tile[i].UpdateTilePosition(i, column);
                    m_grid[i, column] = life_tile[i];
                }
            }
        }

        int min = SizeGridRow;

        for (int i = num_life_tile; i < SizeGridRow; i++)
            min = Mathf.Min(GetRandomTile(i, column).GetRow(), min);
        return min;
    }
    #endregion
}
