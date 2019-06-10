using Assets.Scripts.OrderSystem.Model.Hand;
using OrderSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.OrderSystem.View.HandView
{
    public class HandGridView : MonoBehaviour
    {
        public List<HandCellView> handCellViews;
        //对象池
        public ObjectPool<HandCellView> handCellPool;
        public HandCellView cellPrefab;
        HandMesh handMesh;
       
        void Awake()
        {
            handMesh = GetComponentInChildren<HandMesh>();
            //Resources
            GameObject prefab = Resources.Load<GameObject>("Prefabs/HandCell/HandCell");
            handCellPool = new ObjectPool<HandCellView>(prefab, "handCellPool");
            handCellViews = new List<HandCellView>();
        }

        public void AchieveHandGrid(HandGridItem handGridItem) {
            //清除
            foreach (HandCellView handCellView in handCellViews)
            {
                handCellPool.Push(handCellView);
            }
            //重新
            for (int i = 0; i < handGridItem.handCells.Count(); i++)
            {
                handGridItem.handCells[i].X = i;
                HandCellItem handCellItem = handGridItem.handCells[i];
                Vector3 position = new Vector3();
                position = HandMetrics.erectPosition(position, handCellItem.X);
                //创建一个格子实例
                //       HandCellView cell = Instantiate<HandCellView>(cellPrefab);
                HandCellView cell = handCellPool.Pop();
                //设置图片
                cell.GetComponent<HandCellInstance>().SetImage();
                cell.transform.SetParent(transform, false);
                cell.transform.localPosition = position;
                cell.handCellItem = handCellItem;
                handCellViews.Add(cell);
            }
            //渲染需要放在格子生成完毕后
            //不再渲染，改为用贴图
            //handMesh.Triangulate(handCellViews);
        }
        //移除一张
        public void RemoveOneHandCellViewByHandCellItem(HandCellItem handCellItem) {
            //清除
            foreach (HandCellView handCellView in handCellViews)
            {
                if (handCellView.handCellItem.uuid == handCellItem.uuid) {
                    handCellPool.Push(handCellView);
                } 
            }
        }
    }
}
