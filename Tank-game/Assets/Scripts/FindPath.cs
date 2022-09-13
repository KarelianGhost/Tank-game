using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class Node
{
    public int x;
    public int y;
    public float G;
    public float H;
    public float F;
    public Node parent;

    public Node(int x, int y, float g, float h, float f, Node p)
    {
        this.x = x;
        this.y = y;
        G = g;
        H = h;
        F = f;
        parent = p;
    }
}
public class FindPath
{
    private List<Node> path = new List<Node>();
    private List<Node> open = new List<Node>();
    private List<Node> closed = new List<Node>();

    private Node goalNode;
    private Node startNode;

    private Node lastPos;

    private bool done = false;

    void BeginSearch(int startX, int startY, int goalX, int goalY)
    {
        done = false;

        startNode = new Node(startX, startY, 0, 0, 0, null);
        goalNode = new Node(goalX, goalY, 0, 0, 0, null);

        open.Clear();
        closed.Clear();
        open.Add(startNode);
        lastPos = startNode;
    }

    void Search(Node thisNode)
    {
        
        if (thisNode.x == goalNode.x && thisNode.y == goalNode.y)
        {
            done = true;
            return;
        }


        for (int dir = 0; dir < 4; dir++)
        {
            int neighbourX = 0;
            int neighbourY = 0;
            switch (dir)
            {
                case 0:
                    neighbourX = thisNode.x + 1;
                    neighbourY = thisNode.y;
                    break;
                case 1:
                    neighbourX = thisNode.x;
                    neighbourY = thisNode.y + 1;
                    break;
                case 2:
                    neighbourX = thisNode.x - 1;
                    neighbourY = thisNode.y;
                    break;
                case 3:
                    neighbourX = thisNode.x;
                    neighbourY = thisNode.y - 1;
                    break;
            }
            //if (GameInit.map.GetValue(neighbourX, neighbourY) == 1) continue;
            if (GameInit.map.GetValue(neighbourX, neighbourY) == -1) continue;
            if (isClosed(neighbourX,neighbourY)) continue;

            float G = Vector2.Distance(new Vector2(thisNode.x, thisNode.y), new Vector2(neighbourX,neighbourY)) + thisNode.G;
            float H = Vector2.Distance(new Vector2(neighbourX, neighbourY), new Vector2(goalNode.x, goalNode.y));
            float F = G + H;

            if (!UpdateMarker(neighbourX, neighbourY, G, H, F, thisNode))
                open.Add(new Node(neighbourX,neighbourY, G, H, F, thisNode));
        }

        open = open.OrderBy(p => p.F).ToList<Node>();
        Node pm = (Node)open.ElementAt(0); //Баг: В некоторых ситуациях поиск пути вышибается в этом месте с жалобой на якобы не существующий индекс
        closed.Add(pm);

        open.RemoveAt(0);

        lastPos = pm;
    }

    bool isClosed(int x, int y)
    {
        foreach (Node p in closed)
        {
            if (p.x == x && p.y == y) return true;
        }
        return false;
    }

    bool UpdateMarker(int x, int y, float g, float h, float f, Node prt)
    {
        foreach (Node p in open)
        {
            if (p.x == x && p.y == y)
            {
                p.G = g;
                p.H = h;
                p.F = f;
                p.parent = prt;
                return true;
            }
        }
        return false;
    }

    public List<Node> GetPath(int startX, int startY, int targetX, int targetY)
    {
        BeginSearch(startX, startY, targetX, targetY);
        while (!done)
        {
            if (open.Count == 0)
            {
                Debug.Log("No path found!");
                return new List<Node>();
            }
            Search(lastPos);
        }

        Node begin = lastPos;

        while (!startNode.Equals(begin) && begin != null)
        {
            path.Add(begin);
            begin = begin.parent;
        }
        path.Add(begin);
        return path;
    }
}
