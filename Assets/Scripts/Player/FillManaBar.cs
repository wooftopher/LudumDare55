
using UnityEngine;
using UnityEngine.UI;


public class FillManaBar : MonoBehaviour
{
    public PlayerMana playerMana;
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
        float fillValue = playerMana.CurrentMana / playerMana.MaxMana;


        slider.value = fillValue;
    }
}
