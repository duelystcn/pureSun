

using Assets.Scripts.OrderSystem.Model.Hex;
using UnityEngine;

namespace Assets.Scripts.OrderSystem.Util
{
    class HexUtil
    {
        //校验点击点是否在地图里
        public static bool CheckOnHexGrid(Vector3 position,HexModelInfo hexModelInfo) {
            if (hexModelInfo.arrayMode == HexMetrics.MODE_ERECT)
            {
                float x = position.x / (HexMetrics.innerRadius * 2f);
                float y = -x;
                float offset = position.z / (HexMetrics.outerRadius * 3f);
                x -= offset;
                y -= offset;
                int iX = Mathf.RoundToInt(x);
                int iY = Mathf.RoundToInt(y);
                int iZ = Mathf.RoundToInt(-x - y);
                if (iX + Mathf.RoundToInt(iZ/2) < 0 || iX + Mathf.RoundToInt(iZ / 2) >= hexModelInfo.width || iZ < 0 || iZ >= hexModelInfo.height)
                {
                    return false;
                }
                else {
                    return true;
                }
            }
            else
            {
                float y = -position.z / (HexMetrics.innerRadius * 2f);
                float z = -y;
                float offset = position.x / (HexMetrics.outerRadius * 3f);
                z -= offset;
                y -= offset;
                int iZ = Mathf.RoundToInt(z);
                int iY = Mathf.RoundToInt(y);
                int iX = Mathf.RoundToInt(-z - y);

                if (iX < 0 || iX >= hexModelInfo.width || iZ + Mathf.RoundToInt(iX / 2) < 0 || iZ + Mathf.RoundToInt(iX / 2) >= hexModelInfo.height)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        //根据世界坐标转换为六边形坐标
        public static HexCoordinates FromPosition(Vector3 position, HexModelInfo hexModelInfo)
        {
            if (hexModelInfo.arrayMode == HexMetrics.MODE_ERECT)
            {
                float x = position.x / (HexMetrics.innerRadius * 2f); float y = -x;
                float offset = position.z / (HexMetrics.outerRadius * 3f);
                x -= offset;
                y -= offset;
                int iX = Mathf.RoundToInt(x);
                int iY = Mathf.RoundToInt(y); int iZ = Mathf.RoundToInt(-x - y);
                if (iX + iY + iZ != 0)
                {
                    float dX = Mathf.Abs(x - iX);
                    float dY = Mathf.Abs(y - iY);
                    float dZ = Mathf.Abs(-x - y - iZ);
                    if (dX > dY && dX > dZ)
                    {
                        iX = -iY - iZ;
                    }
                    else if (dZ > dY)
                    {
                        iZ = -iX - iY;
                    }
                }
                return new HexCoordinates(iX, iZ);
            }
            else {
                float y = - position.z / (HexMetrics.innerRadius * 2f);
                float z = -y;
                float offset = position.x / (HexMetrics.outerRadius * 3f);
                z -= offset;
                y -= offset;
                int iZ = Mathf.RoundToInt(z);
                int iY = Mathf.RoundToInt(y);
                int iX = Mathf.RoundToInt(-z - y);
                if (iX + iY + iZ != 0)
                {
                    float dZ = Mathf.Abs(z - iZ);
                    float dY = Mathf.Abs(y - iY);
                    float dX = Mathf.Abs(-z - y - iX);
                    if (dZ > dY && dZ > dX)
                    {
                        iZ = -iY - iX;
                    }
                    else if (dX > dY)
                    {
                        iX = -iZ - iY;
                    }
                }
                return new HexCoordinates(iX, iZ);
            }
            
        }

        //根据当前单元格，和向量单元格，计算出目标单元格
        public static HexCoordinates GetTargetHexCoordinatesByStartPointAndVector(HexCoordinates startPointHexCoordinates, HexCoordinates vectorHexCoordinates) {
            HexCoordinates targetHexCoordinates = new HexCoordinates(startPointHexCoordinates.X + vectorHexCoordinates.X, startPointHexCoordinates.Z + vectorHexCoordinates.Z);
            return targetHexCoordinates;
        }
       
        
       
    }
}
