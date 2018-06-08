using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyboxManager : GameSingleton<SkyboxManager> {

    public delegate void UpdateProgress(float newProgress);

    public UpdateProgress updateProgress;

    [SerializeField]
    float _defaultProgressSmoothTime = 1.5f;

    bool _shifting = false;
    public bool shifting
    {
        get { return _shifting; }
    }

    Material _skyboxOriginal;
    Material _skyboxInstance;
    public  Material skybox
    {
        get { return _skyboxInstance; }
        set
        {
            RenderSettings.skybox = value;
            ReobtainSkybox();
        }
    }

    public void ReobtainSkybox()
    {
        Material s  = RenderSettings.skybox;

        if (s == null)
        {
            Debug.Log("[SkyboxManager] Current skybox is not the correct shader.");
            return;
        }

        _skyboxOriginal = s;
        _skyboxInstance = new Material(_skyboxOriginal);
        _skyboxInstance.name = s.name + " (Instance)";
        _skyboxInstance.hideFlags = HideFlags.DontSave;
        RenderSettings.skybox = _skyboxInstance;
    }

    public void ChangeProgress(float newProgress, float smoothTime = -1f)
    {
        StartCoroutine(SmoothProgress(newProgress, smoothTime));
    }

    private void Start()
    {
        // don't ever stop sending osc messages
        SteamVR_Render.instance.pauseGameWhenDashboardIsVisible = false;

        ReobtainSkybox();
    }

    IEnumerator SmoothProgress(float newProgress, float smoothTime)
    {
        _shifting = true;

        float progressTime = (smoothTime < 0f) ? _defaultProgressSmoothTime : smoothTime;
        float progressVelocity = 0f;
        float currentProgress = skybox.GetFloat("_Progress");
        while (Mathf.Abs(currentProgress - newProgress) > 1e-3)
        {
            currentProgress = Mathf.SmoothDamp(currentProgress, newProgress, ref progressVelocity, progressTime);
            skybox.SetFloat("_Progress", currentProgress);
            yield return null;
        }
        skybox.SetFloat("_Progress", newProgress);

        _shifting = false;
    }
}
