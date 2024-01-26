using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverState : IState
{
    public void OnEnter(GameManager gM)
    {
        UIManager.Instance.turnOnGameOverPanel();
    }

    public void OnExecute(GameManager gM)
    {
        
    }

    public void OnExit(GameManager gM)
    {
        
    }
}
