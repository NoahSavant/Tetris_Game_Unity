using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Text scoreText;
    public Text levelText;
    public Text lineText;

    public GameObject gameoverPannel;

    public void SetScoreText(string txt)
    {
        if (scoreText) scoreText.text = txt;
    }

    public void SetLevelText(string txt)
    {
        if (levelText) levelText.text = txt;
    }

    public void SetLineText(string txt)
    {
        if (lineText) lineText.text = txt;
    }

    public void ShowGameoverPanel(bool active)
    {
        if (gameoverPannel) gameoverPannel.SetActive(active);
    }
}
