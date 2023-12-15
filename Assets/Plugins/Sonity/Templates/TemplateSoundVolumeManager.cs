// Created by Victor Engström
// Copyright 2023 Sonigon AB
// http://www.sonity.org/

using UnityEngine;
using UnityEngine.Audio;

namespace SonityTemplate {

    /// <summary>
    /// Template of a singleton <see cref="AudioMixer"/> volume controller.
    /// The default settings work with the provided "TemplateAudioMixerWithLimiter.mixer".
    /// Add to a GameObject in the scene and use like this:
    /// SonityTemplate.TemplateSoundVolumeManager.Instance.SetVolumeMaster(1f);
    /// </summary>
    [AddComponentMenu("Sonity/Template - Sound Volume Manager")]
    public class TemplateSoundVolumeManager : MonoBehaviour {

        public AudioMixer audioMixer;

        // This sets the volume range in decibel scaled from 0-1
        private float lowestVolumeDb = -60f;

        // The names need to match the names of the exposed parameters in the mixer
        private string parameterNameMaster = "MasterPrivateVolume";
        private float volumeOffsetDbMaster = 0f;

        private string parameterNameSounds = "SoundsVolume";
        private float volumeOffsetDbSounds = 0f;

        private string parameterNameMusic = "MusicVolume";
        private float volumeOffsetDbMusic = 0f;

        /// <summary>
        /// Set <see cref="AudioMixer"/> volumes
        /// Input is range 0 to 1
        /// Its converted to a -60 to -0dB scale (lowest volume assignable by lowestVolumeDb)
        /// 0 will clamp to -80dB (-infinity in the <see cref="AudioMixer"/>)
        /// </summary>
        public void SetVolumeMaster(float volumeLinear) {
            if (audioMixer == null) {
#if UNITY_EDITOR
                Debug.LogWarning($"You need to assign an {nameof(AudioMixer)} to set volumes.", this);
#endif
                return;
            }
            volumeLinear = Mathf.Clamp(volumeLinear, 0f, 1f);
            // Invert and convert to dB
            volumeLinear = (1f - volumeLinear) * lowestVolumeDb;
            // Snap to -infinity
            if (volumeLinear <= lowestVolumeDb) {
                volumeLinear = -80f;
            } else {
                volumeLinear += volumeOffsetDbMaster;
            }
            // Set volume
            audioMixer.SetFloat(parameterNameMaster, volumeLinear);
        }

        /// <summary>
        /// Set <see cref="AudioMixer"/> volumes
        /// Input is range 0 to 1
        /// Its converted to a -60 to -0dB scale (lowest volume assignable by lowestVolumeDb)
        /// 0 will clamp to -80dB (-infinity in the <see cref="AudioMixer"/>)
        /// </summary>
        public void SetVolumeSounds(float volumeLinear) {
            if (audioMixer == null) {
#if UNITY_EDITOR
                Debug.LogWarning($"You need to assign an {nameof(AudioMixer)} to set volumes.", this);
#endif
                return;
            }
            volumeLinear = Mathf.Clamp(volumeLinear, 0f, 1f);
            // Invert and convert to dB
            volumeLinear = (1f - volumeLinear) * lowestVolumeDb;
            // Snap to -infinity
            if (volumeLinear <= lowestVolumeDb) {
                volumeLinear = -80f;
            } else {
                volumeLinear += volumeOffsetDbSounds;
            }
            // Set volume
            audioMixer.SetFloat(parameterNameSounds, volumeLinear);
        }

        /// <summary>
        /// Set <see cref="AudioMixer"/> volumes
        /// Input is range 0 to 1
        /// Its converted to a -60 to -0dB scale (lowest volume assignable by lowestVolumeDb)
        /// 0 will clamp to -80dB (-infinity in the <see cref="AudioMixer"/>)
        /// </summary>
        public void SetVolumeMusic(float volumeLinear) {
            if (audioMixer == null) {
#if UNITY_EDITOR
                Debug.LogWarning($"You need to assign an {nameof(AudioMixer)} to set volumes.", this);
#endif
                return;
            }
            volumeLinear = Mathf.Clamp(volumeLinear, 0f, 1f);
            // Invert and convert to dB
            volumeLinear = (1f - volumeLinear) * lowestVolumeDb;
            // Snap to -infinity
            if (volumeLinear <= lowestVolumeDb) {
                volumeLinear = -80f;
            } else {
                volumeLinear += volumeOffsetDbMusic;
            }
            // Set volume
            audioMixer.SetFloat(parameterNameMusic, volumeLinear);
        }

        // Singleton Instance
        public bool useDontDestroyOnLoad = true;
        private bool isGoingToDelete;
#if UNITY_EDITOR
        private bool debugInstanceDestroyed = true;
#endif

        public static TemplateSoundVolumeManager Instance {
            get;
            private set;
        }

        private void Awake() {
            InstanceCheck();
            if (useDontDestroyOnLoad && !isGoingToDelete) {
                // DontDestroyOnLoad only works for root GameObjects
                gameObject.transform.parent = null;
                DontDestroyOnLoad(gameObject);
            }
        }

        // Checks if there are multiple instances this script, if so it destroys one of them
        private void InstanceCheck() {
            if (Instance == null) {
                Instance = this;
            } else if (Instance != this) {
#if UNITY_EDITOR
                if (debugInstanceDestroyed) {
                    Debug.LogWarning($"There can only be one {nameof(TemplateSoundVolumeManager)} instance per scene.", this);
                }
#endif
                // So that it does not run the rest of the Awake and Update code
                isGoingToDelete = true;
                if (Application.isPlaying) {
                    Destroy(gameObject);
                }
            }
        }
    }
}