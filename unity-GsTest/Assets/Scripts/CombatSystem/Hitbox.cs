using System.Collections;
using UnityEngine;
using Photon.Pun;
public class Hitbox : MonoBehaviourPun
{
    private float damage;
    public bool disableOnHit;
    private Vector3 moveDirection;
    private float moveSpeed;
    private new Collider collider;
    private GameObject ownerObject;
    private void Awake()
    {
        collider = GetComponent<Collider>();
    }
    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == ownerObject || !photonView.IsMine)
            return;
        if (other.gameObject.TryGetComponent(out PlayerAttributes playerAttributes))
            playerAttributes.photonView.RPC("TakeDamage", RpcTarget.All, damage);
        if (disableOnHit)
            collider.enabled = false;
    }
    public void SetOwner(GameObject ownerObject)
    {
        this.ownerObject = ownerObject;
    }
    public void SetActiveTime(float spawnDelay, float lifeTime)
    {
        StartCoroutine(SetActiveTimeCoroutine(spawnDelay, lifeTime));
    }
    private IEnumerator SetActiveTimeCoroutine(float spawnDelay, float lifeTime)
    {
        collider.enabled = false;
        yield return new WaitForSeconds(spawnDelay);
        collider.enabled = true;
        yield return new WaitForSeconds(lifeTime);
        PhotonNetwork.Destroy(gameObject);
    }
    public void SetDamageData(float damage)
    {
        this.damage = damage;
    }
    public void SetMoveDirection(float speed, Vector3 direction)
    {
        moveSpeed = speed;
        moveDirection = direction.normalized;
    }
    private void Update()
    {
        if (moveSpeed > 0)
            transform.Translate(moveDirection * moveSpeed * Time.deltaTime, Space.World);
    }
}
