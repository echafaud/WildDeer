using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class SettingsMenu : MonoBehaviour
{
    public bool isOpened = false; //Открыто ли меню
    public Slider volume; //Громкость
    public Dropdown quality; //Качество
    public Toggle isFullscreen; //Полноэкранный режим
    public AudioMixer audioMixer; //Регулятор громкости
    public Dropdown resolutionDropdown; //Список с разрешениями для игры
    private Resolution[] resolutions; //Список доступных разрешений
    private int currResolutionIndex = 0; //Текущее разрешение
    public Button saveButton;
    // Start is called before the first frame update
    void Start()
    {
        volume.value = DeerUnity.VolumeRatio;
        resolutionDropdown.ClearOptions(); //Удаление старых пунктов
        resolutions = Screen.resolutions; //Получение доступных разрешений
        List<string> options = new List<string>(); //Создание списка со строковыми значениями

        for (int i = 0; i < resolutions.Length; i++) //Поочерёдная работа с каждым разрешением
        {
            string option = resolutions[i].width + " x " + resolutions[i].height; //Создание строки для списка
            options.Add(option); //Добавление строки в список

            if (resolutions[i].Equals(Screen.currentResolution)) //Если текущее разрешение равно проверяемому
            {
                currResolutionIndex = i; //То получается его индекс
            }
        }

        resolutionDropdown.AddOptions(options); //Добавление элементов в выпадающий список
        resolutionDropdown.value = currResolutionIndex; //Выделение пункта с текущим разрешением
        resolutionDropdown.RefreshShownValue(); //Обновление отображаемого значения
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
