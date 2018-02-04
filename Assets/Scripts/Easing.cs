using UnityEngine;

public static class Easing
{ 
    public static float easeInOutSine(float start, float end, float value)
    {
        end -= start;
        return -end * 0.5f * (Mathf.Cos(Mathf.PI * value) - 1) + start;
    }

    public static float easeOutQuad(float start, float end, float value)
    {
        end -= start;
        return -end * value * (value - 2) + start;
    }

    public static float easeOutCubic(float start, float end, float value)
    {
        value--;
        end -= start;
        return end * (value * value * value + 1) + start;
    }

    public static float easeOutElastic(float value)
    {
        float p = 0.3f;
        float s = p * 0.25f;

        return Mathf.Pow(2, -10 * value) * Mathf.Sin((value - s) * (2 * Mathf.PI) / p) + 1;
    }

    public static float easeOutExpo(float value, float totalTime)
    {
        return -Mathf.Pow(2, -10 * value / totalTime) + 1;
    }
}
