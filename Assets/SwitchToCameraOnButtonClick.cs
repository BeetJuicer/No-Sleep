using Unity.Cinemachine;
using UnityEngine;

public class SwitchToCameraOnButtonClick : MonoBehaviour
{
    [SerializeField] KeyCode key;
    [SerializeField] int priorityValueToAdd;
    private CinemachineCamera originalCam;
    private CinemachineCamera thisCam;
    bool isPrioritized = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        thisCam = GetComponent<CinemachineCamera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(key))
        {
            SwitchCamera();
        }
    }

    public void SwitchCamera()
    {
        print("swtiching cams");
        if (!isPrioritized)
        {
            //originalCam = CinemachineBrain.GetActiveBrain(0).ActiveVirtualCamera as CinemachineCamera;
            thisCam.Priority += priorityValueToAdd;
            isPrioritized = true;
        }
        else
        {
            thisCam.Priority -= priorityValueToAdd;
            isPrioritized = false;
        }
    }
}
