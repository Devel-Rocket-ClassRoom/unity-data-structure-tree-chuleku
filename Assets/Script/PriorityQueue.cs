using Mono.Cecil;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.VisualScripting;
using UnityEngine.Rendering;

public class Node
{
    public string Name;
    public Node(string name) {  Name = name; }
}

public class PriorityQueue<TElement, TPriority> where TPriority : IComparable<TPriority> 
{

/*    public int Count => heap.Count;
    
    private List<(TElement Element, TPriority Priority)> heap;
    private readonly IComparer<TPriority> comparer;

    public PriorityQueue()
    {
        heap = new <(TElement, TPriority)> ();
        
    }
    public void Enqueue(TElement element, TPriority priority)
    {
        heap.Add((element, priority));

        HeapifyUp(heap.Count);
    }
    private void HeapifyUp(int index)
    {
        while(index>0)
        {
            int parentIndex = (index-1)/2;
            if (comparer.Compare(heap[index].Priority, heap[parentIndex].Priority) >= 0 )
            {
                break;
            }
            Swap(index,parentIndex);
            index = parentIndex;
        }
    }

    private void Swap(int index,int parentindex)
    {
        int teamp = index;
    }
    public TElement Dequeue()
    {
        if(heap == null) return
        TElement result = heap[0].Element;
        int lastindex = heap.Count-1;
        heap[0] = heap[lastindex];
        heap.RemoveAt(lastindex);
        if(heap.Count >0)
        {
            HeapifyDwon(0);
        }
        return result
    }
    private void HeapifyDown(int index)
    {
        int lastIndex = heap.Count-1;

        while(true)
        {
            int leftchild = 2 * index + 1;
            int rightchild = 2 * index + 2;
            int current = index;

            if(leftchild <= lastIndex && comparer.Compare(heap[rightchild].Priority, heap[current].Priority)<0)
                {
                current = rightchild;
            }
            if()
        }
    }
    public TElement Peek();
    public void Clear();*/



    public int Count => _List.Count;
    private List<(TElement Element, TPriority Priority)> _List = new List<(TElement Element, TPriority Priority)>();
    public void Enqueue(TElement element, TPriority priority)
    {
        _List.Add((element, priority));
        int currentcount = _List.Count-1;
        while (currentcount > 0)
        {
            int p = (currentcount-1)/2;
            if (_List[currentcount].Priority.CompareTo(_List[p].Priority)<0)
            {
                var temp = _List[currentcount];
                _List[currentcount] = _List[p];
                _List[p] = temp;
                currentcount = p;
            }
            else
            {
                break;
            }
        }

    }
    public TElement Dequeue()
    {
        if (_List.Count == 0) return default;
        TElement root = _List[0].Element;
        _List[0] = _List[_List.Count - 1];
        _List.RemoveAt(_List.Count - 1);
        int current = 0;
        while (true)
        {
            int left = current * 2 + 1;
            int right = current * 2 + 2;
            int next = current;
            if (left < _List.Count && _List[left].Priority.CompareTo(_List[next].Priority) < 0)
            {
                next = left;
            }
            if (right < _List.Count && _List[right].Priority.CompareTo(_List[next].Priority) < 0)
            {
                next = right;
            }
            if (next == current)
            {
                break;
            }

            var temp = _List[current];
            _List[current] = _List[next];
            _List[next] = temp;
            current = next;
        }
        return root;
    }
    public TElement Peek()
    {
        return _List[0].Element;
    }
    public void Clear()
    {
        _List.Clear();
    }
}