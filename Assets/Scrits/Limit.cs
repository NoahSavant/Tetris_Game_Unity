using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Limit : MonoBehaviour
{
    static int width = 10;
    static int height = 22;

    Transform[,] grid = new Transform[width, height];
    public GameObject[] block = new GameObject[7];

    GameObject nextBlock, presentBlock;
    static int lines = 0;
    static int level = 0;
    static int score = 0;

    UIManager m_ui;

    public AudioSource aud;
    public AudioClip getBlockSound;
    public AudioClip loseBlockSound;
    public AudioClip moveSound;
    public AudioClip touchBlockSound;

    bool shallPause = true;
    bool musicOn = false;

    // Start is called before the first frame update
    void Start()
    {
        m_ui = FindObjectOfType<UIManager>();
        presentBlock = Instantiate(block[BlockCode()], new Vector3(4.0f, 20.0f, 0), Quaternion.identity);
        nextBlock = Instantiate(block[BlockCode()], new Vector3(14.5f, 14.0f, 0), Quaternion.identity);
        nextBlock.GetComponent<Move>().stop = true;
        PauseGame();
        PauseGame();
    }

    public bool OutGrid(Vector2 vt)
    {
        if(vt.x < 0 || vt.x >= width || vt.y < 0) return true;
        return false;
    }

    public Vector2 Round(Vector2 vt)
    {
        return new Vector2(Mathf.Round(vt.x), Mathf.Round(vt.y));
    }

    int BlockCode()
    {
        return Random.Range(0, 7);
    }

    public void GetNext()
    {
        nextBlock.transform.localPosition = new Vector3(4, 20, 0);
        presentBlock = nextBlock;
        nextBlock.GetComponent<Move>().stop = false;
        nextBlock = Instantiate(block[BlockCode()], new Vector3(14.5f, 14.0f, 0), Quaternion.identity);
        nextBlock.GetComponent<Move>().stop = true;
    }

    public Transform ExistBlock(Vector2 vt)
    {
        if (vt.y >= height) return null;
        return grid[(int)vt.x, (int)vt.y];
    }

    public void UpdateGrid(Move block)
    {
        for(int y = 0; y < height; y++)
        {
            for(int x = 0; x < width; x++)
            {
                if (grid[x, y] != null && grid[x, y].parent == block.transform) grid[x, y] = null;
            }
        }    
        foreach (Transform unit in block.transform)
        {
                Vector2 vt = Round(unit.position);
                if (vt.y < height) grid[(int)vt.x, (int)vt.y] = unit;
        }       
    }

    public bool FullRow(int y)
    {
        for(int x = 0; x < width; x++)
        {
            if (grid[x, y] == null) return false;
        }
        return true;
    }

    public void MoveRowDown(int y)
    {
        for(int x = 0; x < width; x++)
        {
            if(grid[x, y] != null)
            {
                grid[x, y - 1] = grid[x, y];
                grid[x, y] = null;
                grid[x, y - 1].position += Vector3.down;
            }
        }
    }

    public void MoveAllDown(int y)
    {
        for(int i = y; i < height; i++)
        {
            MoveRowDown(i);
        }
    }

    public void DeleteRow(int y)
    {
        for(int x = 0; x < width; x++)
        {
            Destroy(grid[x, y].gameObject);
            grid[x, y] = null;
        }
        aud.PlayOneShot(getBlockSound);
    }

    public void CheckGrid()
    {
        int point = 0;
        for(int y = 0; y < height; y++)
        {
            if(FullRow(y))
            {
                DeleteRow(y);
                MoveAllDown(y + 1);
                y--;
                point++;
                lines++;
            }
        }
        if(point > 0)
        {
            SetPoint(point);
            m_ui.SetLineText("" + lines);
            level = SetLevel();
            m_ui.SetLevelText("" + level);
        }       
    }

    public void SetPoint(int point)
    {
        score += (point * 2 - 1) * 10;
        m_ui.SetScoreText("" + score);
    }

    public int SetLevel()
    {
        return (int)(score / 20 - lines / 2);
    }

    public void MoveSound()
    {
        aud.PlayOneShot(moveSound);
    }

    public void TouchSound()
    {
        aud.PlayOneShot(touchBlockSound);
    }

    public bool Gameover(Move block)
    {
        for(int x = 0; x < width; x++)
        {
            foreach (Transform unit in block.transform)
            {
                Vector2 vt = Round(unit.position);
                if (vt.y >= 20)
                {
                    return true;
                }
            }            
        }
        return false;
    }

    public void SetGameover()
    {
        PauseGame();
        aud.PlayOneShot(loseBlockSound);
        m_ui.ShowGameoverPanel(true);
    }

    public void PauseGame()
    {
        if(shallPause)
        {
            Time.timeScale = 0;
            shallPause = false;
        }
        else
        {
            Time.timeScale = 1;
            shallPause = true;
        }
    }

    public void MusicGame()
    {
        if (musicOn)
        {
            aud.mute = false;
            musicOn = false;
        }
        else
        {
            aud.mute = true;
            musicOn = true;
        }        
    }

    public void Menu()
    {
        Application.LoadLevel("Menu");
    }

    public void Replay()
    {
        SceneManager.LoadScene("Tetris");
    }

}
    