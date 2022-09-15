using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class Node
{
    public MapLocation pos;
    public float G;
    public float H;
    public float F;
    public Node parent;

    public Node(MapLocation pos, float g, float h, float f, Node p)
    {
        this.pos = pos;
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

    void BeginSearch(MapLocation start, MapLocation goal)
    {
        done = false;

        startNode = new Node(start, 0, 0, 0, null);
        goalNode = new Node(goal, 0, 0, 0, null);

        open.Clear();
        closed.Clear();
        open.Add(startNode);
        lastPos = startNode;
    }

    void Search(Node thisNode)
    {
        //Debug.Log(thisNode.pos.x + ":" + thisNode.pos.y + "(" + goalNode.pos.x + ":" + goalNode.pos.y + ")");
        if (thisNode.pos.Equals(goalNode.pos))
        {
            done = true;
            return;
        }


        for (int dir = 0; dir < 4; dir++)
        {
            MapLocation neighbour = new MapLocation(0, 0);
            switch (dir)
            {
                case 0:
                    neighbour = new MapLocation(thisNode.pos.x + 1, thisNode.pos.y);
                    break;
                case 1:
                    neighbour = new MapLocation(thisNode.pos.x, thisNode.pos.y + 1);
                    break;
                case 2:
                    neighbour = new MapLocation(thisNode.pos.x - 1, thisNode.pos.y);
                    break;
                case 3:
                    neighbour = new MapLocation(thisNode.pos.x, thisNode.pos.y - 1);
                    break;
            }
            //if (GameInit.map.GetValue(neighbourX, neighbourY) == 1) continue;
            if (GameInit.map.GetValue(neighbour) == -1) continue;
            if (isClosed(neighbour)) continue;

            float G = Vector2.Distance(new Vector2(thisNode.pos.x, thisNode.pos.y), new Vector2(neighbour.x,neighbour.y)) + thisNode.G;
            float H = Vector2.Distance(new Vector2(neighbour.x, neighbour.y), new Vector2(goalNode.pos.x, goalNode.pos.y));
            float F = G + H;

            if (!UpdateMarker(neighbour, G, H, F, thisNode))
                open.Add(new Node(neighbour, G, H, F, thisNode));
        }

        open = open.OrderBy(p => p.F).ToList<Node>();
        Node pm = (Node)open.ElementAt(0);
        closed.Add(pm);

        open.RemoveAt(0);

        lastPos = pm;
    }

    bool isClosed(MapLocation pos)
    {
        foreach (Node p in closed)
        {
            if (p.pos.Equals(pos)) return true;
        }
        return false;
    }

    bool UpdateMarker(MapLocation pos, float g, float h, float f, Node prt)
    {
        foreach (Node p in open)
        {
            if (p.pos.Equals(pos))
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

    public List<Node> GetPath(MapLocation start, MapLocation target)
    {
        BeginSearch(start, target);
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
