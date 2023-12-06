using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Bigmode
{
	public enum SettingKey
	{
		Width,
		Height,
		RefreshRate,
		FullscreenMode,
		Volume,
		ToggleADS,
	}

	public static class Settings
	{
		static Settings()
		{
			InputActions = new Controls();
			FetchStoredSettings();
		}

		public static Controls InputActions { get; private set; }
		
		public static event Action OnKeybindChanged;

		private static void FetchStoredSettings()
		{
			SetupScreen();
			LoadKeybinds();
			LoadGameplaySettings();
			LoadAudioSettings();
			PlayerPrefs.Save();
		}

		private static void LoadAudioSettings()
		{
			AudioListener.volume = GetFloat(SettingKey.Volume, 1f);
		}

		private static void LoadGameplaySettings()
		{
			//ToggleADS = GetBool(SettingKey.ToggleADS, true);
		}

		private static void SetupScreen()
		{
			var width = GetInt(SettingKey.Width, Screen.currentResolution.width);
			var height = GetInt(SettingKey.Height, Screen.currentResolution.height);
			var hz = (uint)GetInt(SettingKey.RefreshRate, (int)Screen.currentResolution.refreshRateRatio.numerator);
			var stringMode = GetString(SettingKey.FullscreenMode, Screen.fullScreenMode.ToString());

			var mode = Screen.fullScreenMode;
			if (Enum.TryParse<FullScreenMode>(stringMode, out var result))
			{
				mode = result;
			}

			Screen.SetResolution(width, height, mode, new RefreshRate { numerator = hz, denominator =  1});
		}

		private static void LoadKeybinds()
		{
			foreach (var action in InputActions)
			{
				for (var i = 0; i < action.bindings.Count; i++)
				{
					var binding = action.bindings[i];
					var key = $"{action.name},{binding.name}";

					if (!PlayerPrefs.HasKey(key)) continue;
					
					if (PlayerPrefs.GetString(key) == "")
						PlayerPrefs.DeleteKey(key);
					else
						action.ApplyBindingOverride(i, PlayerPrefs.GetString(key));
				}
			}
			OnKeybindChanged?.Invoke();
		}

		public static void SaveActionKeybind(InputAction action)
		{
			foreach (var binding in action.bindings)
			{
				var key = $"{action.name}.{binding.name}";

				if (binding.overridePath == "")
				{
					if (PlayerPrefs.HasKey(key))
						PlayerPrefs.DeleteKey(key);
				}
				else
				{
					PlayerPrefs.SetString(key, binding.overridePath);
				}
			}

			OnKeybindChanged?.Invoke();
		}

		public static void SaveAllKeybinds()
		{
			foreach (var action in InputActions)
			{
				SaveActionKeybind(action);
			}
			PlayerPrefs.Save();
			OnKeybindChanged?.Invoke();
		}

		public static int GetInt(SettingKey key)
		{
			return PlayerPrefs.GetInt(key.ToString());
		}

		public static int GetInt(SettingKey key, int defaultValue)
		{
			return PlayerPrefs.GetInt(key.ToString(), defaultValue);
		}

		public static float GetFloat(SettingKey key, float defaultValue)
		{
			return PlayerPrefs.GetFloat(key.ToString(), defaultValue);
		}

		public static bool GetBool(SettingKey key)
		{
			return PlayerPrefs.GetInt(key.ToString()) == 1;
		}

		public static bool GetBool(SettingKey key, bool defaultValue)
		{
			return PlayerPrefs.GetInt(key.ToString(), defaultValue ? 1 : 0) == 1;
		}

		public static string GetString(SettingKey key)
		{
			return PlayerPrefs.GetString(key.ToString());
		}

		public static string GetString(SettingKey key, string defaultValue)
		{
			return PlayerPrefs.GetString(key.ToString(), defaultValue);
		}

		public static void SetInt(SettingKey key, int value)
		{
			PlayerPrefs.SetInt(key.ToString(), value);
			PlayerPrefs.Save();
		}

		public static void SetBool(SettingKey key, bool value)
		{
			PlayerPrefs.SetInt(key.ToString(), value ? 1 : 0);
			PlayerPrefs.Save();
		}

		public static void SetString(SettingKey key, string value)
		{
			PlayerPrefs.SetString(key.ToString(), value);
			PlayerPrefs.Save();
		}

		public static void SetFloat(SettingKey key, float value)
		{
			PlayerPrefs.SetFloat(key.ToString(), value);
			PlayerPrefs.Save();
		}
	}
}