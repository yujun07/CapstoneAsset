using UnityEngine;
using UnityEngine.UI;

public class GaugeSlider : MonoBehaviour
{
    public Slider Gauge;
    private bl_SniperScope _sniperScope;
    private GameObject sniper;

    private void OnEnable()
    {
        if (_sniperScope == null)
        {
            sniper = GameObject.FindWithTag("Sniper");
            _sniperScope = sniper.GetComponent<bl_SniperScope>();
        }
    }

    private void Update()
    {
        if (_sniperScope != null && Gauge.value != _sniperScope.GaugeValue())
        {
            Gauge.value = _sniperScope.GaugeValue();
        }
    }
}
