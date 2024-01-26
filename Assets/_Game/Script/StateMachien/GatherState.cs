using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GatherState : IState
{
    public List<Block> listBlockMove ;
    public void OnEnter(GameManager gM)
    {
        listBlockMove = new List<Block>();
        PointXY pointXY = gM.listSolution[gM.bestSolution];
        List<int> rowsEat=  gM.EatItem(gM.matrixSolution[pointXY.x, pointXY.y][gM.bestSolutionType], pointXY.x,pointXY.y);
        if(rowsEat.Count==0)
        {
            gM.ChangeState(new FloodFillState());
        }
        else
        {
            int j = 0,dem=0;
            for(int i = rowsEat[0]; i < 17; i++)
            {
                if (j<=rowsEat.Count-1 && i == rowsEat[j])
                {
                    dem++;
                    j++;
                    for (int k = 0; k < 7; k++)
                    {
                        RaycastHit2D hit = Physics2D.Raycast(new Vector2(k, i), Vector2.zero);
                        if (hit.collider != null)
                        {
                            GameManager.Destroy(hit.collider.gameObject);
                        }
                    }
                }
                else
                {
                    for(int k = 0; k < 7; k++)
                    {
                        gM.matrix[i-dem,k] = gM.matrix[i, k];
                        RaycastHit2D hit = Physics2D.Raycast(new Vector2(k, i), Vector2.zero);
                        if( hit.collider != null)
                        {
                            hit.collider.gameObject.GetComponent<Block>().exactPos = new Vector2(k, i - dem);
                            listBlockMove.Add(hit.collider.GetComponent<Block>());
                        }
                        if(i+ dem >= 17)
                        {
                            gM.matrix[i, k] = 0;
                        }
                    }
                }
            }
        }
    }

    public void OnExecute(GameManager gM)
    {
        bool checkComplete = true;
        for(int i= listBlockMove.Count-1; i>=0; i--)
        {
            if(listBlockMove[i].transform.position.y- listBlockMove[i].exactPos.y > 0.001)
            {
                checkComplete = false;
                listBlockMove[i].transform.position = Vector2.MoveTowards(listBlockMove[i].transform.position,
                    listBlockMove[i].exactPos, 0.05f);
            }
        }
        if (checkComplete) gM.ChangeState(new FloodFillState());
    }

    public void OnExit(GameManager gM)
    {
        
    }
}
