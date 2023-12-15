// Created by Victor Engstr�m
// Copyright 2023 Sonigon AB
// http://www.sonity.org/

using UnityEngine;

namespace Sonity.Internal {

    // ExecuteInEditMode so that e.g. SoundContainer & SoundTrigger distanceScale, guiWarnings, GetIsInSolo can be checked in editor
    [ExecuteInEditMode]
    public abstract class SoundManagerBase : MonoBehaviour {

        /// <summary>
        /// The static instance of the <see cref="SoundManagerBase">SoundManager</see>
        /// </summary>
        public static SoundManagerBase Instance { get; private set; }

        [SerializeField]
        private SoundManagerInternals internals = new SoundManagerInternals();

        /// <summary>
        /// The internal data of the <see cref="SoundManagerBase">SoundManager</see>
        /// </summary>
        public SoundManagerInternals Internals { get { return internals; } private set { internals = value; } }

        private void InstanceCheck() {
            if (Instance == null) {
                Instance = this;
                // Needed for disabling reload domain
                internals.isGoingToDelete = false;
                internals.cachedSoundManagerTransform = GetComponent<Transform>();
            } else if (Instance != this) {
                if (ShouldDebug.Warnings()) {
                    Debug.LogWarning($"There can only be one Sonity.{nameof(NameOf.SoundManager)} instance per scene.", this);
                }
                // So that it does not run the rest of the Awake and Update code
                internals.isGoingToDelete = true;
                if (Application.isPlaying) {
                    Destroy(gameObject);
                }
            }
        }

        // Needed for disabling reload domain
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        static void RuntimeInitializeOnLoad() {
            if (Instance != null) {
                Instance.Awake();
            }
        }

        private void Awake() {
            InstanceCheck();
            if (!internals.isGoingToDelete) {
                if (Application.isPlaying) {
                    internals.AwakeCheck();
                }
            }
        }

        private void Update() {
            if (!internals.isGoingToDelete) {
#if UNITY_EDITOR
                if (Application.isPlaying) {
#endif
                    internals.ManagedUpdate();
#if UNITY_EDITOR
                } else {
                    InstanceCheck();
                }
#endif
            }
        }

        private void OnDestroy() {
            // So that if the SoundManager is destroyed during eg. scene switching it will stop all playing sounds
            if (Application.isPlaying) {
                internals.Destroy();
            }
            // Needed for disabling reload domain. If the current SoundManager is removed, then find any other one.
            if (Instance == this) {
                Instance = null;
                SoundManagerBase tempSoundManagerBase = UnityEngine.Object.FindObjectOfType<SoundManagerBase>();
                if (tempSoundManagerBase != null) {
                    tempSoundManagerBase.InstanceCheck();
                }
            }
        }

#if UNITY_EDITOR
        private void OnGUI() {
            // Double check for DLL
            if (Application.isEditor) {
                // For live SoundEvent debugging
                EditorDebugInGameView.Update();
            }
        }
#endif

        private void OnApplicationQuit() {
            internals.applicationIsQuitting = true;
        }
    }
}