using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//来源
//https://blog.csdn.net/Mr_Dongzheng/article/details/124633566
//这是一个非常简陋的AStar的实现，主要讲了大致的思路，而且中间还有越界的错误，这点要注意
 
public class TestAStar : MonoBehaviour
{
    //左上角第一个立方体的位置
    public int beginX=-3;
    public int beginY=5;
 
    //之后每一个立方体的偏移位置
    public int offsetX=2;
    public int offsetY=2;
 
    //地图的宽高
    public int mapH=5;
    public int mapW=5;
 
    public Material red;
    public Material yello;
    public Material green;
    public Material normal;
    private Dictionary<string, GameObject> cubes = new Dictionary<string, GameObject>();
 
    //开始点，给它一个坐标为负的点
    public Vector2 beginPos = Vector2.right * -1;
    [SerializeField]
    private List<AStarNode> list = new List<AStarNode>();
 
    private void Start()
    {
        AStarManager.Instance.InitMapInfo(mapW, mapH);
 
        for (int i= 0; i < mapW;++i)
        {
            for(int j=0;j<mapH;++j)
            {
                GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
                obj.transform.position = new Vector3(beginX + i * offsetX, beginY +j* offsetY, 0);
                //名字
                obj.name = i + "_" + j;
                cubes.Add(obj.name, obj);
                AStarNode node = AStarManager.Instance.nodes[i, j];
                if(node.type==NodeType.stop)
                {
                    obj.GetComponent<MeshRenderer>().material = red;
                }
            }
        }
 
    }
    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            RaycastHit inFo;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
 
            if(Physics.Raycast(ray,out inFo,1000))
            {
                if(beginPos==Vector2.right*-1)
                {
                    if (list != null)
                    {
                        for (int i = 0; i < list.Count; i++)
                        {
                            cubes[list[i].x + "_" + list[i].y].GetComponent<MeshRenderer>().material = normal;
                        }
                    }
                    string[] strs = inFo.collider.gameObject.name.Split('_');
                    beginPos = new Vector2(int.Parse(strs[0]), int.Parse(strs[1]));
                    inFo.collider.gameObject.GetComponent<MeshRenderer>().material = yello;
                }
                else
                {
                    string[] strs = inFo.collider.gameObject.name.Split('_');
                    Vector2 endPos = new Vector2(int.Parse(strs[0]), int.Parse(strs[1]));
                   // inFo.collider.gameObject.GetComponent<MeshRenderer>().material = yello;
                    list = AStarManager.Instance.FindPath(beginPos, endPos);
                    if(list!=null)
                    {
                        for(int i=0;i<list.Count;i++)
                        {
                            cubes[list[i].x + "_" + list[i].y].GetComponent<MeshRenderer>().material = green;
                        }
                    }
                    cubes[(int)beginPos.x + "_" +(int) beginPos.y].GetComponent<MeshRenderer>().material = normal;
                    beginPos = Vector2.right * -1;
                }
            }
        }
    }
}