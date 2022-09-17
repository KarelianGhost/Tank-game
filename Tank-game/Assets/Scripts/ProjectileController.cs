using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    public float projectileSpeed;
    public float damage;
    public DestructableObject.Faction faction;

    private void Update()
    {
        DestroyOutOfBounds();
        Move();
    }
    private void Move()
    {
        transform.position += projectileSpeed * Time.deltaTime * transform.forward;
    }

    private void DestroyOutOfBounds()
    {
        if (GameInit.map.GetValue(transform.position) == -1)
        {
            Destroy(gameObject);
        }
    }

}
