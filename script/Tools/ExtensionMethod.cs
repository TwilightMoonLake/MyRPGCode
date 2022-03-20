using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExtensionMathot 
{
    private const float dotThreshold = 0.5f;
    //拓展方法
    public static bool IsFacingTarget(this Transform transform,Transform target)
    {
        var vectorToTarget = target.position - transform.position;
        vectorToTarget.Normalize();
        float dot = Vector3.Dot(transform.forward, vectorToTarget);
        Debug.Log("执行了拓展");

        return dot >= dotThreshold;
    }

}
