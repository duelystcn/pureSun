using Assets.Scripts.OrderSystem.Common;
using Assets.Scripts.OrderSystem.Common.UnityExpand;
using Assets.Scripts.OrderSystem.Event;
using Assets.Scripts.OrderSystem.Metrics;
using Assets.Scripts.OrderSystem.Model.Common;
using Assets.Scripts.OrderSystem.Model.Hex;
using Assets.Scripts.OrderSystem.Model.Minion;
using Assets.Scripts.OrderSystem.View.UIView;
using OrderSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Assets.Scripts.OrderSystem.View.MinionView
{
    public class MinionGridView : MonoBehaviour
    {
        
        public Dictionary<HexCoordinates, MinionCellView> minionCellViews = new Dictionary<HexCoordinates, MinionCellView>();
        public MinionCellView cellPrefab;
        public ObjectPool<MinionCellView> minionCellPool;
        //MinionMesh minionMesh;
        //Canvas gridCanvas;


        public Material effectTargetMaterial;
        public Material originalMaterial;

        void Awake()
        {
            GameObject prefab = cellPrefab.gameObject;
            minionCellPool = new ObjectPool<MinionCellView>(prefab, "minionCellPool");
            //gridCanvas = GetComponentInChildren<Canvas>();
            //minionMesh = GetComponentInChildren<MinionMesh>();

        }

        //初始化
        public void AchieveMinionGrid(MinionGridItem minionGridItem, HexGridItem hexGridItem, MinionGridMediator minionGridMediator) {
            Dictionary<HexCoordinates, MinionCellItem>.KeyCollection keyCol = minionGridItem.minionCells.Keys;
            foreach (HexCoordinates key in keyCol)
            {
                MinionCellItem minionCellItem = minionGridItem.minionCells[key];
                AchieveOneMinion(minionCellItem, hexGridItem, minionGridMediator);
            }
            //渲染需要放在格子生成完毕后
            //minionMesh.Triangulate(minionCellViews, hexGridItem.modelInfo.arrayMode);
        }
        //添加一个生物实例展示
        public void AchieveOneMinion(MinionCellItem minionCellItem, HexGridItem hexGridItem, MinionGridMediator minionGridMediator)
        {
            Vector3 position = new Vector3();
            HexCoordinates showHexCoordinates = HexCoordinates.ReverseFromOffsetCoordinates(minionCellItem.index.X, minionCellItem.index.Z, hexGridItem.modelInfo.arrayMode);
            position = MinionMetrics.erectPosition(position, showHexCoordinates.X, showHexCoordinates.Z, hexGridItem.modelInfo.arrayMode);
            position.y = 1f;
            //创建一个生物实例
            MinionCellView cell = minionCellPool.Pop();
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
            UtilityLog.Log("生成一个生物：" + minionCellItem.cardEntry.cardInfo.name, LogUtType.Operate);
        }
        //指定生物死亡，从前端移除
        public void MinionIsDeadNeedRemove(MinionCellItem minionCellItemDead) {
            UnityAction callBack = () =>
            {
                minionCellPool.Push(minionCellViews[minionCellItemDead.index]);
            };
            StartCoroutine(DelayToInvokeDo(callBack,1f));
        }
        public static IEnumerator DelayToInvokeDo(UnityAction callBack, float delaySeconds)
        {
            yield return new WaitForSeconds(delaySeconds);
            callBack();
        }
        //生物像指定方向进行移动
        public void MinionMoveTargetHexCell(MinionCellItem minionCellItemNew, HexModelInfo hexModelInfo, UnityAction callBack) {
            minionCellViews.Add(minionCellItemNew.index, minionCellViews[minionCellItemNew.lastIndex]);
            minionCellViews.Remove(minionCellItemNew.lastIndex);
            foreach (MinionCellView minCellView in minionCellViews.Values)
            {
                if (minCellView.minionCellItem.uuid == minionCellItemNew.uuid)
                {
                    Vector3 startPosition = minCellView.transform.position;
                    Vector3 endPosition = new Vector3();
                    HexCoordinates showHexCoordinates = HexCoordinates.ReverseFromOffsetCoordinates(minionCellItemNew.index.X, minionCellItemNew.index.Z, hexModelInfo.arrayMode);
                    endPosition = HexMetrics.erectPosition(
                       endPosition,
                       showHexCoordinates.X,
                       showHexCoordinates.Z, hexModelInfo.arrayMode);
                    StartCoroutine(MoveMinionCellShowMove(minCellView, callBack, startPosition, endPosition));

                }
            } 
        }
        //移动动画
        public IEnumerator MoveMinionCellShowMove(MinionCellView minCellView, UnityAction callBack, Vector3 startPosition, Vector3 endPosition)
        {

            bool isNear = false;
            Vector3 position = new Vector3();
            position.y = 0;
            int xdirection = endPosition.x - startPosition.x == 0 ? 0 : endPosition.x - startPosition.x > 0 ? 1 : -1;
            int zdirection = endPosition.z - startPosition.z == 0 ? 0 : endPosition.z - startPosition.z > 0 ? 1 : -1;

            position.x = 8 * xdirection;
            position.y = 8 * zdirection;
            //到达目的点
            while (!isNear)
            {
                minCellView.transform.Translate(position * Time.deltaTime);
                if (Math.Abs(minCellView.transform.position.x - endPosition.x) < 0.5)
                {
                    if (Math.Abs(minCellView.transform.position.z - endPosition.z) < 0.5)
                    {
                        isNear = true;
                    }
                }
                yield return null;
            }
            //直接设置为目标点，避免有偏差
            Vector3 OverPosition = new Vector3(endPosition.x, minCellView.transform.position.y, endPosition.z);
            minCellView.transform.position = OverPosition;
            callBack();

        }

        //生物像指定方向发起一次攻击
        public void MinionAttackTargetIndex(MinionCellItem minionCellItem, HexModelInfo hexModelInfo, UnityAction callBack) {
            MinionCellView minCellView = minionCellViews[minionCellItem.index];
            Vector3 startPosition = minCellView.transform.position;
            Vector3 endPosition = new Vector3();
            HexCoordinates showHexCoordinates = HexCoordinates.ReverseFromOffsetCoordinates(minionCellItem.attackTargetIndex.X, minionCellItem.attackTargetIndex.Z, hexModelInfo.arrayMode);
            endPosition = HexMetrics.erectPosition(
                endPosition,
                showHexCoordinates.X,
                showHexCoordinates.Z, hexModelInfo.arrayMode);

            endPosition = new Vector3(
                startPosition.x + (endPosition.x - startPosition.x) / 4,
                startPosition.y + (endPosition.y - startPosition.y) / 4,
                startPosition.z + (endPosition.z - startPosition.z) / 4
                );
            StartCoroutine(MoveMinionCellShowAttack(minCellView, callBack, startPosition, endPosition));
           
            

        }
        //暂时先用一段移动的动画代替攻击动画
        //移动某个子节点到某个位置
        public IEnumerator MoveMinionCellShowAttack(MinionCellView minCellView, UnityAction callBack, Vector3 startPosition, Vector3 endPosition)
        {
            
            bool isNear = false;
            Vector3 position = new Vector3();
            position.y = 0;
            int xdirection = endPosition.x - startPosition.x == 0 ? 0 : endPosition.x - startPosition.x > 0 ? 1 : -1;
            int zdirection = endPosition.z - startPosition.z == 0 ? 0 : endPosition.z - startPosition.z > 0 ? 1 : -1;

            position.x = 8 * xdirection;
            position.y = 8 * zdirection;
            //先到达目的点
            while (!isNear)
            {
                minCellView.transform.Translate(position * Time.deltaTime);
                if (Math.Abs(minCellView.transform.position.x - endPosition.x) < 0.5)
                {
                    if (Math.Abs(minCellView.transform.position.z - endPosition.z) < 0.5)
                    {
                        isNear = true;
                    }
                }
                yield return null;
            }
            //在返回出发点
            isNear = false;
            position.x = -8 * xdirection;
            position.y = -8 * zdirection;
            while (!isNear)
            {
                minCellView.transform.Translate(position * Time.deltaTime);
                if (Math.Abs(minCellView.transform.position.x - startPosition.x) < 0.5)
                {
                    if (Math.Abs(minCellView.transform.position.z - startPosition.z) < 0.5)
                    {
                        isNear = true;
                    }
                }
                yield return null;
            }
            //直接设置为目标点，避免有偏差
            Vector3 OverPosition = new Vector3(startPosition.x, minCellView.transform.position.y, startPosition.z);
            minCellView.transform.position = OverPosition;
            callBack();

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

            string atkStr = minionCellItem.minionVariableAttributeMap.CheckCurrentValueIsBetterByCode("Atk");
            if (atkStr == "Good")
            {
                atkStr = "<color=\"green\">" + minionCellItem.minionVariableAttributeMap.GetValueByCodeAndType("Atk", VATtrtype.CurrentValue);
            }
            else if (atkStr == "Bad") {
                atkStr = "<color=\"red\">" + minionCellItem.minionVariableAttributeMap.GetValueByCodeAndType("Atk", VATtrtype.CurrentValue);
            }
            else if (atkStr == "NoChange")
            {
                atkStr = "<color=\"black\">" + minionCellItem.minionVariableAttributeMap.GetValueByCodeAndType("Atk", VATtrtype.CurrentValue);
            }
            string defStr = minionCellItem.minionVariableAttributeMap.CheckCurrentValueIsBetterByCode("Def");
            if (defStr == "Good")
            {
                defStr = "<color=\"green\">" + minionCellItem.minionVariableAttributeMap.GetValueByCodeAndType("Def", VATtrtype.CurrentValue);
            }
            else if (defStr == "Bad")
            {
                defStr = "<color=\"red\">" + minionCellItem.minionVariableAttributeMap.GetValueByCodeAndType("Def", VATtrtype.CurrentValue);
            }
            else if (defStr == "NoChange")
            {
                defStr = "<color=\"black\">" + minionCellItem.minionVariableAttributeMap.GetValueByCodeAndType("Def", VATtrtype.CurrentValue);
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
