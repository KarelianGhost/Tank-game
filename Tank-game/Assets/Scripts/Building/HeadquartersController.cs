using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadquartersController : BuildingController
{
    // Start is called before the first frame update
    protected override void Start()
    {
        SetPosition(GameInit.map.GetHeadquartersPosition());
        base.Start();
        SetFaction(DestructableObject.Faction.ally);
    }
    
    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    protected override void DestroyBuilding()
    {
        Debug.Log("HQ destroyed. You Lost.");
        base.DestroyBuilding();
    }
}
