using UnityEngine; 
using System.Collections.Generic;

public class TilesCache
{
    private Queue<Tile> m_freeTiles;
    private List<Tile> m_usingTiles;
    private GameObject m_template;

    public void InitCache(int size)
    {
        m_freeTiles = new Queue<Tile>();
        m_usingTiles = new List<Tile>();
        m_template = ResourcesManager.GetResource("Tile/Tile");

        for (int i = 0; i < size; i++)
        {
            Tile new_tile = CreateNewTile(false);
            m_freeTiles.Enqueue(new_tile);
        }
    }

    public Tile GetTile()
    { 
        if (m_freeTiles.Count == 0)
        {
            Tile tile = CreateNewTile(true);
            m_usingTiles.Add(tile);
            return tile;
        }

        Tile free_tile = m_freeTiles.Dequeue();
        m_usingTiles.Add(free_tile);
        free_tile.CGameObject.SetActive(true);
        return free_tile;

    }
    public void ReleaseTile(Tile tile)
    {
        m_usingTiles.Remove(tile);
        tile.CGameObject.SetActive(false);
        m_freeTiles.Enqueue(tile);
    }
    private Tile CreateNewTile(bool active)
    {
        GameObject go = GameObject.Instantiate(m_template) as GameObject;
        go.SetActive(active);
        return go.GetComponent<Tile>();
    }
}
