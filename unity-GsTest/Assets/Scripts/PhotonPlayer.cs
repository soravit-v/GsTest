using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class PhotonPlayer : MonoBehaviourPun, IPunObservable
{
    public new Rigidbody rigidbody;
    public float moveSpeed;
    public Vector2 rotateSpeed;
    public Transform cameraTransform;
    public Transform cameraPivot;
    Vector3 direction = Vector3.zero;
    Vector3 lookDelta = Vector3.zero;
    Vector3 lastMousePos;
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        cameraTransform.gameObject.SetActive(photonView.IsMine);
        lastMousePos = Input.mousePosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine)
        {
            direction.x = Input.GetAxisRaw("Horizontal");
            direction.z = Input.GetAxisRaw("Vertical");
            lookDelta.x = Input.GetAxis("Mouse X");
            lookDelta.y = Input.GetAxis("Mouse Y");
            lastMousePos = Input.mousePosition;
        }
    }

    private void FixedUpdate()
    {
        if (!photonView.IsMine)
            return;
        var moveDirecton = transform.right * direction.x + transform.forward * direction.z;
        transform.position = (rigidbody.position + moveDirecton * moveSpeed * Time.fixedDeltaTime);
        transform.Rotate(Vector3.up, lookDelta.x * rotateSpeed.x * Time.fixedDeltaTime);
        cameraPivot.Rotate(Vector3.right, lookDelta.y * rotateSpeed.y * Time.fixedDeltaTime, Space.Self);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {

    }
}
