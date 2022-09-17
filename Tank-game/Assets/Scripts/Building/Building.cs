using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Building : DestructableObject
{
    public BuildingBase body;
}

public class BuildingController : MonoBehaviour
{
    [SerializeField] private Building building = new Building();

    // Start is called before the first frame update
    protected virtual void Start()
    {
        transform.position = GameInit.map.GetCellCenterPosition(new MapLocation(building.pos.x, building.pos.y));
        GameInit.map.SetValue(new MapLocation(building.pos.x, building.pos.y), 1);

        if (building.body != null)
            LoadBuilding(building.body);
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (building.hp <= 0)
        {
            DestroyBuilding();
        }
    }
    protected virtual void DestroyBuilding()
    {
        GameInit.map.SetValue(building.pos, 0);
        Destroy(gameObject);
    }

    private void LoadBuilding(BuildingBase buildingBase)
    {
        building.hp = building.body.maxHp;
        foreach (Transform child in this.transform)
        {
            if (Application.isEditor)
                DestroyImmediate(child.gameObject);
            else
                Destroy(child.gameObject);
        }
        GameObject model = Instantiate(buildingBase.model);
        model.transform.SetParent(this.transform);
        model.transform.localPosition = new Vector3(0, 0.8f, 0);
        model.transform.rotation = Quaternion.identity;
    }
    private void TakeDamage(float damage)
    {
        building.hp -= damage;
    }

    protected void SetFaction(DestructableObject.Faction faction)
    {
        building.faction = faction;
    }

    protected void SetPosition(MapLocation pos)
    {
        building.pos = pos;
    }

    private void OnTriggerEnter(Collider other)
    {
        ProjectileController pController = other.GetComponent<ProjectileController>();
        if (pController != null)
        {
            if (pController.faction != building.faction)
                TakeDamage(pController.damage);
            Destroy(other.gameObject);
        }
    }
}
