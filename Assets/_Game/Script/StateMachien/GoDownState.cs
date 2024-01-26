using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoDownState : IState
{
    float timer;
    public void OnEnter(GameManager gM)
    {
        timer = 0;
        PointXY pointXY = gM.listSolution[gM.bestSolution];
        BlockItemOfType blockItemOfType = gM.blockSO.listBlockSO[(int)gM.currentBlock].listBlockItemOfType[gM.matrixSolution[pointXY.x, pointXY.y][gM.bestSolutionType]];
        for(int i=0;i< 4; i++)
        {
            gM.matrix[pointXY.x + blockItemOfType.listBlockUnit[i].x, pointXY.y+ blockItemOfType.listBlockUnit[i].y] = 1;

            GameObject blockUnit = gM.blockSO.listBlockSO[(int)gM.currentBlock].blockUnit;
            GameObject blockTMP=  GameManager.Instantiate(blockUnit);
            blockTMP.transform.position = new Vector2(pointXY.y + blockItemOfType.listBlockUnit[i].y, 
                pointXY.x + blockItemOfType.listBlockUnit[i].x);
        }
    }

    public void OnExecute(GameManager gM)
    {
        timer += Time.deltaTime;
        if (timer > 0.3) gM.ChangeState(new GatherState());
    }

    public void OnExit(GameManager gM)
    {
        
    }
}
