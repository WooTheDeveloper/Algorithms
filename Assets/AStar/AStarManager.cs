using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class AStarManager 
{
    private static AStarManager instance;
    public static AStarManager Instance
    {
        get
        {
            if (instance == null)
               instance = new AStarManager();
            return instance;
        }
    }
    //地图的宽高
    int mapW;
    int mapH;
 
    //地图相关所有的格子对象容器
    public  AStarNode[,] nodes;
    //开启列表和关闭列表
    private List<AStarNode> openList=new List<AStarNode>();
    private List<AStarNode> closeList=new List<AStarNode>() ;
 
    //初始化地图信息
    public  void InitMapInfo(int w,int h)
    {
        this.mapH = h;
        this.mapW = w;
        nodes = new AStarNode[w, h];
        for(int i=0;i<w;i++)
        {
            for(int j=0;j<h;j++)
            {
                //随机阻挡只是为了测试，真正的项目中阻挡信息应该是从地图的配置文件中读出来的
                AStarNode node = new AStarNode(i, j, Random.Range(0, 100) < 20 ? NodeType.stop: NodeType.walk);
                nodes[i, j] = node;
            }
        }
    }
 
    public  List<AStarNode> FindPath(Vector2 startPos,Vector2 endPos)
    {
        //实际项目中，传入的点往往是世界坐标系的位置，可能是小数，需要转化成整数也就是转化成每个格子的坐标，这里默认传入的数据为整数
        //首先判断传入的两个点是否合法
        //1.判断是否在地图内
        if (startPos.x < 0 || startPos.x >= mapW || startPos.y < 0 || startPos.y >= mapH || endPos.x < 0 || endPos.x >= mapW || endPos.y < 0 || endPos.y >= mapH)
        {
            Debug.Log("开始或者结束点在地图外");
                 return null;
        }
        //2.判断是否是阻挡
        AStarNode startNode = nodes[(int)startPos.x,(int)startPos.y];
        AStarNode endNode = nodes[(int)endPos.x, (int)endPos.y];
        if (startNode.type == NodeType.stop || endNode.type == NodeType.stop)
        {
            Debug.Log("开始或者结束点是阻挡");
            return null;
        }
 
        openList.Clear();
        closeList.Clear();
 
        startNode.father = null;
        startNode.toEnd = 0;
        startNode.toBegining = 0;
        startNode.consume = 0;
        closeList.Add(startNode);
 
        while(true)
        {
            //寻找四周的点
            //左上
            FindNearlyNodetoOpenList(startNode.x - 1, startNode.y + 1, 1.4f, startNode, endNode);
            //上
            FindNearlyNodetoOpenList(startNode.x, startNode.y + 1, 1f, startNode, endNode);
            //右上
            FindNearlyNodetoOpenList(startNode.x + 1, startNode.y + 1, 1.4f, startNode, endNode);
            //左
            FindNearlyNodetoOpenList(startNode.x - 1, startNode.y, 1f, startNode, endNode);
            //右
            FindNearlyNodetoOpenList(startNode.x + 1, startNode.y, 1f, startNode, endNode);
            //左下
            FindNearlyNodetoOpenList(startNode.x - 1, startNode.y - 1, 1.4f, startNode, endNode);
            //下
            FindNearlyNodetoOpenList(startNode.x, startNode.y - 1, 1f, startNode, endNode);
            //右下
            FindNearlyNodetoOpenList(startNode.x + 1, startNode.y - 1, 1.4f, startNode, endNode);
 
            //死路判断
            if (openList.Count==0)
            {
                return null;
            }
 
            openList.Sort(SortOpenList);
            startNode = openList[0];
 
            closeList.Add(openList[0]);
            openList.RemoveAt(0);
 
            if(startNode==endNode)
            {
                List<AStarNode> path = new List<AStarNode>();
                path.Add(endNode);
                while(endNode.father!=null)
                {
                    path.Add(endNode.father);
                    endNode = endNode.father;
                }
                path.Reverse();
                return path;
            }
        }
    }
     //将最近的点放进开始列表并计算寻路消耗
    private void FindNearlyNodetoOpenList(int x,int y,float toFather,AStarNode father,AStarNode end)
    {
        //边界判断
        if (x < 0 || x > mapW || y < 0 || y > mapH)
            return;
 
        AStarNode node = nodes[x, y];
        //判断是否是阻挡
        if (node == null || node.type == NodeType.stop || closeList.Contains(node) || openList.Contains(node))
            return;
 
        node.father = father;
        node.toBegining = father.toBegining + toFather;
        node.toEnd = Mathf.Abs(end.x - x) + Mathf.Abs(end.y - y);
        node.consume = node.toBegining + node.toEnd;
 
        openList.Add(node);
    }
    //开始列表排序
    private int SortOpenList(AStarNode a, AStarNode b)
    {
        if (a.consume > b.consume)
            return 1;
        else if (a.consume == b.consume)
            return 1;
        else
            return -1;
    }
}