using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class PCG_OptimizePolygonCollider2D : MonoBehaviour
{
    bool first_pass = false;
    bool second_pass = false;

    void LateUpdate()
    {
        if (!first_pass)
        {
            first_pass = true;
            if (gameObject.GetComponent<PolygonCollider2D>() != null) { OptimizePolygonCollider2D(gameObject, gameObject.GetComponent<PolygonCollider2D>()); }
        }

        if (!second_pass)
        {
            second_pass = true;
            if (gameObject.GetComponent<PolygonCollider2D>() != null) { OptimizePolygonCollider2D(gameObject, gameObject.GetComponent<PolygonCollider2D>()); }
        }
    }

    void OptimizePolygonCollider2D(GameObject gameobject, PolygonCollider2D polygonCollider2D)
    {
        int origVertCount = 0;
        int cleanVertCount = 0;
        float angleThreshold = 90.0f;

        List<Vector2> newVerts = new List<Vector2>();

        for (int i = 0; i < polygonCollider2D.pathCount; i++)
        {
            newVerts.Clear();
            Vector2[] path = polygonCollider2D.GetPath(i);
            if (path.Length < 4) { continue; }
            origVertCount += path.Length;

            float angle1 = 0;
            float angle2 = 0;

            newVerts.Clear();
            int mx = path.Length;

            Vector2 currentDir = (path[0] - path[1]).normalized;
            angle1 = Vector2.Angle(path[0] - path[1], currentDir);

            for (int j = 0; j < mx; j++)
            {
                int sPrev = (((j - 1) % mx) + mx) % mx;
                int sNext = (((j + 1) % mx) + mx) % mx;

                angle1 = Vector2.Angle(path[sPrev] - path[j], currentDir);
                angle2 = Vector2.Angle(path[j] - path[sNext], currentDir);

                if (angle1 > angleThreshold || angle2 > angleThreshold)
                {
                    currentDir = (path[j] - path[sNext]).normalized;
                    newVerts.Add(path[j]);
                }
            }
            polygonCollider2D.SetPath(i, newVerts.ToArray());
            cleanVertCount += newVerts.Count;
        }
    }
}
