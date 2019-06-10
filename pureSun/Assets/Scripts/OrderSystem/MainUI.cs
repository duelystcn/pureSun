
using Assets.Scripts.OrderSystem.View.CircuitView;
using Assets.Scripts.OrderSystem.View.HandView;
using Assets.Scripts.OrderSystem.View.HexView;
using Assets.Scripts.OrderSystem.View.MinionView;
using Assets.Scripts.OrderSystem.View.OperateSystem;
using Assets.Scripts.OrderSystem.View.SpecialOperateView.ChooseView;
using OrderSystem;
using UnityEngine;

public class MainUI : MonoBehaviour {

    public HexGridView HexGridView = null;
    public HandGridView HandGridView = null;
    public MinionGridView minionGridView = null;
    public CircuitButton circuitButton = null;
    public OperateSystemView operateSystemView = null;
    public ChooseGridView chooseGridView = null;

    //程序入口函数
    private void Start()
    {
        ApplicationFacade facade = new ApplicationFacade("SUN");
        facade.StartUp(this);
    }
}
