using System.Collections;
using UnityEngine;

public static class Lerp
{
    public static IEnumerator MoveObjectFromPointAToPointBOverTime(Transform Object, Vector3 StartPos, Vector3 EndPos, float duration)
    {
        float elapsedTime = 0.0f;

        while (elapsedTime < duration)
        {
            Object.position = Vector3.Lerp(StartPos, EndPos, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        Object.position = EndPos;
    }
}
