using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(UnityEngine.XR.Interaction.Toolkit.Interactables.XRBaseInteractable))]
public class OnSelectStartRain : MonoBehaviour
{
    public bool stopOnDeselect = true;
    public float fadeTime = 1.0f;

    UnityEngine.XR.Interaction.Toolkit.Interactables.XRBaseInteractable interactable;

    void Awake()
    {
        interactable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRBaseInteractable>();
    }

    void OnEnable()
    {
        interactable.selectEntered.AddListener(HandleSelectEntered);
        if (stopOnDeselect) interactable.selectExited.AddListener(HandleSelectExited);
    }

    void OnDisable()
    {
        interactable.selectEntered.RemoveListener(HandleSelectEntered);
        if (stopOnDeselect) interactable.selectExited.RemoveListener(HandleSelectExited);
    }

    void HandleSelectEntered(SelectEnterEventArgs args)
    {
        if (WeatherManager.Instance != null) WeatherManager.Instance.StartRain(fadeTime);
    }

    void HandleSelectExited(SelectExitEventArgs args)
    {
        if (WeatherManager.Instance != null) WeatherManager.Instance.StopRain(fadeTime);
    }
}