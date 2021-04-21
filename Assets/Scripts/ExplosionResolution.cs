using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ExplosionResolution : MonoBehaviour
{
    public Slider sl;
    public TextMeshProUGUI qualitySet;

    private void Start()
    {
        sl.minValue = 2;
        sl.maxValue = 64;
        sl.wholeNumbers = true;
        sl.value = PlayerPrefs.GetFloat("quality", 16);
    }
    public void UpdateQualityOnChange(float value)
    {
        PlayerPrefs.SetFloat("quality", value);
        qualitySet.text = value.ToString();
    }
}
