using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    float fallTime;
    float fallStep;
    float fallSpeed;
    public bool stop = false;

    float moveTime;
    float moveStep;
    float moveSpeed;

    Limit m_l;

    public int rotate;
    int zindex = 0;

    int canRotate = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        m_l = FindObjectOfType<Limit>();
        fallSpeed = 1 - m_l.SetLevel()*0.01f;
        fallStep = Time.time;
        fallTime = fallSpeed;
        moveSpeed = fallSpeed / 10;
        moveStep = Time.time;
        moveTime = moveSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if (stop) return;
        GetInput();
        FallDown();
    }

    void FallDown()
    {
        if (Time.time - fallStep >= fallTime)
        {
            transform.position += Vector3.down;
            fallStep = Time.time;
            if (OutRange())
            {
                m_l.TouchSound();
                transform.position += Vector3.up;
                m_l.UpdateGrid(this);
                if(!stop)
                {
                    stop = true;
                    m_l.CheckGrid();
                    m_l.GetNext();
                }            
            }
            if (m_l.Gameover(this))
            {
                m_l.SetGameover();
            }
            canRotate++;
        }
    }

    void GetInput()
    {
        if (Input.GetKey(KeyCode.LeftArrow) && Time.time - moveStep >= moveTime)
        {
            transform.position += Vector3.left;
            moveStep = Time.time;
            m_l.MoveSound();
            if(OutRange()) transform.position += Vector3.right;
        }
        if (Input.GetKey(KeyCode.RightArrow) && Time.time - moveStep >= moveTime)
        {
            transform.position += Vector3.right;
            moveStep = Time.time;
            m_l.MoveSound();
            if (OutRange()) transform.position += Vector3.left;
        }
        if (Input.GetKeyDown(KeyCode.UpArrow) && rotate != 0 && canRotate > 2)
        {
            if(rotate == 1)
            {
                zindex = 90;
                rotate = 2;
            }
            else if (rotate == 2)
            {
                zindex = -90;
                rotate = 1;
            }
            if (rotate == 3)
            {
                zindex = 90;
            }
            transform.Rotate(0, 0, zindex);
            m_l.MoveSound();
            if (OutRange()) transform.Rotate(0, 0, -zindex);
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            fallTime = fallSpeed / 10;
        }
        if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            fallTime = fallSpeed;
        }
        if(Input.GetKeyDown(KeyCode.Space))
        {
            m_l.PauseGame();
            m_l.MusicGame();
        }
    }

    bool OutRange()
    {
        foreach(Transform unit in transform)
        {
            Vector2 pos = m_l.Round(unit.position);
            if (m_l.OutGrid(pos)) return true;
            if (m_l.ExistBlock(pos) != null && m_l.ExistBlock(pos).parent != transform) return true;
        }
        return false;
    }

}