using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;

public class PhotonPlayer : MonoBehaviourPun, IPunObservable
{
    public new Rigidbody rigidbody;
    public float moveSpeed;
    public Vector2 rotateSpeed;
    public float maxLookUp;
    public float minLookUp;

    public Transform cameraTransform;
    public Transform cameraLookAtTarget;
    public PhotonAnimator photonAnimator;
    Vector3 direction = Vector3.zero;
    Vector3 lookDelta = Vector3.zero;

    public Transform bulletSpawnPoint;
    private static Hitbox bulletPrefab;
    private static Hitbox meleePrefab;
    private bool isAlive = true;
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        photonAnimator = GetComponent<PhotonAnimator>();
        cameraTransform.gameObject.SetActive(photonView.IsMine);
    }
    void Update()
    {
        if (photonView.IsMine && isAlive)
        {
            direction.x = Input.GetAxisRaw("Horizontal");
            direction.z = Input.GetAxisRaw("Vertical");
            direction = direction.normalized;
            lookDelta.x = Input.GetAxis("Mouse X");
            lookDelta.y = Input.GetAxis("Mouse Y");
            if (Input.GetMouseButtonDown(0))
            {
                MeleeAttack();
            }
            else if (Input.GetMouseButtonDown(1))
            {
                RangeAttack();
            }
        }
    }
    [PunRPC]
    internal void Die()
    {
        isAlive = false;
        photonAnimator.photonView.RPC("SetTrigger", RpcTarget.All, "Die");
    }

    public void MeleeAttack()
    {
        photonAnimator.photonView.RPC("SetTrigger", RpcTarget.All, "Attack2");
        //get weapon hitbox
        if (meleePrefab == null)
            meleePrefab = Resources.Load<Hitbox>("Melee");
        var hitbox = Instantiate(meleePrefab, bulletSpawnPoint.position, transform.rotation);
        hitbox.SetDamageData(10f);
        hitbox.SetActiveTime(0.3f, 0.2f);
        hitbox.transform.localScale = Vector3.one;
    }
    public void RangeAttack()
    {
        photonAnimator.photonView.RPC("SetTrigger", RpcTarget.All, "Attack1");
        photonView.RPC("SpawnBullet", RpcTarget.All, bulletSpawnPoint.position, transform.forward);
    }
    [PunRPC]
    public void SpawnBullet(Vector3 spawnPosition, Vector3 direction)
    {
        if (bulletPrefab == null)
            bulletPrefab = Resources.Load<Hitbox>("Bullet");
        var bullet = Instantiate(bulletPrefab, spawnPosition, transform.rotation);
        bullet.SetDamageData(10f);
        bullet.SetMoveDirection(3, direction);
        bullet.transform.localScale = Vector3.one;
    }

    private void FixedUpdate()
    {
        if (!photonView.IsMine || !isAlive)
            return;
        SetWalkAnimation(direction.sqrMagnitude);
        var moveDirecton = transform.right * direction.x + transform.forward * direction.z;
        transform.position = (rigidbody.position + moveDirecton * moveSpeed * Time.fixedDeltaTime);
        transform.Rotate(Vector3.up, lookDelta.x * rotateSpeed.x * Time.fixedDeltaTime);
        var newLookAtPosition = cameraLookAtTarget.localPosition;
        newLookAtPosition.y += lookDelta.y * rotateSpeed.y * Time.fixedDeltaTime;
        newLookAtPosition.y = Mathf.Clamp(newLookAtPosition.y, minLookUp, maxLookUp);
        cameraLookAtTarget.localPosition = newLookAtPosition;
    }
    private void SetWalkAnimation(float magnitude)
    {
        photonAnimator.photonView.RPC("SetFloat", RpcTarget.All, "walkSpeed", magnitude);
    }
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        //if (photonView.IsMine)
        //    return;
        /*if (stream.IsWriting)
        {
            stream.SendNext(direction.x);
            stream.SendNext(direction.z);
        }
        else
        {
            direction.x = (float)stream.ReceiveNext();
            direction.z = (float)stream.ReceiveNext();
            SetWalkAnimation(direction.sqrMagnitude);
        }*/
    }
}
public struct DamageData
{
    public string source;
    public float damage;
}
