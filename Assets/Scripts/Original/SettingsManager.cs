using System;
using UnityEngine;

// Token: 0x0200009A RID: 154
public class SettingsManager : MonoBehaviour
{
	// Token: 0x1700001C RID: 28
	// (get) Token: 0x0600022C RID: 556 RVA: 0x0000FA22 File Offset: 0x0000DE22
	public static SettingsManager Singleton
	{
		get
		{
			return SettingsManager.singleton;
		}
	}

	// Token: 0x0600022D RID: 557 RVA: 0x0000FA29 File Offset: 0x0000DE29
	private void Awake()
	{
		if (SettingsManager.singleton != null)
		{
			UnityEngine.Object.Destroy(SettingsManager.singleton);
		}
		SettingsManager.singleton = this;
		this.Inititalize();
	}

	// Token: 0x0600022E RID: 558 RVA: 0x0000FA54 File Offset: 0x0000DE54
	public void Inititalize()
	{
		if (PlayerPrefs.GetInt("settingsInitialized") == 0)
		{
			PlayerPrefs.SetFloat("gameplay_Xsens", this.defaultXAxisSensibility);
			PlayerPrefs.SetFloat("gameplay_Ysens", this.defaultYAxisSensibility);
			PlayerPrefs.SetInt("gameplay_Xinvert", this.defaultXAxisInvert);
			PlayerPrefs.SetInt("gameplay_Yinvert", this.defaultYAxisInvert);
			PlayerPrefs.SetInt("gameplay_zenmode", this.defaultZenMode);
			PlayerPrefs.SetInt("gameplay_showCurrentScore", this.defaultShowCurrentScore);
			PlayerPrefs.SetInt("video_antialiasing", this.defaultAA);
			PlayerPrefs.SetFloat("video_brightness", this.defaultBrightness);
			PlayerPrefs.SetInt("video_shadowquality", this.defaultShadowQuality);
			PlayerPrefs.SetInt("video_shadowdistance", this.defaultShadowDistance);
			PlayerPrefs.SetInt("video_postprocessing", this.defaultPostProcessing);
			PlayerPrefs.SetInt("video_motionblur", this.defaultMotionBlur);
			PlayerPrefs.SetFloat("audio_master", this.defaultMasterVolume);
			PlayerPrefs.SetFloat("audio_wind", this.defaultWindVolume);
			PlayerPrefs.SetFloat("audio_scoring", this.defaultScoreVolume);
			PlayerPrefs.SetFloat("audio_effects", this.defaultEffectsVolume);
			PlayerPrefs.SetFloat("audio_menu", this.defaultMenuVolume);
			PlayerPrefs.SetInt("settingsInitialized", 1);
			PlayerPrefs.Save();
		}
		if (PlayerPrefs.GetInt("update1_1Initialized") == 0)
		{
			PlayerPrefs.SetFloat("audio_menu", this.defaultMenuVolume);
			PlayerPrefs.SetInt("update1_1Initialized", 1);
			PlayerPrefs.Save();
		}
		SettingsManager.xAxisInverted = this.GetXAxisInvert();
		SettingsManager.yAxisInverted = this.GetYAxisInvert();
		SettingsManager.xAxisSensibility = this.GetXAxisSensiblity();
		SettingsManager.yAxisSensibility = this.GetYAxisSensibility();
		SettingsManager.zenMode = this.GetZenMode();
		SettingsManager.showCurrentScore = this.GetShowCurrentScore();
		//LocalGameManager.Singleton.ZenModus = SettingsManager.zenMode;
		QualitySettings.antiAliasing = this.GetAntiAliasing();
		switch (this.GetShadowQuality())
		{
		case 0:
			QualitySettings.shadowResolution = ShadowResolution.Low;
			break;
		case 1:
			QualitySettings.shadowResolution = ShadowResolution.Medium;
			break;
		case 2:
			QualitySettings.shadowResolution = ShadowResolution.High;
			break;
		case 3:
			QualitySettings.shadowResolution = ShadowResolution.VeryHigh;
			break;
		}
		//this.brightnessSettings.SetAmbientCurrentValue(this.GetBrightness());
		QualitySettings.shadowDistance = (float)this.GetShadowDistance();
		//if (this.GetPostProcessing())
		//{
		//	this.postProcessing.enabled = true;
		//}
		//else
		//{
		//	this.postProcessing.enabled = false;
		//}
		//if (this.GetMotionBlur())
		//{
		//	this.motionBlur.enabled = true;
		//}
		//else
		//{
		//	this.motionBlur.enabled = false;
		//}
		SettingsManager.masterVolume = this.GetMasterVolume();
		SettingsManager.windVolume = this.GetWindVolume();
		SettingsManager.scoreVolume = this.GetScoringVolume();
		SettingsManager.effectsVolume = this.GetEffectsVolume();
		SettingsManager.menuVolume = this.GetMenuVolume();
	}

	// Token: 0x0600022F RID: 559 RVA: 0x0000FD0D File Offset: 0x0000E10D
	public void SetZenMode(bool value)
	{
		if (value)
		{
			PlayerPrefs.SetInt("gameplay_zenmode", 1);
		}
		else
		{
			PlayerPrefs.SetInt("gameplay_zenmode", 0);
		}
		PlayerPrefs.Save();
		SettingsManager.zenMode = value;
		//LocalGameManager.Singleton.ZenModus = SettingsManager.zenMode;
	}

	// Token: 0x06000230 RID: 560 RVA: 0x0000FD4A File Offset: 0x0000E14A
	public bool GetZenMode()
	{
		return PlayerPrefs.GetInt("gameplay_zenmode") != 0;
	}

	// Token: 0x06000231 RID: 561 RVA: 0x0000FD60 File Offset: 0x0000E160
	public void SetShowCurrentScore(bool value)
	{
		if (value)
		{
			PlayerPrefs.SetInt("gameplay_showCurrentScore", 1);
			//UIManager.Singleton.currentScore.gameObject.SetActive(true);
		}
		else
		{
			PlayerPrefs.SetInt("gameplay_showCurrentScore", 0);
			//UIManager.Singleton.currentScore.gameObject.SetActive(false);
		}
		PlayerPrefs.Save();
		SettingsManager.showCurrentScore = value;
	}

	// Token: 0x06000232 RID: 562 RVA: 0x0000FDC3 File Offset: 0x0000E1C3
	public bool GetShowCurrentScore()
	{
		if (PlayerPrefs.GetInt("gameplay_showCurrentScore") == 0)
		{
			//UIManager.Singleton.currentScore.gameObject.SetActive(false);
			return false;
		}
		//UIManager.Singleton.currentScore.gameObject.SetActive(true);
		return true;
	}

	// Token: 0x06000233 RID: 563 RVA: 0x0000FE01 File Offset: 0x0000E201
	public void SetXAxisInvert(bool value)
	{
		if (value)
		{
			PlayerPrefs.SetInt("gameplay_Xinvert", 1);
		}
		else
		{
			PlayerPrefs.SetInt("gameplay_Xinvert", 0);
		}
		PlayerPrefs.Save();
		SettingsManager.xAxisInverted = value;
	}

	// Token: 0x06000234 RID: 564 RVA: 0x0000FE2F File Offset: 0x0000E22F
	public bool GetXAxisInvert()
	{
		return PlayerPrefs.GetInt("gameplay_Xinvert") != 0;
	}

	// Token: 0x06000235 RID: 565 RVA: 0x0000FE43 File Offset: 0x0000E243
	public void SetYAxisInvert(bool value)
	{
		if (value)
		{
			PlayerPrefs.SetInt("gameplay_Yinvert", 1);
		}
		else
		{
			PlayerPrefs.SetInt("gameplay_Yinvert", 0);
		}
		PlayerPrefs.Save();
		SettingsManager.yAxisInverted = value;
	}

	// Token: 0x06000236 RID: 566 RVA: 0x0000FE71 File Offset: 0x0000E271
	public bool GetYAxisInvert()
	{
		return PlayerPrefs.GetInt("gameplay_Yinvert") != 0;
	}

	// Token: 0x06000237 RID: 567 RVA: 0x0000FE85 File Offset: 0x0000E285
	public void SetXAxisSensibility(float value)
	{
		PlayerPrefs.SetFloat("gameplay_Xsens", value);
		PlayerPrefs.Save();
		SettingsManager.xAxisSensibility = value;
	}

	// Token: 0x06000238 RID: 568 RVA: 0x0000FE9D File Offset: 0x0000E29D
	public float GetXAxisSensiblity()
	{
		return PlayerPrefs.GetFloat("gameplay_Xsens");
	}

	// Token: 0x06000239 RID: 569 RVA: 0x0000FEA9 File Offset: 0x0000E2A9
	public void SetYAxisSensibility(float value)
	{
		PlayerPrefs.SetFloat("gameplay_Ysens", value);
		PlayerPrefs.Save();
		SettingsManager.yAxisSensibility = value;
	}

	// Token: 0x0600023A RID: 570 RVA: 0x0000FEC1 File Offset: 0x0000E2C1
	public float GetYAxisSensibility()
	{
		return PlayerPrefs.GetFloat("gameplay_Ysens");
	}

	// Token: 0x0600023B RID: 571 RVA: 0x0000FECD File Offset: 0x0000E2CD
	public void SetAntiAliasing(int value)
	{
		PlayerPrefs.SetInt("video_antialiasing", value);
		PlayerPrefs.Save();
		QualitySettings.antiAliasing = value;
	}

	// Token: 0x0600023C RID: 572 RVA: 0x0000FEE5 File Offset: 0x0000E2E5
	public int GetAntiAliasing()
	{
		return PlayerPrefs.GetInt("video_antialiasing");
	}

	// Token: 0x0600023D RID: 573 RVA: 0x0000FEF1 File Offset: 0x0000E2F1
	public void SetBrightness(float value)
	{
		PlayerPrefs.SetFloat("video_brightness", value);
		PlayerPrefs.Save();
		//this.brightnessSettings.SetAmbientCurrentValue(value);
	}

	// Token: 0x0600023E RID: 574 RVA: 0x0000FF0F File Offset: 0x0000E30F
	public float GetBrightness()
	{
		return PlayerPrefs.GetFloat("video_brightness");
	}

	// Token: 0x0600023F RID: 575 RVA: 0x0000FF1C File Offset: 0x0000E31C
	public void SetShadowQuality(int value)
	{
		PlayerPrefs.SetInt("video_shadowquality", value);
		PlayerPrefs.Save();
		switch (value)
		{
		case 0:
			QualitySettings.shadowResolution = ShadowResolution.Low;
			break;
		case 1:
			QualitySettings.shadowResolution = ShadowResolution.Medium;
			break;
		case 2:
			QualitySettings.shadowResolution = ShadowResolution.High;
			break;
		case 3:
			QualitySettings.shadowResolution = ShadowResolution.VeryHigh;
			break;
		}
	}

	// Token: 0x06000240 RID: 576 RVA: 0x0000FF80 File Offset: 0x0000E380
	public int GetShadowQuality()
	{
		return PlayerPrefs.GetInt("video_shadowquality");
	}

	// Token: 0x06000241 RID: 577 RVA: 0x0000FF8C File Offset: 0x0000E38C
	public void SetShadowDistance(int value)
	{
		PlayerPrefs.SetInt("video_shadowdistance", value);
		PlayerPrefs.Save();
		QualitySettings.shadowDistance = (float)value;
	}

	// Token: 0x06000242 RID: 578 RVA: 0x0000FFA5 File Offset: 0x0000E3A5
	public int GetShadowDistance()
	{
		return PlayerPrefs.GetInt("video_shadowdistance");
	}

	// Token: 0x06000243 RID: 579 RVA: 0x0000FFB1 File Offset: 0x0000E3B1
	public void SetPostProcessing(bool value)
	{
		if (value)
		{
			PlayerPrefs.SetInt("video_postprocessing", 1);
		}
		else
		{
			PlayerPrefs.SetInt("video_postprocessing", 0);
		}
		PlayerPrefs.Save();
		//this.postProcessing.enabled = value;
	}

	// Token: 0x06000244 RID: 580 RVA: 0x0000FFE5 File Offset: 0x0000E3E5
	public bool GetPostProcessing()
	{
		return PlayerPrefs.GetInt("video_postprocessing") != 0;
	}

	// Token: 0x06000245 RID: 581 RVA: 0x0000FFF9 File Offset: 0x0000E3F9
	public void SetMotionBlur(bool value)
	{
		if (value)
		{
			PlayerPrefs.SetInt("video_motionblur", 1);
		}
		else
		{
			PlayerPrefs.SetInt("video_motionblur", 0);
		}
		PlayerPrefs.Save();
		//this.motionBlur.enabled = value;
	}

	// Token: 0x06000246 RID: 582 RVA: 0x0001002D File Offset: 0x0000E42D
	public bool GetMotionBlur()
	{
		return PlayerPrefs.GetInt("video_motionblur") != 0;
	}

	// Token: 0x06000247 RID: 583 RVA: 0x00010041 File Offset: 0x0000E441
	public void SetMasterVolume(float value)
	{
		PlayerPrefs.SetFloat("audio_master", value);
		PlayerPrefs.Save();
		SettingsManager.masterVolume = value;
	}

	// Token: 0x06000248 RID: 584 RVA: 0x00010059 File Offset: 0x0000E459
	public float GetMasterVolume()
	{
		return PlayerPrefs.GetFloat("audio_master");
	}

	// Token: 0x06000249 RID: 585 RVA: 0x00010065 File Offset: 0x0000E465
	public void SetWindVolume(float value)
	{
		PlayerPrefs.SetFloat("audio_wind", value);
		PlayerPrefs.Save();
		SettingsManager.windVolume = value;
	}

	// Token: 0x0600024A RID: 586 RVA: 0x0001007D File Offset: 0x0000E47D
	public float GetWindVolume()
	{
		return PlayerPrefs.GetFloat("audio_wind");
	}

	// Token: 0x0600024B RID: 587 RVA: 0x00010089 File Offset: 0x0000E489
	public void SetScoringVolume(float value)
	{
		PlayerPrefs.SetFloat("audio_scoring", value);
		PlayerPrefs.Save();
		SettingsManager.scoreVolume = value;
	}

	// Token: 0x0600024C RID: 588 RVA: 0x000100A1 File Offset: 0x0000E4A1
	public float GetScoringVolume()
	{
		return PlayerPrefs.GetFloat("audio_scoring");
	}

	// Token: 0x0600024D RID: 589 RVA: 0x000100AD File Offset: 0x0000E4AD
	public void SetEffectsVolume(float value)
	{
		PlayerPrefs.SetFloat("audio_effects", value);
		PlayerPrefs.Save();
		SettingsManager.effectsVolume = value;
	}

	// Token: 0x0600024E RID: 590 RVA: 0x000100C5 File Offset: 0x0000E4C5
	public float GetEffectsVolume()
	{
		return PlayerPrefs.GetFloat("audio_effects");
	}

	// Token: 0x0600024F RID: 591 RVA: 0x000100D1 File Offset: 0x0000E4D1
	public void SetMenuVolume(float value)
	{
		PlayerPrefs.SetFloat("audio_menu", value);
		PlayerPrefs.Save();
		SettingsManager.menuVolume = value;
	}

	// Token: 0x06000250 RID: 592 RVA: 0x000100E9 File Offset: 0x0000E4E9
	public float GetMenuVolume()
	{
		return PlayerPrefs.GetFloat("audio_menu");
	}

	// Token: 0x04000339 RID: 825
	private static SettingsManager singleton;

	// Token: 0x0400033A RID: 826
	public static bool xAxisInverted;

	// Token: 0x0400033B RID: 827
	public static bool yAxisInverted;

	// Token: 0x0400033C RID: 828
	public static float xAxisSensibility;

	// Token: 0x0400033D RID: 829
	public static float yAxisSensibility;

	// Token: 0x0400033E RID: 830
	public static bool zenMode;

	// Token: 0x0400033F RID: 831
	public static bool showCurrentScore;

	// Token: 0x04000340 RID: 832
	public static float masterVolume;

	// Token: 0x04000341 RID: 833
	public static float windVolume;

	// Token: 0x04000342 RID: 834
	public static float scoreVolume;

	// Token: 0x04000343 RID: 835
	public static float effectsVolume;

	// Token: 0x04000344 RID: 836
	public static float menuVolume;

	// Token: 0x04000345 RID: 837
	//[SerializeField]
	//private AmplifyMotionEffect motionBlur;

	// Token: 0x04000346 RID: 838
	//[SerializeField]
	//private VignetteAndChromaticAberration postProcessing;

	// Token: 0x04000347 RID: 839
	//[SerializeField]
	//private Settings brightnessSettings;

	// Token: 0x04000348 RID: 840
	private float defaultXAxisSensibility = 1f;

	// Token: 0x04000349 RID: 841
	private float defaultYAxisSensibility = 1f;

	// Token: 0x0400034A RID: 842
	private int defaultXAxisInvert;

	// Token: 0x0400034B RID: 843
	private int defaultYAxisInvert;

	// Token: 0x0400034C RID: 844
	private int defaultZenMode;

	// Token: 0x0400034D RID: 845
	private int defaultShowCurrentScore;

	// Token: 0x0400034E RID: 846
	private int defaultAA = 4;

	// Token: 0x0400034F RID: 847
	private int defaultShadowQuality = 1;

	// Token: 0x04000350 RID: 848
	private float defaultBrightness = 0.3f;

	// Token: 0x04000351 RID: 849
	private int defaultShadowDistance = 4000;

	// Token: 0x04000352 RID: 850
	private int defaultMotionBlur = 1;

	// Token: 0x04000353 RID: 851
	private int defaultPostProcessing = 1;

	// Token: 0x04000354 RID: 852
	private float defaultMasterVolume = 1f;

	// Token: 0x04000355 RID: 853
	private float defaultWindVolume = 1f;

	// Token: 0x04000356 RID: 854
	private float defaultScoreVolume = 1f;

	// Token: 0x04000357 RID: 855
	private float defaultEffectsVolume = 1f;

	// Token: 0x04000358 RID: 856
	private float defaultMenuVolume = 1f;

	// Token: 0x04000359 RID: 857
	private const string varname_SettingsInitialized = "settingsInitialized";

	// Token: 0x0400035A RID: 858
	private const string varname_Updated1_1Initialized = "update1_1Initialized";

	// Token: 0x0400035B RID: 859
	private const string varname_Xinvert = "gameplay_Xinvert";

	// Token: 0x0400035C RID: 860
	private const string varname_Yinvert = "gameplay_Yinvert";

	// Token: 0x0400035D RID: 861
	private const string varname_Xsens = "gameplay_Xsens";

	// Token: 0x0400035E RID: 862
	private const string varname_Ysens = "gameplay_Ysens";

	// Token: 0x0400035F RID: 863
	private const string varname_ZenMode = "gameplay_zenmode";

	// Token: 0x04000360 RID: 864
	private const string varname_ShowCurrentScore = "gameplay_showCurrentScore";

	// Token: 0x04000361 RID: 865
	private const string varname_Antialiasing = "video_antialiasing";

	// Token: 0x04000362 RID: 866
	private const string varname_Brightness = "video_brightness";

	// Token: 0x04000363 RID: 867
	private const string varname_ShadowQuality = "video_shadowquality";

	// Token: 0x04000364 RID: 868
	private const string varname_ShadowDistance = "video_shadowdistance";

	// Token: 0x04000365 RID: 869
	private const string varname_PostProcessing = "video_postprocessing";

	// Token: 0x04000366 RID: 870
	private const string varname_MotionBlur = "video_motionblur";

	// Token: 0x04000367 RID: 871
	private const string varname_AudioMaster = "audio_master";

	// Token: 0x04000368 RID: 872
	private const string varname_AudioWind = "audio_wind";

	// Token: 0x04000369 RID: 873
	private const string varname_AudioScoring = "audio_scoring";

	// Token: 0x0400036A RID: 874
	private const string varname_AudioEffects = "audio_effects";

	// Token: 0x0400036B RID: 875
	private const string varname_AudioMenu = "audio_menu";
}
