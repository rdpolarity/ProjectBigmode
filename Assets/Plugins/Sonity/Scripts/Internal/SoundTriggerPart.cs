// Created by Victor Engström
// Copyright 2023 Sonigon AB
// http://www.sonity.org/

using UnityEngine;
using System;

namespace Sonity.Internal {

    [Serializable]
    public class SoundTriggerPart {

        public void ResetTodo() {
            soundTriggerTodo = new SoundTriggerTodo();
            expandTrigger = true;
            triggerTagUse = false;
            triggerTags = new string[] { "Untagged" };
            collisionVelocityToIntensity = false;
            collisionTagUse = false;
            collisionTags = new string[] { "Untagged" };
        }

        public bool expandSoundEvent = true;
        public SoundEventBase soundEvent;
        public SoundEventModifier soundEventModifier = new SoundEventModifier();
        public bool expandModifier = true;
        public SoundTriggerTodo soundTriggerTodo = new SoundTriggerTodo();
        public bool expandTrigger = true;

        public bool triggerTagUse = false;
        public string[] triggerTags = { "Untagged" };

        public bool collisionVelocityToIntensity = false;
        public bool collisionTagUse = false;
        public string[] collisionTags = { "Untagged" };

        public void Play(Transform owner, SoundParameterInternals[] soundParameters, SoundParameterInternals soundParameterDistanceScale, SoundTagBase localSoundTag) {
            if (SoundManagerBase.Instance == null) {
                Debug.LogWarning($"Sonity.{nameof(NameOf.SoundManager)} is null. Add one to the scene.");
            } else {
                SoundManagerBase.Instance.Internals.InternalPlay(soundEvent, SoundEventPlayType.Play, owner, null, null, soundEventModifier, null, soundParameters, soundParameterDistanceScale, localSoundTag);
            }
        }

        public void PlayAtPosition(Transform owner, Transform position, SoundParameterInternals[] soundParameters, SoundParameterInternals soundParameterDistanceScale, SoundTagBase localSoundTag) {
            if (SoundManagerBase.Instance == null) {
                Debug.LogWarning($"Sonity.{nameof(NameOf.SoundManager)} is null. Add one to the scene.");
            } else {
                SoundManagerBase.Instance.Internals.InternalPlay(soundEvent, SoundEventPlayType.PlayAtTransform, owner, null, position, soundEventModifier, null, soundParameters, soundParameterDistanceScale, localSoundTag);
            }
        }

        public void PlayAtPosition(Transform owner, Vector3 position, SoundParameterInternals[] soundParameters, SoundParameterInternals soundParameterDistanceScale, SoundTagBase localSoundTag) {
            if (SoundManagerBase.Instance == null) {
                Debug.LogWarning($"Sonity.{nameof(NameOf.SoundManager)} is null. Add one to the scene.");
            } else {
                SoundManagerBase.Instance.Internals.InternalPlay(soundEvent, SoundEventPlayType.PlayAtVector, owner, position, null, soundEventModifier, null, soundParameters, soundParameterDistanceScale, localSoundTag);
            }
        }

        public void Stop(Transform owner, bool allowFadeOut) {
            if (SoundManagerBase.Instance == null) {
                Debug.LogWarning($"Sonity.{nameof(NameOf.SoundManager)} is null. Add one to the scene.");
            } else {
                SoundManagerBase.Instance.Internals.Stop(soundEvent, owner, allowFadeOut);
            }
        }

        public void StopAtPosition(Transform position, bool allowFadeOut) {
            if (SoundManagerBase.Instance == null) {
                Debug.LogWarning($"Sonity.{nameof(NameOf.SoundManager)} is null. Add one to the scene.");
            } else {
                SoundManagerBase.Instance.Internals.StopAtPosition(soundEvent, position, allowFadeOut);
            }
        }

        public void LoadAudioData() {
            if (soundEvent != null) {
                soundEvent.LoadAudioData();
            }
        }

        public void UnloadAudioData() {
            if (soundEvent != null) {
                soundEvent.UnloadAudioData();
            }
        }
    }
}