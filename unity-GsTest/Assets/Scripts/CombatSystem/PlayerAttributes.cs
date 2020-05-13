using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttributes : MonoBehaviourPun, IPunObservable, IPunInstantiateMagicCallback
{
    public float maxHp;
    public float hp;
    public float HpRatio => hp / maxHp;
    public float hpRecoverySpeed;

    public float maxMp;
    public float mp;
    public float MpRatio => mp / maxMp;
    public float mpRecoverySpeed;

    public float maxStamina;
    public float stamina;
    public float StaminaRatio => stamina / maxStamina;
    public float staminaRecoverySpeed;
    private PhotonPlayer player;

    private void Start()
    {
        player = GetComponent<PhotonPlayer>();
        SetMaxValue();
    }
    public void SetMaxValue()
    {
        hp = maxHp;
        mp = maxHp;
        Debug.Log("Set hp " + hp);
        stamina = maxStamina;
    }
    [PunRPC]
    public void TakeDamage(float damage)
    {
        hp = Mathf.Max(0, hp - damage);
        if (hp == 0)
            player.photonView.RPC("Die", RpcTarget.All);
    }
    private void Update()
    {
        RecoverHp(Time.deltaTime);
        RecoverMp(Time.deltaTime);
        RecoverStamina(Time.deltaTime);
    }
    private void RecoverHp(float timePassed)
    {
        hp += timePassed * hpRecoverySpeed;
        hp = Mathf.Clamp(hp, 0, maxHp);
    }
    private void RecoverMp(float timePassed)
    {
        mp += timePassed * mpRecoverySpeed;
        mp = Mathf.Clamp(mp, 0, maxMp);
    }
    private void RecoverStamina(float timePassed)
    {
        stamina += timePassed * staminaRecoverySpeed;
        stamina = Mathf.Clamp(stamina, 0, maxStamina);
    }
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(hp);
            stream.SendNext(mp);
            stream.SendNext(stamina);
        }
        else
        {
            hp = (float)stream.ReceiveNext();
            mp = (float)stream.ReceiveNext();
            stamina = (float)stream.ReceiveNext();
        }
    }

    public void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        SetMaxValue();
        if (photonView.IsMine)
        {
            PlayerAttributesPanel.Instance.AddMainPlayerAttribute(this, photonView.Owner);
        }
        else
        {
            PlayerAttributesPanel.Instance.AddOtherPlayerAttribute(this, photonView.Owner);
        }
    }
}
