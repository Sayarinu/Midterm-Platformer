using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Volume : MonoBehaviour
{
    [SerializeField]
    private Slider slider;
    // Start is called before the first frame update
    private void Start() {
        slider.value = AudioListener.volume;
    }
    public void ChangeVolume()
    {
        AudioListener.volume = slider.value;
    }
}
