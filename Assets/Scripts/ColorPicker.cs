using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorPicker : MonoBehaviour
{
    public Material carColor;
    public Slider redSlider;
    public Slider greenSlider;
    public Slider blueSlider;
    public Slider metallicSlider;
    public Slider smoothnessSlider;

    private float red;
    private float green;
    private float blue;
    private float metallic;
    private float smoothness;

    private Color selectedColor;

    private void Awake() {
        if(PlayerPrefs.GetInt("user_id") != null)
        {
            Color userColor;
            if (ColorUtility.TryParseHtmlString(PlayerPrefs.GetString("CarColor"), out userColor))
            {
                carColor.color = userColor;
                Debug.Log(userColor);
            }

            redSlider.value = userColor.r;
            greenSlider.value = userColor.g;
            blueSlider.value = userColor.b;

            Debug.Log(PlayerPrefs.GetString("Metallic"));

            metallicSlider.value = float.Parse(PlayerPrefs.GetString("Metallic"));
            smoothnessSlider.value = float.Parse(PlayerPrefs.GetString("Smoothness"));

            carColor.SetFloat("_Metallic", metallicSlider.value);
            carColor.SetFloat("_Glossiness", smoothnessSlider.value);
        }
        else 
        {
            redSlider.value = 0.0f;
            greenSlider.value = 0.0f;
            blueSlider.value = 0.0f;
            carColor.SetFloat("_Metallic", 0.0f);
            carColor.SetFloat("_Glossiness", 0.0f);
            carColor.color = new Color(0.0f, 0.0f, 0.0f);
        }
    }

    private void Start()
    {
        redSlider.onValueChanged.AddListener(OnSliderValueChanged);
        greenSlider.onValueChanged.AddListener(OnSliderValueChanged);
        blueSlider.onValueChanged.AddListener(OnSliderValueChanged);
        metallicSlider.onValueChanged.AddListener(OnSliderValueChanged);
        smoothnessSlider.onValueChanged.AddListener(OnSliderValueChanged);
    }

    private void OnSliderValueChanged(float value)
    {
        float red = redSlider.value;
        float green = greenSlider.value;
        float blue = blueSlider.value;
        float metallic = metallicSlider.value;
        float smoothness = smoothnessSlider.value;

        PlayerPrefs.SetFloat("red", red);
        PlayerPrefs.SetFloat("green", green);
        PlayerPrefs.SetFloat("blue", blue);
        PlayerPrefs.SetString("metallic", metallic.ToString());
        PlayerPrefs.SetString("smoothness", smoothness.ToString());

        selectedColor = new Color(red, green, blue);
        carColor.SetFloat("_Metallic", metallic);
        carColor.SetFloat("_Glossiness", smoothness);
        carColor.color = selectedColor;

        Debug.Log("#"+ColorUtility.ToHtmlStringRGB(carColor.color));
        Debug.Log(PlayerPrefs.GetString("Metallic"));
    }

    public void UpdateColor()
    {
        WWWForm regForm = new WWWForm();
        regForm.AddField("user_id", PlayerPrefs.GetInt("user_id"));
		regForm.AddField("car_color", "#"+ColorUtility.ToHtmlStringRGB(carColor.color));
        regForm.AddField("metallic_value", PlayerPrefs.GetString("metallic"));
        regForm.AddField("smoothness_value", PlayerPrefs.GetString("smoothness"));

		WWW www = new WWW("http://localhost/SushiDriver/php/updateCarColor.php", regForm);
		StartCoroutine(UpdateFunc(www));
    }

    private IEnumerator UpdateFunc(WWW www)
	{
		yield return www;
        Debug.Log(www.text);
		if(www.error != null)
		{
			Debug.Log("Ошибка: " + www.error);
			yield break;
		}
	}
}
