

using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.OrderSystem.View.UIView.UISonView.BaseView.PlayerComponent
{
    public class ShipComponentView : ViewBaseView
    {
        public TextMeshProUGUI myselfScore;
        public TextMeshProUGUI enemyScore;

        public string myselfPlayerCode;

        public void ChangeScoreShow(bool isMyself,int changeNum) {
            if (isMyself)
            {
                myselfScore.text = (Convert.ToInt32(myselfScore.text) + changeNum).ToString();
            }
            else {
                enemyScore.text = (Convert.ToInt32(enemyScore.text) + changeNum).ToString();
            }
        }
        public override void InitViewForParameter(UIControllerListMediator mediator, object body, Dictionary<string, string> parameterMap)
        {
            this.myselfPlayerCode = mediator.playerCode;
        }
    }
}
