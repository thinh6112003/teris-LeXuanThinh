using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SuggestState : IState
{
    GameObject blocktmp;
    int type;
    public void OnEnter(GameManager gM)
    {
        if (gM.listSolution.Count == 0)
        {
            gM.ChangeState(new GameOverState());
        }
        else
        {
            PointXY pointXY = gM.listSolution[gM.bestSolution];
            GameObject block = gM.blockSO.listBlockSO[(int)gM.currentBlock].listBlockItemOfType[gM.matrixSolution[pointXY.x, pointXY.y][gM.bestSolutionType]].block;
            type = gM.matrixSolution[pointXY.x, pointXY.y][gM.bestSolutionType];
            blocktmp = GameManager.Instantiate(block);
            blocktmp.transform.position = new Vector2(pointXY.y, pointXY.x);    
        }
    }

    public void OnExecute(GameManager gM)
    {
        gM.fillvalue -= 0.004f;
        if( gM.fillvalue < 0.01)
        {
            gM.ChangeState(new GoDownState());
            GameManager.Destroy(blocktmp);
        }
        else
        {
            Image fill= blocktmp.GetComponent<Block>().image ;
            fill.fillAmount = gM.fillvalue;
        }
        if (Input.GetMouseButtonDown(0)) {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            GameManager.Destroy(blocktmp);
            if (hit.collider != null)
            {
                gM.ChangeState(new GoDownState());
            }
            else
            {
                if(gM.currentBlock== BlockType.BLOCK7)
                {
                    gM.bestSolution++; 
                    if (gM.bestSolution == gM.listSolution.Count)
                    {
                        gM.bestSolution = 0;
                    }
                }
                else
                    {
                    gM.bestSolutionType++;
                    PointXY pointXY = gM.listSolution[gM.bestSolution];
                    if (gM.bestSolutionType == gM.matrixSolution[pointXY.x, pointXY.y].Count)
                    {
                        gM.bestSolutionType = 0;
                        gM.bestSolution++;
                        if (gM.bestSolution == gM.listSolution.Count)
                        {
                            gM.bestSolution = 0;
                        }
                    }
                }
                gM.ChangeState(new SuggestState());
            }
        }
    }

    public void OnExit(GameManager gM)
    {

    }
}
