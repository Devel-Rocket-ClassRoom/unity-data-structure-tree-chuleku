using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.Android.Gradle;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;
public class TreeTest : MonoBehaviour
{
    public GameObject prefab;
    public GameObject lineRenderer;
    public Transform treecanvas;
    public float horizontalSpacing = 2.0f;
    public float verticalSpacing = 2.0f;
    private BinarySearchTree<int, int> bst = new BinarySearchTree<int, int>();
    private readonly Dictionary<object, Vector3> nodePosition = new();
    public void RandomPow()
    {
        for (int i = 0; i < 10; i++)
        {
            int rnum = Random.Range(1, 100);
            bst.Add(rnum, rnum);
        }
        Vector3 firstpos = new Vector3(Screen.width / 2, Screen.height - 100f, 0f);
        Tree(bst.root,firstpos , 200f, 50f);
    }
    public void Tree(TreeNode<int, int> node, Vector3 pos, float x, float y)
    {
        if (node == null) return;
        GameObject obj = Instantiate(prefab,pos,Quaternion.identity, treecanvas.transform);
        nodePosition[node] = obj.transform.position;
        obj.GetComponentInChildren<TextMeshProUGUI>().text = node.Value.ToString();
        if(node.Left !=null)
        {
            Vector3 lpos = pos + new Vector3(-x, -y, 0);
            LineRender(pos, lpos);
            Tree(node.Left, lpos, x*0.5f, y);
        }
        if(node.Right !=null)
        {
            Vector3 rpos = pos + new Vector3(x, -y, 0);
            LineRender(pos, rpos);
            Tree(node.Right, rpos, x*0.5f, y);
        }
    }
    public void LineRender(Vector3 obj,Vector3 subobj)
    {
        if (lineRenderer == null) return;
        GameObject line = Instantiate(lineRenderer, treecanvas.transform);
        line.transform.SetAsFirstSibling();
        Vector3 dir = (subobj - obj);
        Vector3 spos = obj + (dir * 50f);
        Vector3 epos = subobj + (dir * 50f);
        float distance = Vector3.Distance(spos, epos);
        float angle = Mathf.Atan2(epos.y-spos.y,epos.x-spos.x) * Mathf.Rad2Deg;

        RectTransform rect = line.GetComponent<RectTransform>();
        rect.position = obj;
        rect.sizeDelta = new Vector2(distance, 5f);
        rect.pivot = new Vector2(0, 0.5f);
        rect.rotation = Quaternion.Euler(0, 0, angle);

    }
    public void OnClear()
    {
        bst.Clear();
        foreach (Transform child in treecanvas)
        {
            Destroy(child.gameObject);
        }
    }
    public void AssignPositioinsLevelOrder<TKey,TValue>(TreeNode<TKey,TValue> node)
    {
        var levels = new List<List<TreeNode<TKey,TValue>>> ();
        Vector3 pos = new Vector3(Screen.width / 2, Screen.height - 100f, 0f);
        var queue = new Queue<(TreeNode<TKey,TValue>node,int depth,float xoff, Vector3? parentPos)>();
        queue.Enqueue((node, 0,pos.x,null));
        
        while (queue.Count > 0)
        {
            var (root,depth,x, parentPos) = queue.Dequeue ();
            float y = pos.y -(depth * 100f);
            Vector3 currentPos = new Vector3(x, y, 0f);
            GameObject obj = Instantiate(prefab, currentPos, quaternion.identity,treecanvas.transform);
            obj.GetComponentInChildren<TextMeshProUGUI>().text = root.Value.ToString();
            nodePosition[root] = currentPos;
            if (parentPos != null)
            {
                LineRender(parentPos.Value, currentPos);
            }
            while (levels.Count <= depth)
            {
                levels.Add(new List<TreeNode<TKey, TValue>>());
            }
            levels[depth].Add(root);

            float spread = 200f / Mathf.Pow(2, depth);
            if (root.Left != null)queue.Enqueue((root.Left, depth+1,x,currentPos));
            if(root.Right != null)queue.Enqueue((root.Right, depth+1,x+spread,currentPos));
        }
        for(int depth = 0; depth<levels.Count; depth++)
        {
            float y = -depth * 2f;
            var row =levels[depth];
            for(int i = 0; i < row.Count; i++)
            {
                nodePosition[row[i]] = new Vector3(nodePosition[node].x , y, 0f);
            }
        }
    }
    private void AssignPositionsInOrder<TKey, TValue>(TreeNode<TKey, TValue> node, int depth, ref int xIndex,Vector3 pos)
    {
        if (node == null) return;
        AssignPositionsInOrder(node.Left, depth + 1, ref xIndex,pos);
        float xPos = pos.x+(xIndex * horizontalSpacing);
        float yPos = 1080f-200f-(depth * verticalSpacing);
        Vector3 currentPos = new Vector3(xPos, yPos, 0);

        GameObject obj = Instantiate(prefab, currentPos, Quaternion.identity, treecanvas);
        obj.GetComponentInChildren<TextMeshProUGUI>().text = node.Value.ToString();
        nodePosition[node] = currentPos;
        xIndex++;

        AssignPositionsInOrder(node.Right, depth + 1, ref xIndex,pos);
    }
    public void OnClickLevelOrder()
    {
        OnClear();
        for (int i = 0; i < 10; i++)
        {
            int rnum = Random.Range(1, 100);
            bst.Add(rnum, rnum);
        }
        AssignPositioinsLevelOrder(bst.root);
    }
    public void OnClickInOrder()
    {
        OnClear();
        for (int i = 0; i < 10; i++)
        {
            int rnum = Random.Range(1, 100);
            bst.Add(rnum, rnum);
        }
        int x = 0;
        Vector3 firstpos = new Vector3(Screen.width / 2, Screen.height - 100f, 0f);
        AssignPositionsInOrder(bst.root, 0,ref x,firstpos);
    }
}
