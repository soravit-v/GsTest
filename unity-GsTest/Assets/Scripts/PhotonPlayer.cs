using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class PhotonPlayer : MonoBehaviourPun, IPunObservable
{
    public new Rigidbody rigidbody;
    public float moveSpeed;
    public Vector2 rotateSpeed;
    public float maxLookUp;
    public float minLookUp;

    public Transform cameraTransform;
    public Transform cameraLookAtTarget;
    public Animator animator;
    Vector3 direction = Vector3.zero;
    Vector3 lookDelta = Vector3.zero;

    public Hitbox hitbox;
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        cameraTransform.gameObject.SetActive(photonView.IsMine);
    }
    void Update()
    {
        if (photonView.IsMine)
        {
            direction.x = Input.GetAxisRaw("Horizontal");
            direction.z = Input.GetAxisRaw("Vertical");
            direction = direction.normalized;
            lookDelta.x = Input.GetAxis("Mouse X");
            lookDelta.y = Input.GetAxis("Mouse Y");
            if (Input.GetMouseButtonDown(1))
            {
                MeleeAttack();
            }
            else if (Input.GetMouseButtonDown(2))
            {
                RangeAttack();
            }
        }
    }
    public void MeleeAttack()
    {
        animator.Play("Attack2");
        //get weapon hitbox
        hitbox.SetDamageData(new DamageData() { source = photonView.Owner.UserId, damage = 10f });
    }
    public void RangeAttack()
    {
        animator.Play("Attack1");
    }

    private void FixedUpdate()
    {
        if (!photonView.IsMine)
            return;
        animator.SetFloat("walkSpeed", direction.sqrMagnitude);
        var moveDirecton = transform.right * direction.x + transform.forward * direction.z;
        transform.position = (rigidbody.position + moveDirecton * moveSpeed * Time.fixedDeltaTime);
        transform.Rotate(Vector3.up, lookDelta.x * rotateSpeed.x * Time.fixedDeltaTime);
        var newLookAtPosition = cameraLookAtTarget.localPosition;
        newLookAtPosition.y += lookDelta.y * rotateSpeed.y * Time.fixedDeltaTime;
        newLookAtPosition.y = Mathf.Clamp(newLookAtPosition.y, minLookUp, maxLookUp);
        cameraLookAtTarget.localPosition = newLookAtPosition;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {

    }
}
public struct DamageData
{
    public string source;
    public float damage;
}
