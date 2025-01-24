using Unity.Cinemachine;
using UnityEngine;


[RequireComponent(typeof(CinemachinePositionComposer))]
public class CinemachineCameraManager : MonoBehaviour
{
    CinemachinePositionComposer cinemachinePositionComposer;

    public float yOffset = 0.2f;
    public float changeOffsetDuration = 2f;

    PlayerMovement player;


    private void Start()
    {
        cinemachinePositionComposer = GetComponent<CinemachinePositionComposer>();
        player = FindAnyObjectByType<PlayerMovement>();
    }

    private void Update()
    {
        float y = cinemachinePositionComposer.Composition.ScreenPosition.y;
        if (player.bubbleReady)
        {
            y = Mathf.Lerp(y, yOffset, changeOffsetDuration * Time.deltaTime);
        }
        else
        {
            y = Mathf.Lerp(y, -yOffset, changeOffsetDuration * Time.deltaTime);
        }

        cinemachinePositionComposer.Composition.ScreenPosition.y = y;

    }
}
