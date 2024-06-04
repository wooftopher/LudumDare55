
using UnityEngine;
using UnityEngine.UI;

public class FillHealthBar : MonoBehaviour
{
    public PlayerHealth playerHealth;
    public UnityEngine.UI.Image fillImage;
    private Slider slider;
    // Start is called before the first frame update
    void Awake()
    {
        slider = GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        float fillValue = playerHealth.CurrentHealth / playerHealth.MaxHealth;

        slider.value = fillValue;
    }
}
