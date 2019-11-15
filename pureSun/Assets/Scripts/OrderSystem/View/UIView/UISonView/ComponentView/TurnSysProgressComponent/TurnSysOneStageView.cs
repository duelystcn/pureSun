
using Assets.Scripts.OrderSystem.Common.UnityExpand;
using Assets.Scripts.OrderSystem.Model.Database.GameModelInfo;
using TMPro;
using UnityEngine;

public class TurnSysOneStageView : MonoBehaviour
{
    public GM_OneStageSite gM_OneStageSite;
    public void LoadingOneStageInfo(GM_OneStageSite gM_OneStageSite, bool isInitiative, bool isCurrent) {
        this.gM_OneStageSite = gM_OneStageSite;
        TextMeshProUGUI turnSysOneStageText = UtilityHelper.FindChild<TextMeshProUGUI>(this.transform, "TurnSysOneStageText");
        turnSysOneStageText.text = gM_OneStageSite.name;
        if (isInitiative)
        {
            turnSysOneStageText.color = Color.green;
        }
        else {
            turnSysOneStageText.color = Color.red;
        }
        if (isCurrent)
        {
            this.transform.localScale = new Vector3(1, 1, 1);
        }
        else {
            this.transform.localScale = new Vector3(0.7f, 0.7f, 1);
        }
    }
}
