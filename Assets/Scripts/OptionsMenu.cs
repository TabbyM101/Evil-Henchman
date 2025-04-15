using UnityEngine;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{

    [SerializeField] private Slider volumeSlider;

    [SerializeField] private Slider cameraZoomSlider;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        volumeSlider.value = AudioManager.Current.audioSource.volume;
        cameraZoomSlider.value = CameraUtils.Current.CamZoomSpeed;

        //volumeSlider.onValueChanged.AddListener();
    }

    public void UpdateVolumeSlider() {
        AudioManager.Current.audioSource.volume = volumeSlider.value;
    }

    public void UpdateCameraSlider() {
        CameraUtils.Current.CamZoomSpeed = cameraZoomSlider.value;
    }
}
