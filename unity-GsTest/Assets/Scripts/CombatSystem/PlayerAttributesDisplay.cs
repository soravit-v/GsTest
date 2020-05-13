using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class PlayerAttributesDisplay : MonoBehaviour
{
    private PlayerAttributes playerAttributes;
    public RectTransform rectTransform;
    public TMP_Text nameTextMesh;
    public Image hpFill;
    public Image mpFill;
    public Image staminaFill;
    private bool followPlayer;

    private void Awake()
    {
    }
    public void SetName(string name)
    {
        nameTextMesh.text = name;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerAttributes == null)
            return;
        hpFill.fillAmount = playerAttributes.HpRatio;
        mpFill.fillAmount = playerAttributes.MpRatio;
        staminaFill.fillAmount = playerAttributes.StaminaRatio;
        if (followPlayer)
            FollowCharacter();
    }
    public void Init(PlayerAttributes attribute, bool followPlayer)
    {
        this.playerAttributes = attribute;
        this.followPlayer = followPlayer;
    }
    void FollowCharacter()
    {
        var screenPoint = Camera.main.WorldToScreenPoint(playerAttributes.transform.position);
        transform.position = screenPoint;
        //transform.position = playerAttributes.transform.position;
    }
}
