using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class PhotonPlayer : MonoBehaviourPun , IPunObservable
{
    public new Rigidbody rigidbody;
    private float x;
    private float z;
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        if (photonView.IsMine)
        {

        }
    }

    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine)
        {
            x = Input.GetAxisRaw("Horizontal");
            z = Input.GetAxisRaw("Vertical");
        }
    }

    private void FixedUpdate()
    {
        if (photonView.IsMine)
            rigidbody.MovePosition(rigidbody.position + new Vector3(x, 0, z));
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        throw new System.NotImplementedException();
    }
}
