using System.Collections;
using UnityEngine;

public static class TargetPositionLerper
{
    public static IEnumerator LerpToTargetPosition(Transform target, Vector3 startPosition, Vector3 targetPosition, float lerpDuration)
    {
        float timeElapsed = 0f;

        while (timeElapsed < lerpDuration)
        {
            float lerpFactor = timeElapsed / lerpDuration;

            target.position = Vector3.Lerp(startPosition, targetPosition, lerpFactor);

            timeElapsed += Time.deltaTime;
            yield return null;
        }

        target.position = targetPosition;
    }

    public static IEnumerator LerpToTargetPosition(Transform target, Vector3 startPosition, Vector3 targetPosition, Vector3 startScale, Vector3 targetScale, float lerpDuration)
    {
        float timeElapsed = 0f;

        while (timeElapsed < lerpDuration)
        {
            float lerpFactor = timeElapsed / lerpDuration;

            target.position = Vector3.Lerp(startPosition, targetPosition, lerpFactor);
            target.localScale = Vector3.Lerp(startScale, targetScale, lerpFactor);

            timeElapsed += Time.deltaTime;
            yield return null;
        }

        target.position = targetPosition;
        target.localScale = targetScale;
    }
}
