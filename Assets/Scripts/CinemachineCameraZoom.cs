using Unity.Cinemachine;
using UnityEngine;

public class CinemachineCameraZoom : MonoBehaviour
{
    private const float NORMAL_ORTHOGRAPHIC_SIZE = 10f;
    public static CinemachineCameraZoom Instance { get; private set; }

    [SerializeField] private CinemachineCamera cinemachineCamera;
    private float targetOrthographicSize = 10f;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        cinemachineCamera.Lens.OrthographicSize = targetOrthographicSize;
    }
    public void SetTargetOrthographicSize(float targetOrthographicSize)
    {
        this.targetOrthographicSize = targetOrthographicSize;
    }


    public void SetNormalOrthographicSize()
    {
        SetTargetOrthographicSize(NORMAL_ORTHOGRAPHIC_SIZE);
    }
}
