using UnityEngine;

public class PriorityQueueTest : MonoBehaviour
{
    public void OnClickTest()
    {
        var pq = new PriorityQueue<Node, int>();
        pq.Enqueue(new Node("50"), 50);
        pq.Enqueue(new Node("60"), 60);
        pq.Enqueue(new Node("70"), 70);
        pq.Enqueue(new Node("40"), 40);
        pq.Enqueue(new Node("20"), 20);
        pq.Enqueue(new Node("10"), 10);
        Debug.Log($"현재 큐 개수: {pq.Count}");
        while (pq.Count > 0)
        {
            var current = pq.Dequeue();
            Debug.Log(current.Name);
        }
    }
}
