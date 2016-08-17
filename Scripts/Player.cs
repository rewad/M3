using UnityEngine;
using System.Collections;


public enum EStateAction
{
    EFree = 0,
    EPress, 
}

public class Player : MonoBehaviour
{
    private EStateAction m_currentStatePlayer;
    private Tile m_TileFirst;
    void Awake()
    {
        m_currentStatePlayer = EStateAction.EFree;
    } 
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        { 
            GameInstance.Get().SetState(EGameState.EPauseState);
        } 

        if (!GetComponent<Grid>().IsUpdateGrid()) return;

        if (m_currentStatePlayer == EStateAction.EFree)
        {
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
                if (hit)
                {
                    m_currentStatePlayer = EStateAction.EPress;
                    m_TileFirst = hit.transform.GetComponent<Tile>();
                }
            }
        }

        else if (m_currentStatePlayer == EStateAction.EPress)
        {
            if (Input.GetMouseButtonUp(0))
            {
                RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
                if (hit)
                {
                    m_currentStatePlayer = EStateAction.EFree;
                    Tile second_tile = hit.transform.GetComponent<Tile>();
                    if (m_TileFirst != second_tile && GetComponent<Grid>().IsNeighbors(m_TileFirst, second_tile))
                    {
                        StartCoroutine(GetComponent<Grid>().Swap(m_TileFirst, second_tile)); 
                    }
                }
                m_TileFirst = null;
                m_currentStatePlayer = EStateAction.EFree;
            }
        }
    } 
}
