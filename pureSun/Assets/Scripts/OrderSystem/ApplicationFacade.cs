
/*=========================================
* Author: Administrator
* DateTime:2017/6/20 18:17:21
* Description:$safeprojectname$
==========================================*/

using Assets.Scripts.OrderSystem.Controller;
using Assets.Scripts.OrderSystem.Event;
using PureMVC.Patterns.Facade;

namespace OrderSystem
{
    public class ApplicationFacade : Facade
    {
        
        public ApplicationFacade(string key)
         : base(key) 
        {

        }

        /// <summary>
        /// start program
        /// </summary>
        public void StartUp(MainUI mainUI)
        {
            SendNotification(OrderSystemEvent.STARTUP, mainUI);
        }

        #region 重新核心类型的构造方法

        protected override void InitializeFacade()
        {
            base.InitializeFacade();
        }

        protected override void InitializeView()
        {
            base.InitializeView();
        }

        protected override void InitializeController()
        {
            base.InitializeController();
            RegisterCommand(OrderSystemEvent.STARTUP, () => new StartUpCommand());
            RegisterCommand(OrderSystemEvent.START_CIRCUIT, () => new StartCircuitCommand());
            RegisterCommand(OperateSystemEvent.OPERATE_SYS, () => new OperateSystemCommand());
            RegisterCommand(HexSystemEvent.HEX_VIEW_SYS, () => new HexViewCommand());
            RegisterCommand(MinionSystemEvent.MINION_SYS, () => new MinionSysCommand());
            RegisterCommand(UIViewSystemEvent.UI_CHOOSE_STAGE, () => new ChooseStageCommand());
            RegisterCommand(UIViewSystemEvent.UI_CHOOSE_MAKE_STAGE, () => new ChooseMakeStageCommand());
            RegisterCommand(UIViewSystemEvent.UI_QUEST_STAGE, () => new QuestStageCommand());
            RegisterCommand(UIViewSystemEvent.UI_QUEST_TURN_STAGE, () => new QuestStageOneTurnCommand());
            RegisterCommand(LogicalSysEvent.LOGICAL_SYS, () => new LogicalProcessorCommand());
            RegisterCommand(TimeTriggerEvent.TIME_TRIGGER_SYS, () => new TimeTriggerSysCommand());
            RegisterCommand(EffectExecutionEvent.EFFECT_EXECUTION_SYS, () => new EffectExecutionCommand());
            RegisterCommand(GameContainerEvent.GAME_CONTAINER_SYS, () => new GameContainerCommand());
        }

        protected override void InitializeModel()
        {
            base.InitializeModel();
        }

        #endregion
    }
}