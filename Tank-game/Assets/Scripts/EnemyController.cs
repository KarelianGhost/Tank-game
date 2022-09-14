using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : TankController
{
    List<Tank.Direction> dirPath = new List<Tank.Direction>();
    
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        StartCoroutine(TriggerPathFind());
    }

    // Update is called once per frame
    protected override void Update()
    {
        if (dirPath.Count > 0)
        {
            if (!CheckMovement())
            {
                if (CalculateMovement(dirPath[dirPath.Count - 1]) == 0)
                {
                    dirPath.RemoveAt(dirPath.Count - 1);
                }
            }
        }
        if (!CheckRotation())
        {
            TriggerShooting();
        }
        base.Update();
    }

    private void CalculatePath(int targetX, int targetY)
    {
        int posX = GetPositionX();
        int posY = GetPositionY();
        FindPath pathfind = new FindPath();
        dirPath = ConvertPathToDirections(pathfind.GetPath(posX, posY, targetX, targetY));
    }

    private List<Tank.Direction> ConvertPathToDirections(List<Node> path)
    {
        List<Tank.Direction> dirPath = new List<Tank.Direction>();
        foreach (Node n in path) {
            if (n.parent != null)
            {
                if (n.parent.x < n.x)
                {
                    dirPath.Add(Tank.Direction.right);
 
                } else if (n.parent.x > n.x)
                {
                    dirPath.Add(Tank.Direction.left);

                } else if (n.parent.y < n.y)
                {
                    dirPath.Add(Tank.Direction.top);

                } else if (n.parent.y > n.y)
                {
                    dirPath.Add(Tank.Direction.down);

                }
            }
        }
        return dirPath;
    }
    IEnumerator TriggerPathFind()
    {
        while (true)
        {
            int posX = Random.Range(0, 10);
            int posY = Random.Range(0, 10);
            Debug.Log("Target: " + posX + ":" + posY);
            CalculatePath(posX, posY);

            yield return new WaitForSeconds(5f);
        }
    }

    protected override void DestroyTank()
    {
        Debug.Log("Enemy killed");
        base.DestroyTank();
    }
}
