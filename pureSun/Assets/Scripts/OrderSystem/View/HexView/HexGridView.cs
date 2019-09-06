
using Assets.Scripts.OrderSystem.Common.UnityExpand;
using Assets.Scripts.OrderSystem.Model.Hex;
using Assets.Scripts.OrderSystem.Util;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.OrderSystem.View.HexView
{
    public class HexGridView : MonoBehaviour
    {
        public HexCellView[] cellViews;

        public HexCellView cellPrefab;
        public Text cellLabelPrefab;


        HexModelInfo modelInfo;
        Canvas gridCanvas;
        HexMesh hexMesh;
        //原始边框
        public Material originalBorderColor;
        //可召唤边框
        public Material canCallBorderColor;

        void Awake()
        {
            gridCanvas = GetComponentInChildren<Canvas>();
            hexMesh = GetComponentInChildren<HexMesh>();
        }
        //实例化一个hexGrid-
        public void AchieveHexGrid(HexGridItem HexGrid)
        {
            this.modelInfo = HexGrid.modelInfo;
            cellViews = new HexCellView[HexGrid.cells.Length];
            for (int i = 0; i < HexGrid.cells.Length; i++)
            {
                HexCellItem hexCellItem = HexGrid.cells[i];
                int x = hexCellItem.X;
                int z = hexCellItem.Z;
                Vector3 position = new Vector3();
                //格子中心坐标
                position = HexMetrics.erectPosition(position, x, z, this.modelInfo.arrayMode);
                //创建一个格子实例
                HexCellView cell = cellViews[i] = Instantiate<HexCellView>(cellPrefab);
                cell.transform.SetParent(transform, false);
                cell.transform.localPosition = position;
                cell.hexCellItem = hexCellItem;
                TextMeshProUGUI HexCellLabel = UtilityHelper.FindChild<TextMeshProUGUI>(cell.transform, "HexCellLabel");
                HexCellLabel.text = hexCellItem.coordinates.ToStringOnSeparateLines();
                //Text label = Instantiate<Text>(cellLabelPrefab);
                //label.rectTransform.SetParent(gridCanvas.transform, false);
                //label.rectTransform.anchoredPosition =
                //    new Vector2(position.x, position.y);
                //label.text = hexCellItem.coordinates.ToStringOnSeparateLines();
            }
            //渲染需要放在格子生成完毕后
            hexMesh.Triangulate(cellViews, this.modelInfo.arrayMode);
        }


        //更新地图内容
        public void UpdateHexGrid(HexGridItem HexGrid)
        {
            for (int i = 0; i < HexGrid.cells.Length; i++)
            {
                cellViews[i].LoadHexCellItem(HexGrid.cells[i],this);
            }

            hexMesh.Triangulate(cellViews, this.modelInfo.arrayMode);
        }
        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                HandleInput();
            }
        }

        void HandleInput()
        {
            //需要一个tag为MainCanmera的Camera
            Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(inputRay, out hit))
            {
                TouchCell(hit.point);
            }
        }

        public void TouchCell(Vector3 position)
        {
            HexCellView cell = GetCellByPosition(position);
            if (cell != null) {
                cell.OnClick();
            }
        }
        //根据坐标返回所点击的cell
        public HexCellView GetCellByPosition(Vector3 position) {
            HexCellView cell = null;
            position = transform.InverseTransformPoint(position);
            if (HexUtil.CheckOnHexGrid(position, this.modelInfo))
            {
                HexCoordinates coordinates = HexUtil.FromPosition(position, this.modelInfo);
                int index = 0;
                if (this.modelInfo.arrayMode == HexMetrics.MODE_ERECT)
                {
                    index = coordinates.X + coordinates.Z * this.modelInfo.width + coordinates.Z / 2;
                }
                else
                {
                    //偏差值，X增长时，Z轴会向上偏移
                    int deviation = coordinates.X / 2;
                    index = coordinates.X + (coordinates.Z + deviation) * this.modelInfo.width;
                }
                if (index > -1)
                {
                    cell = cellViews[index];
                }
            }
            return cell;
        }
    }
}
