using UnityEngine;
using UnityEngine.UI;


public class StatusBar : MonoBehaviour
{
    [SerializeField] private Slider slider;
    
    public void UpdateStatusBar(float currentValue, float maxValue){
        slider.value = currentValue / maxValue;
    }

    void Update()
    {
        
    }
}
