using UnityEngine;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(Light2D))]
public class LightAnimation : MonoBehaviour
{
    [NaughtyAttributes.MinMaxSlider(0f, 2f)]
    [SerializeField] private Vector2 outerRadiusRange;
    Light2D light2d;

    private float animationTime;
    [SerializeField] private float animationSpeed = 0f;

    private void Start()
    {
        light2d = GetComponent<Light2D>();
    }

    void Update()
    {
        AnimateOuterRadius();
    }

    private void AnimateOuterRadius()
    {
        animationTime += Time.deltaTime * animationSpeed;
        float radius = Mathf.Lerp(outerRadiusRange.x, outerRadiusRange.y, (Mathf.Sin(animationTime) + 1f) / 2f);
        light2d.pointLightOuterRadius = radius;
    }
}
