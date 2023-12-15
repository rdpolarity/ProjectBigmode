// Created by Victor Engström
// Copyright 2023 Sonigon AB
// http://www.sonity.org/

using UnityEngine;
using System;


namespace Sonity.Internal {

    [Serializable]
    public class SoundPickerPart {

        public SoundEventBase soundEvent;
        public SoundEventModifier soundEventModifier = new SoundEventModifier();
        public bool expandModifier = true;

        public void Play(Transform owner, SoundParameterInternals[] soundParameters, SoundParameterInternals soundParameterDistanceScale, SoundTagBase localSoundTag) {
            SoundManagerBase.Instance.Internals.InternalPlay(soundEvent, SoundEventPlayType.Play, owner, null, null, soundEventModifier, null, soundParameters, soundParameterDistanceScale, localSoundTag);
        }

        public void PlayAtPosition(Transform owner, Transform position, SoundParameterInternals[] soundParameters, SoundParameterInternals soundParameterDistanceScale, SoundTagBase localSoundTag) {
            SoundManagerBase.Instance.Internals.InternalPlay(soundEvent, SoundEventPlayType.PlayAtTransform, owner, null, position, soundEventModifier, null, soundParameters, soundParameterDistanceScale, localSoundTag);
        }

        public void PlayAtPosition(Transform owner, Vector3 position, SoundParameterInternals[] soundParameters, SoundParameterInternals soundParameterDistanceScale, SoundTagBase localSoundTag) {
            SoundManagerBase.Instance.Internals.InternalPlay(soundEvent, SoundEventPlayType.PlayAtVector, owner, position, null, soundEventModifier, null, soundParameters, soundParameterDistanceScale, localSoundTag);
        }

        public void Stop(Transform owner, bool allowFadeOut) {
            SoundManagerBase.Instance.Internals.Stop(soundEvent, owner, allowFadeOut);
        }

        public void StopAtPosition(Transform position, bool allowFadeOut) {
            SoundManagerBase.Instance.Internals.StopAtPosition(soundEvent, position, allowFadeOut);
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