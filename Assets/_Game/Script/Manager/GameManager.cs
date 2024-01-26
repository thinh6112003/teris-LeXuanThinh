using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class GameManager : Singleton<GameManager>
{
    public int[,] matrix;
    public List<PointXY> listSolution;
    public List<bool[,]> visited ;
    public List<int>[,] matrixSolution;
    public GameObject block;
    public GameObject block2;
    public PointXY startPoint= new PointXY();
    public BlockType currentBlock;
    public BlockType nextBlock1;
    public BlockType nextBlock2;
    public BlockType nextBlock3;
    public BlockSO blockSO;
    public TextMeshProUGUI score;
    public TextMeshProUGUI highScore;
    public IState currentState;
    public float fillvalue = 1;
    public int rows, cols;
    public int maxEatItem = 0;
    public int bestSolution = 0;
    public int bestSolutionType = 0;
    public void Awake()
    {
        //tranh viec nguoi choi cham da diem vao man hinh
        Input.multiTouchEnabled = false;
        //target frame rate ve 60 fps
        Application.targetFrameRate = 60;
        //tranh viec tat man hinh
        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        //xu tai tho
        int maxScreenHeight = 1280;
        float ratio = (float)Screen.currentResolution.width / (float)Screen.currentResolution.height;
        if (Screen.currentResolution.height > maxScreenHeight)
        {
            Screen.SetResolution(Mathf.RoundToInt(ratio * (float)maxScreenHeight), maxScreenHeight, true);
        }
        Init();
        ChangeState(new FloodFillState());
    }
    private void Update()
    {
        if (currentState != null)
        {
            currentState.OnExecute(this);
        }
    }
    public void Init()
    {
        startPoint.x = 16;
        startPoint.y = 3;
        rows = 7;
        cols = 17;
        matrix = new int[cols,rows];
        for(int i = 0; i < cols; i++)
        {
            for(int j = 0; j < rows; j++)
            {
                RaycastHit2D hit = Physics2D.Raycast(new Vector2(j, i), Vector2.zero);
                if (hit.collider != null)
                {
                    GameManager.Destroy(hit.collider.gameObject);
                }
            }
        }
        nextBlock1 =(BlockType) Random.Range(0, 7);
        nextBlock2 =(BlockType) Random.Range(0, 7);
        nextBlock3 =(BlockType) Random.Range(0, 7);
    }

    struct Point
    {
        public int x;
        public int y;
        public int type;
        public Point(int x, int y, int type)
        {
            this.x = x;
            this.y = y;
            this.type = type;
        }
    }
    public void FloodFill(BlockType blockType)
    {
        matrixSolution = new List<int>[17, 7];
        for (int i = 0; i < cols; i++)
        {
            for (int j = 0; j < rows; j++)
            {
                matrixSolution[i, j] = new List<int>();
            }
        }
        visited = new List<bool[,]>();
        for (int i = 0; i < 4; i++)
        {
            bool[,] t = new bool[cols, rows];
            visited.Add(t);
        }
        Queue<Point> queue = new Queue<Point>();
        visited[0][16,3] = true;
        Point pointTMP = new Point(16, 3, 0);
        queue.Enqueue(pointTMP);

        while (queue.Count != 0)
        {
            pointTMP = queue.Dequeue();
            int i = pointTMP.x;
            int j = pointTMP.y;
            int typetmp = pointTMP.type;
            int incType()
            {
                int tmp = pointTMP.type + 1;
                if (tmp == 4) return 0;
                return tmp;
            }
            if (!visited[incType()][i, j] && Check(blockType, i, j, incType()))
            {
                visited[incType()][i, j] = true;
                queue.Enqueue(new Point(i, j, incType()));
            }
            if (!(j + 1 >= rows) && !visited[typetmp][i, j + 1] && Check(blockType, i, j + 1, typetmp))
            {
                visited[typetmp][i, j + 1] = true;
                queue.Enqueue(new Point(i, j + 1, typetmp));
            }
            if (!(j - 1 < 0) && !visited[typetmp][i, j - 1] && Check(blockType, i, j - 1, typetmp))
            {
                visited[typetmp][i, j - 1] = true;
                queue.Enqueue(new Point(i, j - 1, typetmp));
            }
            if (!(i - 1 < 0) && !visited[typetmp][i - 1, j] && Check(blockType, i - 1, j, typetmp))
            {
                visited[typetmp][i - 1, j] = true;
                queue.Enqueue(new Point(i - 1, j, typetmp));
            }
        }
    }
    public bool Check(BlockType blockType, int x, int y, int type)
    {
        List<BlockUnit> listBlockUnit = blockSO.listBlockSO[(int)blockType].listBlockItemOfType[type].listBlockUnit;
        for (int i = 0; i < listBlockUnit.Count; i++)
        {
            int xtmp = x + listBlockUnit[i].x;
            int ytmp = y + listBlockUnit[i].y;
            if (xtmp < 0 || ytmp < 0 || ytmp >= rows || xtmp >= cols || matrix[xtmp, ytmp] == 1) return false;
        }
        //if (type == 1)
        //{
        //    GameObject t = Instantiate(block2);
        //    t.transform.position = new Vector3(y, x, 0);
        //}
        for (int i = 0; i < listBlockUnit.Count; i++)
        {
            int xtmp = x + listBlockUnit[i].x - 1;
            int ytmp = y + listBlockUnit[i].y;
            if (xtmp == -1 || matrix[xtmp, ytmp] == 1)
            {
                matrixSolution[x, y].Add(type);
                break;
            }
        }
        return true;
    }
    public int CheckEatItem(int type, int x, int y)
    {
        BlockItemOfType blockItemOfType = blockSO.listBlockSO[(int)currentBlock].listBlockItemOfType[type];
        for (int i = 0; i < 4; i++)
        {
            BlockUnit blockUnit = blockItemOfType.listBlockUnit[i];
            matrix[x+blockUnit.x, y+ blockUnit.y ] = 1;
        }
        int dem = 0;
        for (int i = blockItemOfType.lowerPoint; i <= blockItemOfType.upperPoint; i++)
        {
            int xtmp = i + x;
            bool check = true;
            for (int j = 0; j < 7; j++)
            {
                if (matrix[xtmp, j] == 0)
                {
                    check = false; 
                    for (int k= 0; k < 4; k++)
                    {
                        BlockUnit blockUnit = blockItemOfType.listBlockUnit[k];
                        matrix[blockUnit.x + x, blockUnit.y + y] = 0;
                    }
                    break;
                }
            }
            if (check) dem++;
        }
        for (int i = 0; i < 4; i++)
        {
            BlockUnit blockUnit = blockItemOfType.listBlockUnit[i];
            matrix[blockUnit.x + x, blockUnit.y + y] = 0;
        }
        return dem;
    }
    public List<int> EatItem(int type, int x, int y)
    {
        List<int> Eat = new List<int>();
        BlockItemOfType blockItemOfType = blockSO.listBlockSO[(int)currentBlock].listBlockItemOfType[type];
        int dem = 0;
        for (int i = blockItemOfType.lowerPoint; i <= blockItemOfType.upperPoint; i++)
        {
            int xtmp = i + x;
            bool check = true;
            for (int j = 0; j < 7; j++)
            {
                if (matrix[xtmp, j] == 0)
                {
                    check = false;
                    break;
                }
            }
            if (check)
            {
                dem++;
                Eat.Add(xtmp);
            }
        }
        return Eat;
    }
    public void ChangeState(IState newState)
    {
        if (currentState != null)
        {
            currentState.OnExit(this);
        }
        currentState = newState;
        if (currentState != null)
        {
            currentState.OnEnter(this);
        }
    }
}
public class PointXY {
    public int x;
    public int y;
}

public enum BlockType
{
    BLOCK1,
    BLOCK2,
    BLOCK3,
    BLOCK4,
    BLOCK5,
    BLOCK6,
    BLOCK7,
}
