

using Assets.Scripts.OrderSystem.View.CircuitView.QuestStageCircuit;
using Assets.Scripts.OrderSystem.View.HandView;
using Assets.Scripts.OrderSystem.View.HexView;
using Assets.Scripts.OrderSystem.View.MinionView;
using Assets.Scripts.OrderSystem.View.OperateSystem;
using Assets.Scripts.OrderSystem.View.SpecialOperateView.ChooseView;
using Assets.Scripts.OrderSystem.View.UIView;
using OrderSystem;
using UnityEngine;

public class MainUI : MonoBehaviour {

    public HexGridView HexGridView = null;
    public HandControlView HandControlView = null;
    public MinionGridView minionGridView = null;
    public QuestStageCircuitButton circuitButton = null;
    public OperateSystemView operateSystemView = null;
    public ChooseGridView chooseGridView = null;
    public UIControllerListView UIControllerListView = null;


    //程序入口函数
    private void Start()
    {
        ApplicationFacade facade = new ApplicationFacade("SUN");
        facade.StartUp(this);
    }
}
