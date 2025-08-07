using UnityEngine;
using UnityEngine.Rendering.Universal;

public class GlobalLightControls : MonoBehaviour
{

    [SerializeField] Light2D regularGlobalLight;
    [SerializeField] Light2D silhouetteGlobalLight;

    public void SilhouettePlayerMode()
    {
       regularGlobalLight.gameObject.SetActive(false);
       silhouetteGlobalLight.gameObject.SetActive(true);
    }

    public void RegularLightMode()
    {
        regularGlobalLight.gameObject.SetActive(true);
        silhouetteGlobalLight.gameObject.SetActive(false);
    }
}
