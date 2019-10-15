using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class BindSliderValueToText : MonoBehaviour
{
    [SerializeField]
    private Slider slider;

    private Text text;
    
    public void Start()
    {
        if (slider == null)
        {
            Debug.LogWarning("No slider to get value from.");
        }

        text = GetComponent<Text>();
        
        UpdateText(slider.value);

        slider.onValueChanged.AddListener(UpdateText);
    }

    private void UpdateText(float value)
    {
        text.text = value.ToString(CultureInfo.InvariantCulture);
    }
}
