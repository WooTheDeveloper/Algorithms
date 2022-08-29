using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
/// <summary>
/// 格子类型
/// </summary>
public enum NodeType
{
    walk,//可以走
 
    stop,//不能走
}
[Serializable]
public class AStarNode
{
    //格子对象坐标
    public int x;
    public int y;
    //寻路消耗
    public float consume;
    //离起点的距离
    public float toBegining;
    //离终点的距离
    public float toEnd;
    //父对象
    public AStarNode father;
 
    public NodeType type;
 
    public AStarNode (int x,int y,NodeType type)
    {
        this.x = x;
        this.y = y;
        this.type = type;
    }
 
}