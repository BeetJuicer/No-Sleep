using FreeDraw;
using UnityEngine;

public class DrawingControls : MonoBehaviour
{
    [SerializeField] KeyCode resetKey;
    [SerializeField] Drawable drawable;
    [SerializeField] DrawingSettings settings;
    [SerializeField] int width;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        settings.SetMarkerWidth(width);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(resetKey))
        {
            drawable.ResetCanvas();
        }
    }
}
