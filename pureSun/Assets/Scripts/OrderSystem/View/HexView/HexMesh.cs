using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.OrderSystem.View.HexView
{
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
    public class HexMesh : MonoBehaviour
    {

        Mesh hexMesh;
        List<Vector3> vertices;
        List<int> triangles;
        List<Color> colors;


        MeshCollider meshCollider;

        void Awake()
        {
            GetComponent<MeshFilter>().mesh = hexMesh = new Mesh();
            meshCollider = gameObject.AddComponent<MeshCollider>();
            hexMesh.name = "Hex Mesh";
            vertices = new List<Vector3>();
            colors = new List<Color>();
            triangles = new List<int>();
        }
        public void Triangulate(Dictionary<HexCoordinates, HexCellView> cellViewMap, string arrayMode)
        {
            hexMesh.Clear();
            vertices.Clear();
            triangles.Clear();
            colors.Clear();
            foreach (KeyValuePair<HexCoordinates, HexCellView> keyValuePair in cellViewMap) {
                Triangulate(keyValuePair.Value, arrayMode);
            }
            hexMesh.vertices = vertices.ToArray();
            hexMesh.triangles = triangles.ToArray();
            hexMesh.colors = colors.ToArray();
            hexMesh.RecalculateNormals();
            meshCollider.sharedMesh = hexMesh;
        }
        //确定顶点
        void Triangulate(HexCellView cell, string arrayMode)
        {
            Vector3 center = cell.transform.localPosition;
            Vector3[] arrayCorners = HexMetrics.getCornersByArrayMode(arrayMode);
            for (int i = 0; i < 6; i++)
            {
                AddTriangle(
                    center,
                    center + arrayCorners[i],
                    center + arrayCorners[i + 1]
                );
                AddTriangleColor(Color.grey);
            }
        }
        //渲染颜色
        void AddTriangleColor(Color color)
        {
            colors.Add(color);
            colors.Add(color);
            colors.Add(color);
        }

        void AddTriangle(Vector3 v1, Vector3 v2, Vector3 v3)
        {
            int vertexIndex = vertices.Count;
            vertices.Add(v1);
            vertices.Add(v2);
            vertices.Add(v3);
            triangles.Add(vertexIndex);
            triangles.Add(vertexIndex + 1);
            triangles.Add(vertexIndex + 2);
        }

    }
}
    
