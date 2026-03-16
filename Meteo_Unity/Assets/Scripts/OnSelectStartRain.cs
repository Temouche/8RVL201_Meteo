using UnityEngine;

[RequireComponent(typeof(Collider))]
public class CityOrb : MonoBehaviour
{
    public WeatherType targetWeather = WeatherType.Rain;
    public bool toggleMode = true; // si true -> ToggleWeather(targetWeather), sinon SetWeather

    // Méthode publique exposée dans l'Inspector pour être appelée par l'event Activated
    public void OnActivated()
    {
        if (WeatherManager.Instance == null)
        {
            Debug.LogWarning("[CityOrb] WeatherManager missing.");
            return;
        }

        if (toggleMode)
        {
            WeatherManager.Instance.ToggleWeather(targetWeather);
            Debug.Log($"[CityOrb] Toggle request: {targetWeather}");
        }
        else
        {
            WeatherManager.Instance.SetWeather(targetWeather);
            Debug.Log($"[CityOrb] Set request: {targetWeather}");
        }
    }

    // Option: méthode non-paramétrée pour l'Inspector Events
    public void OnActivated_SetRain() => OnActivated();
}