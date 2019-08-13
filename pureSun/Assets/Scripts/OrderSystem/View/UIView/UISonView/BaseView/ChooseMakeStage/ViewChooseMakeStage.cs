
using Assets.Scripts.OrderSystem.Common.UnityExpand;
using Assets.Scripts.OrderSystem.Model.Database.Card;
using Assets.Scripts.OrderSystem.View.UIView.UISonView.ComponentView;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.OrderSystem.View.UIView.UISonView.BaseView.ChooseMakeStage
{
    //复合选择界面
    public class ViewChooseMakeStage : ViewBaseView
    {
        public ViewChooseStage viewChooseStagePrefab;

        public Dictionary<VCSLayerSort, List<CardEntry>> sortCardEntryMap = new Dictionary<VCSLayerSort, List<CardEntry>>();

        public Dictionary<VCSLayerSort, ViewChooseStage> sortViewChosseMap = new Dictionary<VCSLayerSort, ViewChooseStage>();


        protected override void InitUIObjects()
        {
            base.InitUIObjects();

            //高度起点
            //第一组数据
            int x1 = -130;
            int y1 = 220;
            int spacing1 = 20;
            float scale1 = 0.6f;
            //第二组数据
            int x2 = -145;
            int y2 = 85;
            int spacing2 = 25;
            float scale2 = 0.65f;
            //第三组数据
            int x3 = -160;
            int y3 = -70;
            int spacing3 = 30;
            float scale3 = 0.7f;


            //预设出三组选项框
            for (int n = 0; n < 3; n++) {
                ViewChooseStage viewChooseStage = Instantiate<ViewChooseStage>(viewChooseStagePrefab);
                viewChooseStage.transform.SetParent(transform, false);
                viewChooseStage.CreateCardEntryForMake();
                if (n == 0) {
                    //设置位置
                    Vector3 position = new Vector3();
                    position.x = x1;
                    position.y = y1;
                    viewChooseStage.transform.localPosition = position;

                    //设置间距
                    HorizontalLayoutGroup horizontalLayoutGroup =  viewChooseStage.GetComponent<HorizontalLayoutGroup>();
                    horizontalLayoutGroup.spacing = spacing1;

                    //设置缩放
                    Vector3 scale = new Vector3();
                    scale.x = scale1;
                    scale.y = scale1;
                    scale.z = 1;
                    viewChooseStage.transform.localScale = scale;
                    viewChooseStage.layerSort = VCSLayerSort.Three;
                    sortViewChosseMap.Add(VCSLayerSort.Three, viewChooseStage);
                }
                if (n == 1)
                {
                    //设置位置
                    Vector3 position = new Vector3();
                    position.x = x2;
                    position.y = y2;
                    viewChooseStage.transform.localPosition = position;

                    //设置间距
                    HorizontalLayoutGroup horizontalLayoutGroup = viewChooseStage.GetComponent<HorizontalLayoutGroup>();
                    horizontalLayoutGroup.spacing = spacing2;

                    //设置缩放
                    Vector3 scale = new Vector3();
                    scale.x = scale2;
                    scale.y = scale2;
                    scale.z = 1;
                    viewChooseStage.transform.localScale = scale;
                    viewChooseStage.layerSort = VCSLayerSort.Second;
                    sortViewChosseMap.Add(VCSLayerSort.Second, viewChooseStage);
                }
                if (n == 2)
                {
                    //设置位置
                    Vector3 position = new Vector3();
                    position.x = x3;
                    position.y = y3;
                    viewChooseStage.transform.localPosition = position;

                    //设置间距
                    HorizontalLayoutGroup horizontalLayoutGroup = viewChooseStage.GetComponent<HorizontalLayoutGroup>();
                    horizontalLayoutGroup.spacing = spacing3;

                    //设置缩放
                    Vector3 scale = new Vector3();
                    scale.x = scale3;
                    scale.y = scale3;
                    scale.z = 1;
                    viewChooseStage.transform.localScale = scale;
                    viewChooseStage.layerSort = VCSLayerSort.First;
                    sortViewChosseMap.Add(VCSLayerSort.First, viewChooseStage);
                }
            }
            
        }
        //初始化一次读取三个
        public void LoadStartCardList(List<List<CardEntry>> cardEntries) {
            sortCardEntryMap.Add(VCSLayerSort.First, cardEntries[0]);
            sortCardEntryMap.Add(VCSLayerSort.Second, cardEntries[1]);
            sortCardEntryMap.Add(VCSLayerSort.Three, cardEntries[2]);
            ChangeOneChooseView(sortViewChosseMap[VCSLayerSort.First], sortCardEntryMap[VCSLayerSort.First]);
            ChangeOneChooseView(sortViewChosseMap[VCSLayerSort.Second], sortCardEntryMap[VCSLayerSort.Second]);
            ChangeOneChooseView(sortViewChosseMap[VCSLayerSort.Three], sortCardEntryMap[VCSLayerSort.Three]);

        }
        public void ChangeOneChooseView(ViewChooseStage viewChooseStage,List<CardEntry> cardEntries) {
            for (int n = 0; n < cardEntries.Count; n++) {
                cardEntries[n].layerSort = viewChooseStage.layerSort;
                viewChooseStage.cardIntactViews[n].card = cardEntries[n];
                viewChooseStage.cardIntactViews[n].LoadCard(cardEntries[n]);
            }

        }
        //获取一个新的卡牌列，将旧的依次向前推
        public void LoadNewCardList(List<CardEntry> cardEntries) {
            sortCardEntryMap[VCSLayerSort.First] = sortCardEntryMap[VCSLayerSort.Second];
            sortCardEntryMap[VCSLayerSort.Second] =  sortCardEntryMap[VCSLayerSort.Three];
            sortCardEntryMap[VCSLayerSort.Three] =  cardEntries;
            ChangeOneChooseView(sortViewChosseMap[VCSLayerSort.First], sortCardEntryMap[VCSLayerSort.First]);
            ChangeOneChooseView(sortViewChosseMap[VCSLayerSort.Second], sortCardEntryMap[VCSLayerSort.Second]);
            ChangeOneChooseView(sortViewChosseMap[VCSLayerSort.Three], sortCardEntryMap[VCSLayerSort.Three]);
        }
        //一张牌够买完成后渲染成已被购买
        public void ChangeOneCradForBuyed(CardEntry cardEntry) {
            List<CardEntry> cardEntries = sortCardEntryMap[cardEntry.layerSort];
            foreach (CardEntry card in cardEntries)
            {
                if (card.uuid == cardEntry.uuid)
                {
                    card.isBuyed = true;
                }
            }
            ViewChooseStage viewChooseStage = sortViewChosseMap[cardEntry.layerSort];
            foreach (CardIntactView cardIntactView in viewChooseStage.cardIntactViews) {
                if (cardIntactView.card.uuid == cardEntry.uuid) {
                    cardIntactView.LoadCard(cardEntry);
                }
            }

        }

    }
}
