using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
    private DamageData damageData;
    public bool destroyOnHit;
    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out PlayerAttributes playerAttributes))
            playerAttributes.TakeDamage(damageData);
        if (destroyOnHit)
            Destroy(gameObject);
    }
    public void SetDamageData(DamageData damageData)
    {
        this.damageData = damageData;
    }
}
