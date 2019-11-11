using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.OrderSystem.View.HandView
{
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
    public class HandMesh : MonoBehaviour
    {
        Mesh handMesh;
        List<Vector3> vertices;
        List<int> triangles;
        List<Color> colors;

        MeshCollider meshCollider;

        void Awake()
        {
            GetComponent<MeshFilter>().mesh = handMesh = new Mesh();
            meshCollider = gameObject.AddComponent<MeshCollider>();
            handMesh.name = "Hand Mesh";
            vertices = new List<Vector3>();
            colors = new List<Color>();
            triangles = new List<int>();
        }

        public void Triangulate(List<HandCellView> cells)
        {
            handMesh.Clear();
            vertices.Clear();
            triangles.Clear();
            colors.Clear();
            for (int i = 0; i < cells.Count(); i++)
            {
                Triangulate(cells[i]);
            }
            handMesh.vertices = vertices.ToArray();
            handMesh.triangles = triangles.ToArray();
            handMesh.colors = colors.ToArray();
            handMesh.RecalculateNormals();
            meshCollider.sharedMesh = handMesh;
        }

        //确定顶点
        void Triangulate(HandCellView cell)
        {
            Vector3 center = cell.transform.localPosition;
            Vector3[] arrayCorners = HandMetrics.getCorners();
            for (int i = 0; i < 4; i++)
            {
                AddTriangle(
                    center,
                    center + arrayCorners[i],
                    center + arrayCorners[i + 1]
                );
                AddTriangleColor(Color.white);
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
