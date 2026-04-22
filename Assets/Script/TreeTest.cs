using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.Android.Gradle;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TreeTest : MonoBehaviour
{
    public GameObject prefab;
    public GameObject lineRenderer;
    public Transform treecanvas;
    private float delay = 2f;
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
        var queue = new Queue<(TreeNode<TKey,TValue>node,int depth)>();
        queue.Enqueue((node, 0));
        while (queue.Count > 0)
        {
            var (root,depth) = queue.Dequeue ();
            int count = levels.Count;
            while(levels.Count <= depth)
            {
                levels[count++].Add(node);
            }
            levels[depth].Add(root);

            if(root.Left != null)queue.Enqueue((root.Left, depth+1));
            if(root.Right != null)queue.Enqueue((root.Right, depth+1));
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
    public void OnClickLevelOrder()
    {
        if (bst == null) return;
        AssignPositioinsLevelOrder(bst.root);
    }
}
