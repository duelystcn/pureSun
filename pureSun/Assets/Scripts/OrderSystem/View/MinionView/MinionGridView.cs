using Assets.Scripts.OrderSystem.Common;
using Assets.Scripts.OrderSystem.Common.UnityExpand;
using Assets.Scripts.OrderSystem.Event;
using Assets.Scripts.OrderSystem.Metrics;
using Assets.Scripts.OrderSystem.Model.Hex;
using Assets.Scripts.OrderSystem.Model.Minion;
using Assets.Scripts.OrderSystem.View.UIView;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.OrderSystem.View.MinionView
{
    public class MinionGridView : MonoBehaviour
    {
        
        public Dictionary<int, MinionCellView> minionCellViews = new Dictionary<int, MinionCellView>();
        public MinionCellView cellPrefab;
        MinionMesh minionMesh;
        Canvas gridCanvas;
 

        public Material effectTargetMaterial;
        public Material originalMaterial;

        void Awake()
        {
            gridCanvas = GetComponentInChildren<Canvas>();
            minionMesh = GetComponentInChildren<MinionMesh>();

        }

        //初始化
        public void AchieveMinionGrid(MinionGridItem minionGridItem, HexGridItem hexGridItem, MinionGridMediator minionGridMediator) {
            Dictionary<int, MinionCellItem>.KeyCollection keyCol = minionGridItem.minionCells.Keys;
            foreach (int key in keyCol)
            {
                MinionCellItem minionCellItem = minionGridItem.minionCells[key];
                AchieveOneMinion(minionCellItem, hexGridItem, minionGridMediator);
            }
            //渲染需要放在格子生成完毕后
            //minionMesh.Triangulate(minionCellViews, hexGridItem.modelInfo.arrayMode);
        }
        public void AchieveOneMinion(MinionCellItem minionCellItem, HexGridItem hexGridItem, MinionGridMediator minionGridMediator)
        {
            Vector3 position = new Vector3();
            position = MinionMetrics.erectPosition(position, hexGridItem.cells[minionCellItem.index].X, hexGridItem.cells[minionCellItem.index].Z, hexGridItem.modelInfo.arrayMode);
            position.y = 1f;
            //创建一个生物实例
            MinionCellView cell = Instantiate<MinionCellView>(cellPrefab);
            cell.transform.SetParent(transform, false);
            cell.transform.localPosition = position;
            cell.minionCellItem = minionCellItem;
            TextMeshProUGUI atkAndDef = UtilityHelper.FindChild<TextMeshProUGUI>(cell.transform, "MinionCellLabel");
            atkAndDef.text = minionCellItem.cardEntry.atk.ToString() + "-" + minionCellItem.cardEntry.def.ToString();
            //添加绑定信息
            cell.OnPointerEnter = () =>
            {
                minionGridMediator.SendNotification(
                                                UIViewSystemEvent.UI_VIEW_CURRENT,
                                                cell,
                                                StringUtil.GetNTByNotificationTypeAndUIViewNameAndOtherTypeAndDelayedProcess(
                                                    UIViewSystemEvent.UI_VIEW_CURRENT_OPEN_ONE_VIEW,
                                                    UIViewConfig.getNameStrByUIViewName(UIViewName.OneCardAllInfo),
                                                    "MinionCellView",
                                                    "N"
                                                    )
                                                );
            };
            cell.OnPointerExit = () =>
            {
                minionGridMediator.SendNotification(
                                            UIViewSystemEvent.UI_VIEW_CURRENT,
                                            UIViewConfig.getNameStrByUIViewName(UIViewName.OneCardAllInfo),
                                            StringUtil.GetNTByNotificationTypeAndDelayedProcess(
                                                UIViewSystemEvent.UI_VIEW_CURRENT_CLOSE_ONE_VIEW,
                                                "N"
                                                )
                                            ); 
            };
            minionCellViews.Add(minionCellItem.index, cell);
            UtilityLog.Log("生成一个生物：" + minionCellItem.cardEntry.cardInfo.name);
        }
        //重新渲染部分生物
        public void RenderSomeMinionByMinionCellItem(List<MinionCellItem> mList) {
            foreach (MinionCellView minCellView in minionCellViews.Values) {
                foreach (MinionCellItem minCellItem in mList) {
                    if (minCellView.minionCellItem.uuid == minCellItem.uuid) {
                        RenderOneMinionCellByMinionCellItem(minCellView, minCellItem);
                        break;
                    }
                }
            }
        }
        public void RenderOneMinionCellByMinionCellItem(MinionCellView minionCellView, MinionCellItem minionCellItem) {
            minionCellView.minionCellItem = minionCellItem;
            TextMeshProUGUI atkAndDef = UtilityHelper.FindChild<TextMeshProUGUI>(minionCellView.transform, "MinionCellLabel");
            string atkStr = "";
            string defStr = "";
            if (minionCellItem.atkNow > minionCellItem.cardEntry.atk)
            {
                atkStr = "<color=\"green\">" + minionCellItem.atkNow;
            }
            else if (minionCellItem.atkNow == minionCellItem.cardEntry.atk)
            {
                atkStr = "<color=\"black\">" + minionCellItem.atkNow;
            }
            else {
                atkStr = "<color=\"'red\">" + minionCellItem.atkNow;
            }
            if (minionCellItem.defNow - minionCellItem.cumulativeDamage > minionCellItem.cardEntry.def)
            {
                defStr = "<color=\"green\">" + minionCellItem.defNow;
            }
            else if (minionCellItem.defNow - minionCellItem.cumulativeDamage == minionCellItem.cardEntry.def)
            {
                defStr = "<color=\"black\">" + minionCellItem.defNow;
            }
            else
            {
                defStr = "<color=\"red\">" + minionCellItem.defNow;
            }

            atkAndDef.text = atkStr + "<color=\"black\">-" + defStr;
            Component minionCellBg = UtilityHelper.FindChild<Component>(minionCellView.transform, "MinionCellBg");
            Image imageComponent = minionCellBg.GetComponent<Image>();
            if (minionCellItem.IsEffectTarget == false)
            {
                imageComponent.material = null;
            }
            else {
                imageComponent.material = effectTargetMaterial;
            }
        }

    }
}
