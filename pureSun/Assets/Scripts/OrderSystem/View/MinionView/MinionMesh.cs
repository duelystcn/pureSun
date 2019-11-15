using Assets.Scripts.OrderSystem.Metrics;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.OrderSystem.View.MinionView
{
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
    public class MinionMesh : MonoBehaviour
    {
        Mesh minionMesh;
        List<Vector3> vertices;
        List<int> triangles;
        List<Color> colors;

        MeshCollider meshCollider;

        void Awake()
        {
            GetComponent<MeshFilter>().mesh = minionMesh = new Mesh();
            meshCollider = gameObject.AddComponent<MeshCollider>();
            minionMesh.name = "Minion Mesh";
            vertices = new List<Vector3>();
            colors = new List<Color>();
            triangles = new List<int>();
        }

        public void Triangulate(MinionCellView[] cells, string arrayMode)
        {
            minionMesh.Clear();
            vertices.Clear();
            triangles.Clear();
            colors.Clear();
            for (int i = 0; i < cells.Length; i++)
            {
                Triangulate(cells[i], arrayMode);
            }
            minionMesh.vertices = vertices.ToArray();
            minionMesh.triangles = triangles.ToArray();
            minionMesh.colors = colors.ToArray();
            minionMesh.RecalculateNormals();
            meshCollider.sharedMesh = minionMesh;
        }

        //确定顶点
        void Triangulate(MinionCellView cell, string arrayMode)
        {
            Vector3 center = cell.transform.localPosition;
            Vector3[] arrayCorners = MinionMetrics.getCornersByArrayMode(arrayMode);
            for (int i = 0; i < 6; i++)
            {
                AddTriangle(
                 center,
                 center + arrayCorners[i],
                 center + arrayCorners[i + 1]
                );
                AddTriangleColor(Color.green);
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
