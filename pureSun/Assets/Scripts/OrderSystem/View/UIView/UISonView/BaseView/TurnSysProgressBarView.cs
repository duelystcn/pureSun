
using Assets.Scripts.OrderSystem.Common.UnityExpand;
using Assets.Scripts.OrderSystem.Model.Circuit.QuestStageCircuit;
using Assets.Scripts.OrderSystem.Model.Database.GameModelInfo;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.OrderSystem.View.UIView.UISonView.BaseView
{
    public class TurnSysProgressBarView : ViewBaseView
    {
        public TurnSysOneStageView turnSysOneStagePrefab;

        public List<TurnSysOneStageView> turnSysOneStageViewList = new List<TurnSysOneStageView>();

        public void LoadingAllStageInfoToChange(QuestStageCircuitItem questStageCircuitItem,string thisViewPlayerCode, UnityAction turnStageCallBack) {
            for (int n = 0; n < questStageCircuitItem.questOneTurnStageList.Count; n++)
            {
                //是当前回合
                bool isCurrent = false;
                GM_OneStageSite gM_OneStageSite = questStageCircuitItem.questOneTurnStageList[n];
                if (gM_OneStageSite.code == questStageCircuitItem.oneTurnStage.code) {
                    isCurrent = true;
                }
                //是否拥有主动权
                bool isInitiative = false;
                if (questStageCircuitItem.turnHavePlayerCode == thisViewPlayerCode)
                {
                    if (gM_OneStageSite.operatingPlayer == "Myself")
                    {
                        isInitiative = true;
                    }
                    else
                    {
                        isInitiative = false;
                    }
                }
                else {
                    if (gM_OneStageSite.operatingPlayer == "Myself")
                    {
                        isInitiative = false;
                    }
                    else
                    {
                        isInitiative = true;
                    }
                }
                foreach (TurnSysOneStageView turnSysOneStageView in turnSysOneStageViewList) {
                    if (turnSysOneStageView.gM_OneStageSite.code == gM_OneStageSite.code) {
                        turnSysOneStageView.LoadingOneStageInfo(gM_OneStageSite, isInitiative, isCurrent);
                    } 
                }
            }
            StartCoroutine(ShowOverAction(turnStageCallBack));
        }

        public override void InitViewForParameter(UIControllerListMediator mediator, object body, Dictionary<string, string> parameterMap)
        {
            List<GM_OneStageSite> questOneTurnStageList = body as List<GM_OneStageSite>;
            Vector3 postion = this.transform.position;
            postion.x = postion.x - 8.47f;
            this.transform.position = postion;
            //头占10像素
            int oneStageHight = 300 / questOneTurnStageList.Count;
            for (int n = 0; n < questOneTurnStageList.Count; n++) {
                TurnSysOneStageView turnSysOneStageView = Instantiate<TurnSysOneStageView>(turnSysOneStagePrefab);
                Vector3 position = new Vector3();
                position.y = 120 - oneStageHight * n;
                position.x = 50;
                turnSysOneStageView.transform.SetParent(transform, false);
                turnSysOneStageView.transform.localPosition = position;
                turnSysOneStageView.LoadingOneStageInfo(questOneTurnStageList[n], false, false);
                turnSysOneStageViewList.Add(turnSysOneStageView);

            }
        }

        public IEnumerator ShowOverAction(UnityAction callBack)
        {
            yield return new WaitForSeconds(1f);
            callBack();
        }

    }
}
