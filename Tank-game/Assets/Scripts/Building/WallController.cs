using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallController : BuildingController
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        SetFaction(MapObject.Faction.neutral);
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }
}
