using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class Player
{
    public Image panel;
    public TextMeshProUGUI text; 
}

[System.Serializable]
public class PlayerColor
{
    public Color panelColor;
    public Color textColor; 
}


public class GameController : MonoBehaviour
{
    public Player playerX;
    public Player playerO;

    public PlayerColor activePlayerColor;
    public PlayerColor inactivePlayerColor;

    public GameObject gameOverPanel;
    public TextMeshProUGUI gameOverText;

    public TextMeshProUGUI[] buttonList;
    private string playerSide;
    private int moveCount;

    public GameObject restartButton;

    private void Awake() {
        playerSide = "X";
        SetPlayerColors(playerX,playerO);
        restartButton.SetActive(false);
        moveCount = 0;
        gameOverPanel.SetActive(false);
        SetControllerOnButtons();
    }

    void SetControllerOnButtons()
    {
        for(int i=0; i < buttonList.Length; i++)
        {
            buttonList[i].GetComponentInParent<Gridspace>().SetController(this);
        }
    }

    public string GetPlayerSide()
    {
        // TODO: this will return an X or O based on the player turn 
        return playerSide;
    }

    public void EndTurn()
    {
        moveCount++;

        //TODO: Add code that ends the current turn
        if(buttonList[0].text == playerSide && buttonList[1].text == playerSide && buttonList[2].text == playerSide)
        {
            GameOver(playerSide);
        }
        else if(buttonList[3].text == playerSide && buttonList[4].text == playerSide && buttonList[5].text == playerSide)
        {
            GameOver(playerSide);
        }
        else if(buttonList[6].text == playerSide && buttonList[7].text == playerSide && buttonList[8].text == playerSide)
        {
            GameOver(playerSide);
        }
        else if(buttonList[0].text == playerSide && buttonList[3].text == playerSide && buttonList[6].text == playerSide)
        {
            GameOver(playerSide);
        }
        else if(buttonList[1].text == playerSide && buttonList[4].text == playerSide && buttonList[7].text == playerSide)
        {
            GameOver(playerSide);
        }
        else if(buttonList[2].text == playerSide && buttonList[5].text == playerSide && buttonList[8].text == playerSide)
        {
            GameOver(playerSide);
        }
        else if(buttonList[0].text == playerSide && buttonList[4].text == playerSide && buttonList[8].text == playerSide)
        {
            GameOver(playerSide);
        }
        else if(buttonList[2].text == playerSide && buttonList[4].text == playerSide && buttonList[6].text == playerSide)
        {
            GameOver(playerSide);
        }
        else if (moveCount >= 9)
        {
            GameOver("Draw");
        }
        else
        {
            ChangeSides();
            if (playerSide == "O")
            {
                ComputerTurn();
            }
        }
    }

    void GameOver(string winningPlayer)
    {
        for(int i = 0; i < buttonList.Length; i++)
        {
            buttonList[i].GetComponentInParent<Button>().interactable = false;
        }
        if(winningPlayer == "Draw")
        {
            SetGameOverText("Its a draw!");
        }
        else
        {
        SetGameOverText(playerSide + " Wins!");
        }
        restartButton.SetActive(true);
    }

    void ChangeSides()
    {
        playerSide = (playerSide == "X") ? "O" : "X";
        if(playerSide == "X")
        {
            SetPlayerColors(playerX,playerO);
        }
        else
        {
            SetPlayerColors(playerO,playerX);
        }
    }

    void SetGameOverText (string myText)
    {
        gameOverText.text = myText;
        gameOverPanel.SetActive(true);
    }

    public void RestartGame()
    {
        playerSide = "X";
        moveCount = 0;
        gameOverPanel.SetActive(false);

        for(int i = 0; i < buttonList.Length; i++)
        {
            buttonList[i].text = "";
        }
        SetPlayerColors(playerX,playerO);
        SetBoardInteractable(true);
        restartButton.SetActive(false);
    }

    void SetBoardInteractable(bool toggle)
    {
        for(int i = 0; i < buttonList.Length; i++)
        {
            buttonList[i].GetComponentInParent<Button>().interactable = true;
        }
    }

    void SetPlayerColors(Player newPlayer,Player oldPlayer)
    {
        newPlayer.panel.color = activePlayerColor.panelColor;
        newPlayer.text.color = activePlayerColor.textColor;

        oldPlayer.panel.color = inactivePlayerColor.panelColor;
        oldPlayer.text.color = inactivePlayerColor.textColor;
    }

    void ComputerTurn()
    {
        bool foundEmptySpot = false;
        while(!foundEmptySpot)
        {
            int randomNumber = Random.Range(0,9);
            if(buttonList[randomNumber].GetComponentInParent<Button>().IsInteractable())
            {
                buttonList[randomNumber].GetComponentInParent<Button>().onClick.Invoke();
                foundEmptySpot = true;
            }

        }
    }
}
