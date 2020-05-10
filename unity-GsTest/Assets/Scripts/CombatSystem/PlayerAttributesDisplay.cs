using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class PlayerAttributesDisplay : MonoBehaviour
{
    public PlayerAttributes playerAttributes;
    public RectTransform rectTransform;
    public TMP_Text nameTextMesh;
    public Image hpFill;
    public Image mpFill;
    public Image staminaFill;
    
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
        FollowCharacter();
    }
    void FollowCharacter()
    {
        /*var screenPoint = Camera.main.WorldToScreenPoint(playerAttributes.transform.position);
        screenPoint.z = 0;
        rectTransform.position = screenPoint;*/
        //transform.position = playerAttributes.transform.position;
        transform.LookAt(Camera.main.transform);
    }
}
