using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class Settings : MonoBehaviour
{
    public TMP_Dropdown resolutionDropdown;
    public TMP_Dropdown refreshRateDropdown;

    private Resolution[] resolutions;
    private List<int> availableRefreshRates = new List<int>();

    void Start()
    {
        resolutionDropdown.ClearOptions();
        List<string> options = new List<string>();
        resolutions = Screen.resolutions;
        int currentResolutionIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + "x" + resolutions[i].height;
            if (!options.Contains(option))
            {
                options.Add(option);
            }

            if (resolutions[i].width == Screen.currentResolution.width
                && resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = options.Count - 1;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.RefreshShownValue();

        LoadSettings(currentResolutionIndex);

        UpdateRefreshRateOptions();
    }

    private void UpdateRefreshRateOptions()
    {
        if (refreshRateDropdown == null)
            return;

        string selectedRes = resolutionDropdown.options[resolutionDropdown.value].text;
        string[] parts = selectedRes.Split('x');
        if (parts.Length != 2) return;

        int targetWidth = int.Parse(parts[0]);
        int targetHeight = int.Parse(parts[1]);

        var rates = resolutions
            .Where(r => r.width == targetWidth && r.height == targetHeight)
            .Select(r => r.refreshRate)
            .Distinct()
            .OrderBy(r => r)
            .ToList();

        availableRefreshRates = rates;
        refreshRateDropdown.ClearOptions();
        List<string> rateOptions = new List<string>();
        foreach (int rate in rates)
        {
            rateOptions.Add(rate + " Hz");
        }
        refreshRateDropdown.AddOptions(rateOptions);
        refreshRateDropdown.RefreshShownValue();

        if (PlayerPrefs.HasKey("RefreshRatePreference"))
        {
            int savedRate = PlayerPrefs.GetInt("RefreshRatePreference");
            int idx = rates.IndexOf(savedRate);
            refreshRateDropdown.value = idx >= 0 ? idx : 0;
        }
        else
        {
            refreshRateDropdown.value = rates.Count - 1;
        }

        ApplyResolutionAndRefreshRate();
    }

    private void ApplyResolutionAndRefreshRate()
    {
        if (resolutionDropdown == null || refreshRateDropdown == null)
            return;

        string selectedRes = resolutionDropdown.options[resolutionDropdown.value].text;
        string[] parts = selectedRes.Split('x');
        int width = int.Parse(parts[0]);
        int height = int.Parse(parts[1]);

        int refreshRate = availableRefreshRates[refreshRateDropdown.value];

        Screen.SetResolution(width, height, Screen.fullScreen, refreshRate);
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    public void SetResolution(int resolutionIndex)
    {
        UpdateRefreshRateOptions();
    }

    public void SetRefreshRate(int index)
    {
        ApplyResolutionAndRefreshRate();
    }

    public void SaveSettings()
    {
        PlayerPrefs.SetInt("ResolutionPreference", resolutionDropdown.value);
        PlayerPrefs.SetInt("FullscreenPreference", System.Convert.ToInt32(Screen.fullScreen));
        if (refreshRateDropdown != null && availableRefreshRates.Count > 0)
            PlayerPrefs.SetInt("RefreshRatePreference", availableRefreshRates[refreshRateDropdown.value]);
    }

    public void LoadSettings(int currentResolutionIndex)
    {
        if (PlayerPrefs.HasKey("ResolutionPreference"))
            resolutionDropdown.value = PlayerPrefs.GetInt("ResolutionPreference");
        else
            resolutionDropdown.value = currentResolutionIndex;

        if (PlayerPrefs.HasKey("FullscreenPreference"))
            Screen.fullScreen = System.Convert.ToBoolean(PlayerPrefs.GetInt("FullscreenPreference"));
        else
            Screen.fullScreen = true;
    }
}