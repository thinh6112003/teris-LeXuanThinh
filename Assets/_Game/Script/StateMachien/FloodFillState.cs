using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloodFillState : IState
{
    public void OnEnter(GameManager gM)
    {
        gM.currentBlock = gM.nextBlock1;
        gM.nextBlock1 = gM.nextBlock2;
        gM.nextBlock2 = gM.nextBlock3;
        gM.nextBlock3 = (BlockType)Random.Range(0, 7);
        gM.FloodFill(gM.currentBlock);
        gM.listSolution = new List<PointXY>();
        gM.bestSolution = 0;
        gM.maxEatItem = 0;
        gM.bestSolutionType = 0;
        gM.fillvalue = 1;
        gM.UpdateNextBlock();
        for (int i = 0; i < 17; i++)
        {
            for (int j = 0; j < 7; j++)
            {
                if (gM.matrixSolution[i, j].Count > 0)
                {
                    PointXY pointTMP = new PointXY();
                    pointTMP.x = i;
                    pointTMP.y = j;
                    
                    gM.listSolution.Add(pointTMP);
                    for (int k = 0; k < gM.matrixSolution[i, j].Count; k++)
                    {
                        int eatItem = gM.CheckEatItem(gM.matrixSolution[i,j][k], i, j);
                        if (eatItem > 0)
                        {
                            if (eatItem > gM.maxEatItem)
                            {
                                gM.bestSolution = gM.listSolution.Count - 1;
                                gM.maxEatItem = eatItem;
                                gM.bestSolutionType = k;
                            }
                        }
                    }
                }
            }
        }
        gM.ChangeState(new SuggestState());
    }

    public void OnExecute(GameManager gM)
    {
    }

    public void OnExit(GameManager gM)
    {
    }
}
