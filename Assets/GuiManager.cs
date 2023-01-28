using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GuiManager : MonoBehaviour
{
    public Slider Slider;
    public Image Knob, FillArea, Line;
    private string CurrentButtonId = "";
    private Dictionary<string, float> IdToSliderValue = new();
    private Coroutine FadeSliderCoroutine = null;

    void Start()
    {
        Slider.gameObject.SetActive(false);
    }

    public void ShowSlider(string buttonId)
    {
        if (CurrentButtonId.Equals(buttonId))
        {
            CurrentButtonId = "";
            StopFadeCoroutine();
            FadeSliderCoroutine = StartCoroutine(FadeSlider(true));
        }
        else
        {
            CurrentButtonId = buttonId;
            var success = IdToSliderValue.TryGetValue(CurrentButtonId, out var sliderValue);

            if (success)
            {
                Slider.value = sliderValue;
            }
            else
            {
                Slider.value = 0.5f;
            }

            Debug.Log($"Variable '{CurrentButtonId}' has been loaded with value of '{Slider.value}'.");

            if (!Slider.isActiveAndEnabled)
            {
                StopFadeCoroutine();
                FadeSliderCoroutine = StartCoroutine(FadeSlider(false, true));
            }
        }
    }

    public void StopFadeCoroutine()
    {
        if (FadeSliderCoroutine != null)
        {
            StopCoroutine(FadeSliderCoroutine);
        }
    }

    public void SetVariableValue()
    {
        if (!string.IsNullOrEmpty(CurrentButtonId))
        {
            IdToSliderValue[CurrentButtonId] = Slider.value;
            Debug.Log($"Variable '{CurrentButtonId}' has been changed with value of '{Slider.value}'.");
        }
    }

    public IEnumerator WaitToFade()
    {
        yield return new WaitForSeconds(5);

        CurrentButtonId = "";

        FadeSliderCoroutine = StartCoroutine(FadeSlider(true));
    }

    public IEnumerator FadeSlider(bool fadeAway, bool waitToFadeOut = false)
    {
        // fade from opaque to transparent
        if (fadeAway)
        {
            // loop over 1 second backwards
            for (float i = 1; i >= 0; i -= Time.deltaTime)
            {
                // set color with i as alpha
                Knob.color = new Color(1, 1, 1, i);
                FillArea.color = new Color(1, 1, 1, i);
                Line.color = new Color(1, 1, 1, i);
                yield return null;
            }

            Slider.gameObject.SetActive(false);
        }
        // fade from transparent to opaque
        else
        {
            Slider.gameObject.SetActive(true);

            // loop over 1 second
            for (float i = 0; i <= 1; i += Time.deltaTime)
            {
                // set color with i as alpha
                Knob.color = new Color(1, 1, 1, i);
                FillArea.color = new Color(1, 1, 1, i);
                Line.color = new Color(1, 1, 1, i);
                yield return null;
            }
        }

        if (waitToFadeOut)
        {
            FadeSliderCoroutine = StartCoroutine(WaitToFade());
        }
    }
}
