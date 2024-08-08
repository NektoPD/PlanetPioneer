using System.Collections;
using UnityEngine;

public static class TargetPositionLerper
{
    public static IEnumerator LerpToTargetPosition(Transform bodyToMove, Vector3 startPosition, Vector3 targetPosition, float lerpDuration)
    {
        float timeElapsed = 0f;

        while (timeElapsed < lerpDuration)
        {
            float lerpFactor = timeElapsed / lerpDuration;

            bodyToMove.position = Vector3.Lerp(startPosition, targetPosition, lerpFactor);

            timeElapsed += Time.deltaTime;
            yield return null;
        }

        bodyToMove.position = targetPosition;
    }

    public static IEnumerator LerpToTargetPosition(Transform bodyToMove, Vector3 startPosition, Vector3 targetPosition, Vector3 startScale, Vector3 targetScale, float lerpDuration)
    {
        float timeElapsed = 0f;

        while (timeElapsed < lerpDuration)
        {
            float lerpFactor = timeElapsed / lerpDuration;

            bodyToMove.position = Vector3.Lerp(startPosition, targetPosition, lerpFactor);
            bodyToMove.localScale = Vector3.Lerp(startScale, targetScale, lerpFactor);

            timeElapsed += Time.deltaTime;
            yield return null;
        }

        bodyToMove.position = targetPosition;
        bodyToMove.localScale = targetScale;
    }
}
