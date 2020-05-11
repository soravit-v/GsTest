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

    private void Start()
    {
        SetMaxValue();
    }
    public void SetMaxValue()
    {
        hp = maxHp;
        mp = maxHp;
        Debug.Log("Set hp "+ hp);
        stamina = maxStamina;
    }
    public void TakeDamage(DamageData damageData)
    {
        hp = Mathf.Max(0, hp - damageData.damage);
        Debug.Log("Take damage has hp " + hp);
        //Die here
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
    }

    public void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        SetMaxValue();
        if (photonView.IsMine)
            return;
    }
}
