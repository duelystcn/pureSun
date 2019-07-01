

using Assets.Scripts.OrderSystem.Event;
using Assets.Scripts.OrderSystem.Model.OperateSystem;
using Assets.Scripts.OrderSystem.Util;
using Assets.Scripts.OrderSystem.View.HexView;
using PureMVC.Interfaces;
using PureMVC.Patterns.Mediator;
using UnityEngine;

namespace Assets.Scripts.OrderSystem.View.OperateSystem
{
    public class OperateSystemMediator : Mediator
    {
        public new const string NAME = "OperateSystemMediator";
        private HexGridMediator hexGridMediator = null;
        public OperateSystemView operateSystemView
        {
            get { return (OperateSystemView)base.ViewComponent; }
        }
        public OperateSystemMediator(OperateSystemView operateSystemView) : base(NAME, operateSystemView)
        {

        }
        public override void OnRegister()
        {
            base.OnRegister();

        }
        //监听列表
        public override string[] ListNotificationInterests()
        {
            string[] notifications = new string[1];
            notifications[0] = OperateSystemEvent.OPERATE_TRAIL_DRAW;
            return notifications;
        }
        //监听
        public override void HandleNotification(INotification notification)
        {
            switch (notification.Name)
            {
                //划线相关
                case OperateSystemEvent.OPERATE_TRAIL_DRAW:
                    switch (notification.Type)
                    {
                        //划线开始
                        case OperateSystemEvent.OPERATE_TRAIL_DRAW_START:
                            operateSystemView.trailDrawLine.TrailDrawStart();
                            break;
                        //添加划线组件
                        case OperateSystemEvent.OPERATE_TRAIL_DRAW_CREATE:
                            operateSystemView.AchieveOperateSystemView();
                            hexGridMediator = Facade.RetrieveMediator(HexGridMediator.NAME) as HexGridMediator;
                            operateSystemView.trailDrawLine.OnMouseButtonUp += () =>
                            {
                                //判断选择了什么，放在视图层做
                                HexCellView hexCellView = hexGridMediator.GetHexCellViewByPosition(operateSystemView.trailDrawLine.last);
                                if (hexCellView != null)
                                {
                                    //战场上哪一个格子
                                    SendNotification(OperateSystemEvent.OPERATE_SYS, hexCellView.hexCellItem, OperateSystemEvent.OPERATE_SYS_DRAW_END_HEX);
                                    return;
                                }
                                Vector3 startPosition = new Vector3();
                                startPosition.x = 100;
                                startPosition.z = -15;
                                //选择了献祭区域
                                bool isCircuit = GeometricUtil.CheckPointInSomeRectangle(10, 10, startPosition, operateSystemView.trailDrawLine.last);
                                if (isCircuit)
                                {
                                    SendNotification(OperateSystemEvent.OPERATE_SYS, null, OperateSystemEvent.OPERATE_SYS_DRAW_END_CIRCUIT);
                                    return;
                                }
                                //还可能选择了其他目标
                                //如果什么都没选
                                SendNotification(OperateSystemEvent.OPERATE_SYS, null, OperateSystemEvent.OPERATE_SYS_DRAW_END_NULL);
                            };
                            break;
                    }
                    break;
            }


        }
    }
}
