using UnityEngine;
using System.Linq;

public static class MyExtensions
{
    public static Matrix4x4 multiply(this Matrix4x4 mat, float scalar)
    {
        Matrix4x4 result = mat;
        foreach (int x in Enumerable.Range(0,4))
        {
            foreach (int y in Enumerable.Range(0, 4))
            {
                result[x, y] = mat[x, y] * scalar;
            }
        }
        return result;
    }
}
