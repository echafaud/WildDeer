using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class SettingsMenu : MonoBehaviour
{
    public bool isOpened = false; //������� �� ����
    public Slider volume; //���������
    public Dropdown quality; //��������
    public Toggle isFullscreen; //������������� �����
    public AudioMixer audioMixer; //��������� ���������
    public Dropdown resolutionDropdown; //������ � ������������ ��� ����
    private Resolution[] resolutions; //������ ��������� ����������
    private int currResolutionIndex = 0; //������� ����������
    public Button saveButton;
    // Start is called before the first frame update
    void Start()
    {
        volume.value = DeerUnity.VolumeRatio;
        resolutionDropdown.ClearOptions(); //�������� ������ �������
        resolutions = Screen.resolutions; //��������� ��������� ����������
        List<string> options = new List<string>(); //�������� ������ �� ���������� ����������

        for (int i = 0; i < resolutions.Length; i++) //���������� ������ � ������ �����������
        {
            string option = resolutions[i].width + " x " + resolutions[i].height; //�������� ������ ��� ������
            options.Add(option); //���������� ������ � ������

            if (resolutions[i].Equals(Screen.currentResolution)) //���� ������� ���������� ����� ������������
            {
                currResolutionIndex = i; //�� ���������� ��� ������
            }
        }

        resolutionDropdown.AddOptions(options); //���������� ��������� � ���������� ������
        resolutionDropdown.value = currResolutionIndex; //��������� ������ � ������� �����������
        resolutionDropdown.RefreshShownValue(); //���������� ������������� ��������
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void SaveSettings()
    {
        QualitySettings.SetQualityLevel(quality.value);
        DeerUnity.VolumeRatio = volume.value;
        audioMixer.SetFloat("MasterVolume", volume.value * 20 - 10);
        Screen.fullScreen = isFullscreen.isOn;
        Screen.SetResolution(Screen.resolutions[resolutionDropdown.value].width, Screen.resolutions[resolutionDropdown.value].height, isFullscreen.isOn);
        saveButton.interactable = false;
        saveButton.interactable = true;
    }
}
