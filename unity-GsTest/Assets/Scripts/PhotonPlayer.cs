using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;
using UnityEngine.UI;
using PlayFab.ClientModels;

public class PhotonPlayer : MonoBehaviourPun, IPunObservable
{
    public new Rigidbody rigidbody;
    public new Collider collider;
    public float moveSpeed;
    public Vector2 rotateSpeed;
    public float maxLookUp;
    public float minLookUp;

    public Transform cameraTransform;
    public Transform cameraLookAtTarget;
    public PhotonAnimator photonAnimator;
    public PlayerAttributes playerAttributes;
    private Vector3 direction = Vector3.zero;
    private Vector3 lookDelta = Vector3.zero;

    public Transform bulletSpawnPoint;
    private WeaponData meleeData;
    private WeaponData rangeData;
    private bool isAlive = true;
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        collider = GetComponent<Collider>();
        photonAnimator = GetComponent<PhotonAnimator>();
        playerAttributes = GetComponent<PlayerAttributes>();
        cameraTransform.gameObject.SetActive(photonView.IsMine);
    }
    void Update()
    {
        if (!photonView.IsMine || !isAlive)
            return;
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
    public void MeleeAttack()
    {
        if (playerAttributes.TryConsumeStamina(meleeData.stamina))
        {
            photonAnimator.photonView.RPC("SetTrigger", RpcTarget.All, "Attack2");
            var hitbox = PhotonNetwork.Instantiate("Melee", bulletSpawnPoint.position, transform.rotation).GetComponent<Hitbox>();
            hitbox.SetOwner(gameObject);
            hitbox.SetDamageData(meleeData.damage);
            hitbox.SetActiveTime(0.3f, 0.2f);
            hitbox.transform.localScale = Vector3.one;
        }
    }
    public void RangeAttack()
    {
        if (playerAttributes.TryConsumeMana(rangeData.mana))
        {
            photonAnimator.photonView.RPC("SetTrigger", RpcTarget.All, "Attack1");
            var bullet = PhotonNetwork.Instantiate("Bullet", bulletSpawnPoint.position, transform.rotation).GetComponent<Hitbox>();
            bullet.SetOwner(gameObject);
            bullet.SetDamageData(rangeData.damage);
            bullet.SetMoveDirection(rangeData.speed, transform.forward);
            bullet.SetActiveTime(0.3f, 10f);
            bullet.transform.localScale = Vector3.one * rangeData.size;
        }
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
    public void SetEquipment(ItemInstance meleeWeapon, ItemInstance rangeWeapon)
    {
        meleeData = new WeaponData(meleeWeapon);
        rangeData = new WeaponData(rangeWeapon);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {

    }
    #region RPC
    [PunRPC]
    internal void Die()
    {
        isAlive = false;
        rigidbody.isKinematic = true;
        collider.enabled = false;
        photonAnimator.photonView.RPC("SetTrigger", RpcTarget.All, "Die");
        if (photonView.IsMine)
            GameStateManager.Next();
    }
    #endregion
}