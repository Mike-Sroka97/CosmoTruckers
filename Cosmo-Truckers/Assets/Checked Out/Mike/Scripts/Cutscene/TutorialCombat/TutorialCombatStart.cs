using UnityEngine;

/// <summary>
/// Calls the TutorialTurnOrder's start turn order
/// </summary>
public class TutorialCombatStart : MonoBehaviour
{
    private void Awake()
    {
        // Fade out the vignette
        CameraController cameraController = FindObjectOfType<CameraController>();
        StartCoroutine(cameraController.FadeVignette(true)); 

        // Start the turn order
        TutorialTurnOrder turnOrder = FindObjectOfType<TutorialTurnOrder>();
        turnOrder.StartTurnOrder(); 
    }
}
