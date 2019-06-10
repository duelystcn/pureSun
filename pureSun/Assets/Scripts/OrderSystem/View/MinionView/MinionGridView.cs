using Assets.Scripts.OrderSystem.Metrics;
using Assets.Scripts.OrderSystem.Model.Hex;
using Assets.Scripts.OrderSystem.Model.Minion;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.OrderSystem.View.MinionView
{
    public class MinionGridView : MonoBehaviour
    {
    
        public MinionCellView[] minionCellViews;
        public MinionCellView cellPrefab;
        public Text cellLabelPrefab;
        MinionMesh minionMesh;
        Canvas gridCanvas;
        public Material outLight;

        void Awake()
        {
            gridCanvas = GetComponentInChildren<Canvas>();
            minionMesh = GetComponentInChildren<MinionMesh>();

        }

        //初始化
        public void AchieveMinionGrid(MinionGridItem minionGridItem, HexGridItem hexGridItem) {
            minionCellViews = new MinionCellView[minionGridItem.minionCells.Count()];
            Dictionary<int, MinionCellItem>.KeyCollection keyCol = minionGridItem.minionCells.Keys;
            int i = 0;
            foreach (int key in keyCol)
            {
                MinionCellItem minionCellItem = minionGridItem.minionCells[key];
                Vector3 position = new Vector3();
                position = MinionMetrics.erectPosition(position, hexGridItem.cells[key].X, hexGridItem.cells[key].Z, hexGridItem.modelInfo.arrayMode);
                //创建一个生物实例
                MinionCellView cell = minionCellViews[i] = Instantiate<MinionCellView>(cellPrefab);
                cell.transform.SetParent(transform, false);
                cell.transform.localPosition = position;
                cell.minionCellItem = minionCellItem;
                Text label = Instantiate<Text>(cellLabelPrefab);
                label.rectTransform.SetParent(cell.transform, false);
                label.rectTransform.anchoredPosition =
                    new Vector2(position.x, position.z);
                label.text = minionCellItem.cardEntry.atk.ToString() + "-" + minionCellItem.cardEntry.def.ToString();
                i++;
            }
            //渲染需要放在格子生成完毕后
            //minionMesh.Triangulate(minionCellViews, hexGridItem.modelInfo.arrayMode);
        }
        //重新渲染部分生物
        public void RenderSomeMinionByMinionCellItem(List<MinionCellItem> mList) {
            foreach (MinionCellView minCellView in minionCellViews) {
                foreach (MinionCellItem minCellItem in mList) {
                    if (minCellView.minionCellItem.uuid == minCellItem.uuid) {
                        RenderOneMinionCellByMinionCellItem(minCellView, minCellItem);
                    }
                }
            }
        }
        public void RenderOneMinionCellByMinionCellItem(MinionCellView minionCellView, MinionCellItem minionCellItem) {
            minionCellView.minionCellItem = minionCellItem;
            Image imageComponent = minionCellView.GetComponent<Image>();
            if (minionCellItem.IsHighLight == false)
            {
                imageComponent.material = null;
            }
            else {
                imageComponent.material = outLight;
            }
        }

    }
}
