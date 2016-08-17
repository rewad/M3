using UnityEngine;
using System.Collections;
using System;

public enum ETileType
{ 
    TileRed=0,
    TileBlue,
    TileGreen,
    TileBlock,
}
    

public class Tile :CachedObject
{   

    private int m_positionColumn;
    private int m_positionRow;
    private AnimationComponent m_animation;
    private Vector3 m_target;
    private ETileType m_type;


    void Awake()
    {
        Cached();
        m_animation = GetComponent<AnimationComponent>();
    }

    public int GetColumn()
    {
        return m_positionColumn;
    }

    public int GetRow()
    {
        return m_positionRow;
    }

    public void SetColumn(int column)
    {
        m_positionColumn = column;
    }

    public void SetRow(int row)
    {
        m_positionRow = row;
    }
      
    public void CreateTile(int row, int column,ETileType type, Vector3 target)
    {
        m_positionRow = row;
        m_positionColumn = column;
        m_type = type;
        m_target = target;
    }
    
    public TileShortInfo ToShortInfo()
    {
        return new TileShortInfo { Row = m_positionRow, Column = m_positionColumn, TypeTile = m_type };
    }
    public void UpdateFromShortInfo(TileShortInfo info)
    {
        m_positionColumn = info.Column;
        m_positionRow = info.Row;
    }
    public void UpdateTilePosition(int row,int column)
    {
        m_positionRow = row;
        m_positionColumn = column;
    }
    public void SetPosition(Vector3 position)
    {
        CTransform.position = position;
    }
    void Update()
    {
        if (!m_animation.IsPlaying() && Vector3.Distance(CTransform.position, m_target) < 0.1f)
            CTransform.position = m_target;
    }
}
