// Created by Victor Engström
// Copyright 2023 Sonigon AB
// http://www.sonity.org/

using UnityEngine;
using System;
using Sonity.Internal;

namespace Sonity {

    /// <summary>
    /// <see cref="SoundEventBase">SoundEvents</see> are what you play in Sonity.
    /// They contain <see cref="SoundContainerBase">SoundContainer</see> and options of how the sound should be played.
    /// All <see cref="SoundEventBase">SoundEvents</see> are multi-object editable.
    /// </summary>
    [Serializable]
    [CreateAssetMenu(fileName = "_SE", menuName = "Sonity/SoundEvent", order = 2)]
    public class SoundEvent : SoundEventBase {

        /// <summary>
        /// Plays the <see cref="SoundEventBase">SoundEvent</see> at the <see cref="Transform"/> position
        /// </summary>
        /// <param name='owner'>
        /// The owner <see cref="Transform"/> (can follow positon)
        /// </param>
        public void Play(Transform owner) {
            if (SoundManagerBase.Instance == null) {
                Debug.LogWarning($"Sonity.{nameof(NameOf.SoundManager)} is null. Add one to the scene.");
            } else {
                SoundManagerBase.Instance.Internals.Play(this, owner);
            }
        }

        /// <summary>
        /// Plays the <see cref="SoundEventBase">SoundEvent</see> at the <see cref="Transform"/> position with the Local <see cref="SoundTagBase">SoundTag</see>
        /// </summary>
        /// <param name='owner'>
        /// The owner <see cref="Transform"/> (can follow positon)
        /// </param>
        /// <param name='localSoundTag'>
        /// The <see cref="SoundTagBase">SoundTag</see> which will determine the Local <see cref="SoundTagBase">SoundTag</see> of the <see cref="SoundEventBase">SoundEvent</see>
        /// </param>
        public void Play(Transform owner, SoundTagBase localSoundTag) {
            if (SoundManagerBase.Instance == null) {
                Debug.LogWarning($"Sonity.{nameof(NameOf.SoundManager)} is null. Add one to the scene.");
            } else {
                SoundManagerBase.Instance.Internals.Play(this, owner, localSoundTag);
            }
        }

        /// <summary>
        /// Plays the <see cref="SoundEventBase">SoundEvent</see> with <see cref="SoundParameterInternals"/> at the <see cref="Transform"/> position
        /// </summary>
        /// <param name='owner'>
        /// The owner <see cref="Transform"/> (can follow positon)
        /// </param>
        /// <param name='soundParameters'>
        /// For example <see cref="SoundParameterVolumeDecibel"/> is used to modify how the <see cref="SoundEventBase">SoundEvent</see> is played
        /// </param>
        public void Play(Transform owner, params SoundParameterInternals[] soundParameters) {
            if (SoundManagerBase.Instance == null) {
                Debug.LogWarning($"Sonity.{nameof(NameOf.SoundManager)} is null. Add one to the scene.");
            } else {
                SoundManagerBase.Instance.Internals.Play(this, owner, soundParameters);
            }
        }

        /// <summary>
        /// Plays the <see cref="SoundEventBase">SoundEvent</see> with <see cref="SoundParameterInternals"/> at the <see cref="Transform"/> position with the Local <see cref="SoundTagBase">SoundTag</see>
        /// </summary>
        /// <param name='owner'>
        /// The owner <see cref="Transform"/> (can follow positon)
        /// </param>
        /// <param name='localSoundTag'>
        /// The <see cref="SoundTagBase">SoundTag</see> which will determine the Local <see cref="SoundTagBase">SoundTag</see> of the <see cref="SoundEventBase">SoundEvent</see>
        /// </param>
        /// <param name='soundParameters'>
        /// For example <see cref="SoundParameterVolumeDecibel"/> is used to modify how the <see cref="SoundEventBase">SoundEvent</see> is played
        /// </param>
        public void Play(Transform owner, SoundTagBase localSoundTag, params SoundParameterInternals[] soundParameters) {
            if (SoundManagerBase.Instance == null) {
                Debug.LogWarning($"Sonity.{nameof(NameOf.SoundManager)} is null. Add one to the scene.");
            } else {
                SoundManagerBase.Instance.Internals.Play(this, owner, localSoundTag, soundParameters);
            }
        }

        /// <summary>
        /// Plays the <see cref="SoundEventBase">SoundEvent</see> at the <see cref="Transform"/> position with another <see cref="Transform"/> owner
        /// </summary>
        /// <param name='owner'>
        /// The owner <see cref="Transform"/>
        /// </param>
        /// <param name='position'>
        /// The position <see cref="Transform"/> (can follow position)
        /// </param>
        public void PlayAtPosition(Transform owner, Transform position) {
            if (SoundManagerBase.Instance == null) {
                Debug.LogWarning($"Sonity.{nameof(NameOf.SoundManager)} is null. Add one to the scene.");
            } else {
                SoundManagerBase.Instance.Internals.PlayAtPosition(this, owner, position);
            }
        }

        /// <summary>
        /// Plays the <see cref="SoundEventBase">SoundEvent</see> at the <see cref="Transform"/> position with another <see cref="Transform"/> owner with the Local <see cref="SoundTagBase">SoundTag</see>
        /// </summary>
        /// <param name='owner'>
        /// The owner <see cref="Transform"/>
        /// </param>
        /// <param name='position'>
        /// The position <see cref="Transform"/> (can follow position)
        /// </param>
        /// <param name='localSoundTag'>
        /// The <see cref="SoundTagBase">SoundTag</see> which will determine the Local <see cref="SoundTagBase">SoundTag</see> of the <see cref="SoundEventBase">SoundEvent</see>
        /// </param>
        public void PlayAtPosition(Transform owner, Transform position, SoundTagBase localSoundTag) {
            if (SoundManagerBase.Instance == null) {
                Debug.LogWarning($"Sonity.{nameof(NameOf.SoundManager)} is null. Add one to the scene.");
            } else {
                SoundManagerBase.Instance.Internals.PlayAtPosition(this, owner, position, localSoundTag);
            }
        }

        /// <summary>
        /// Plays the <see cref="SoundEventBase">SoundEvent</see> with <see cref="SoundParameterInternals"/> at the <see cref="Transform"/> position with another <see cref="Transform"/> owner
        /// </summary>
        /// <param name='owner'>
        /// The owner <see cref="Transform"/>
        /// </param>
        /// <param name='position'>
        /// The position <see cref="Transform"/> (can follow position)
        /// </param>
        /// <param name='soundParameters'>
        /// For example <see cref="SoundParameterVolumeDecibel"/> is used to modify how the <see cref="SoundEventBase">SoundEvent</see> is played
        /// </param>
        public void PlayAtPosition(Transform owner, Transform position, params SoundParameterInternals[] soundParameters) {
            if (SoundManagerBase.Instance == null) {
                Debug.LogWarning($"Sonity.{nameof(NameOf.SoundManager)} is null. Add one to the scene.");
            } else {
                SoundManagerBase.Instance.Internals.PlayAtPosition(this, owner, position, soundParameters);
            }
        }

        /// <summary>
        /// Plays the <see cref="SoundEventBase">SoundEvent</see> with <see cref="SoundParameterInternals"/> at the <see cref="Transform"/> position with another <see cref="Transform"/> owner with the Local <see cref="SoundTagBase">SoundTag</see>
        /// </summary>
        /// <param name='owner'>
        /// The owner <see cref="Transform"/>
        /// </param>
        /// <param name='position'>
        /// The position <see cref="Transform"/> (can follow position)
        /// </param>
        /// <param name='soundParameters'>
        /// For example <see cref="SoundParameterVolumeDecibel"/> is used to modify how the <see cref="SoundEventBase">SoundEvent</see> is played
        /// </param>
        /// <param name='localSoundTag'>
        /// The <see cref="SoundTagBase">SoundTag</see> which will determine the Local <see cref="SoundTagBase">SoundTag</see> of the <see cref="SoundEventBase">SoundEvent</see>
        /// </param>
        public void PlayAtPosition(Transform owner, Transform position, SoundTagBase localSoundTag, params SoundParameterInternals[] soundParameters) {
            if (SoundManagerBase.Instance == null) {
                Debug.LogWarning($"Sonity.{nameof(NameOf.SoundManager)} is null. Add one to the scene.");
            } else {
                SoundManagerBase.Instance.Internals.PlayAtPosition(this, owner, position, localSoundTag, soundParameters);
            }
        }

        /// <summary>
        /// Plays the <see cref="SoundEventBase">SoundEvent</see> at the <see cref="Vector3"/> position
        /// </summary>
        /// <param name='owner'>
        /// The owner <see cref="Transform"/>
        /// </param>
        /// <param name='position'>
        /// The position <see cref="Vector3"/> (can't follow position)
        /// </param>
        public void PlayAtPosition(Transform owner, Vector3 position) {
            if (SoundManagerBase.Instance == null) {
                Debug.LogWarning($"Sonity.{nameof(NameOf.SoundManager)} is null. Add one to the scene.");
            } else {
                SoundManagerBase.Instance.Internals.PlayAtPosition(this, owner, position);
            }
        }

        /// <summary>
        /// Plays the <see cref="SoundEventBase">SoundEvent</see> at the <see cref="Vector3"/> position with the Local <see cref="SoundTagBase">SoundTag</see>
        /// </summary>
        /// <param name='owner'>
        /// The owner <see cref="Transform"/>
        /// </param>
        /// <param name='position'>
        /// The position <see cref="Vector3"/> (can't follow position)
        /// </param>
        /// <param name='localSoundTag'>
        /// The <see cref="SoundTagBase">SoundTag</see> which will determine the Local <see cref="SoundTagBase">SoundTag</see> of the <see cref="SoundEventBase">SoundEvent</see>
        /// </param>
        public void PlayAtPosition(Transform owner, Vector3 position, SoundTagBase localSoundTag) {
            if (SoundManagerBase.Instance == null) {
                Debug.LogWarning($"Sonity.{nameof(NameOf.SoundManager)} is null. Add one to the scene.");
            } else {
                SoundManagerBase.Instance.Internals.PlayAtPosition(this, owner, position, localSoundTag);
            }
        }

        /// <summary>
        /// Plays the <see cref="SoundEventBase">SoundEvent</see> with <see cref="SoundParameterInternals"/> at the <see cref="Vector3"/> position
        /// </summary>
        /// <param name='owner'>
        /// The owner <see cref="Transform"/>
        /// </param>
        /// <param name='position'>
        /// The position <see cref="Vector3"/> (can't follow position)
        /// </param>
        /// <param name='soundParameters'>
        /// For example <see cref="SoundParameterVolumeDecibel"/> is used to modify how the <see cref="SoundEventBase">SoundEvent</see> is played
        /// </param>
        public void PlayAtPosition(Transform owner, Vector3 position, params SoundParameterInternals[] soundParameters) {
            if (SoundManagerBase.Instance == null) {
                Debug.LogWarning($"Sonity.{nameof(NameOf.SoundManager)} is null. Add one to the scene.");
            } else {
                SoundManagerBase.Instance.Internals.PlayAtPosition(this, owner, position, soundParameters);
            }
        }

        /// <summary>
        /// Plays the <see cref="SoundEventBase">SoundEvent</see> with <see cref="SoundParameterInternals"/> at the <see cref="Vector3"/> position with the Local <see cref="SoundTagBase">SoundTag</see>
        /// </summary>
        /// <param name='owner'>
        /// The owner <see cref="Transform"/>
        /// </param>
        /// <param name='position'>
        /// The position <see cref="Vector3"/> (can't follow position)
        /// </param>
        /// <param name='soundParameters'>
        /// For example <see cref="SoundParameterVolumeDecibel"/> is used to modify how the <see cref="SoundEventBase">SoundEvent</see> is played
        /// </param>
        /// <param name='localSoundTag'>
        /// The <see cref="SoundTagBase">SoundTag</see> which will determine the Local <see cref="SoundTagBase">SoundTag</see> of the <see cref="SoundEventBase">SoundEvent</see>
        /// </param>
        public void PlayAtPosition(Transform owner, Vector3 position, SoundTagBase localSoundTag, params SoundParameterInternals[] soundParameters) {
            if (SoundManagerBase.Instance == null) {
                Debug.LogWarning($"Sonity.{nameof(NameOf.SoundManager)} is null. Add one to the scene.");
            } else {
                SoundManagerBase.Instance.Internals.PlayAtPosition(this, owner, position, localSoundTag, soundParameters);
            }
        }

        /// <summary>
        /// Stops the <see cref="SoundEventBase">SoundEvent</see> with the owner <see cref="Transform"/>
        /// </summary>
        /// <param name='owner'>
        /// The owner <see cref="Transform"/>
        /// </param>
        /// <param name='allowFadeOut'>
        /// If the <see cref="SoundEventBase">SoundEvent</see> should be allowed to fade out. Otherwise it is going to be stopped immediately
        /// </param>
        public void Stop(Transform owner, bool allowFadeOut = true) {
            if (SoundManagerBase.Instance == null) {
                Debug.LogWarning($"Sonity.{nameof(NameOf.SoundManager)} is null. Add one to the scene.");
            } else {
                SoundManagerBase.Instance.Internals.Stop(this, owner, allowFadeOut);
            }
        }

        /// <summary>
        /// Stops the <see cref="SoundEventBase">SoundEvent</see> played at the position <see cref="Transform"/>.
        /// </summary>
        /// <param name='position'>
        /// The position <see cref="Transform"/>
        /// </param>
        /// <param name='allowFadeOut'>
        /// If the <see cref="SoundEventBase">SoundEvent</see> should be allowed to fade out. Otherwise it is going to be stopped immediately
        /// </param>
        public void StopAtPosition(Transform position, bool allowFadeOut = true) {
            if (SoundManagerBase.Instance == null) {
                Debug.LogWarning($"Sonity.{nameof(NameOf.SoundManager)} is null. Add one to the scene.");
            } else {
                SoundManagerBase.Instance.Internals.StopAtPosition(this, position, allowFadeOut);
            }
        }

        /// <summary>
        /// Stops all <see cref="SoundEventBase">SoundEvents</see> with the owner <see cref="Transform"/>
        /// </summary>
        /// <param name='owner'>
        /// The owner <see cref="Transform"/>
        /// </param>
        /// <param name='allowFadeOut'>
        /// If the <see cref="SoundEventBase">SoundEvent</see> should be allowed to fade out. Otherwise it is going to be stopped immediately
        /// </param>
        public void StopAllAtOwner(Transform owner, bool allowFadeOut = true) {
            if (SoundManagerBase.Instance == null) {
                Debug.LogWarning($"Sonity.{nameof(NameOf.SoundManager)} is null. Add one to the scene.");
            } else {
                SoundManagerBase.Instance.Internals.StopAllAtOwner(owner, allowFadeOut);
            }
        }

        /// <summary>
        /// Stops the <see cref="SoundEventBase">SoundEvent</see> everywhere
        /// </summary>
        /// <param name='allowFadeOut'>
        /// If the <see cref="SoundEventBase">SoundEvent</see> should be allowed to fade out. Otherwise it is going to be stopped immediately
        /// </param>
        public void StopEverywhere(bool allowFadeOut = true) {
            if (SoundManagerBase.Instance == null) {
                Debug.LogWarning($"Sonity.{nameof(NameOf.SoundManager)} is null. Add one to the scene.");
            } else {
                SoundManagerBase.Instance.Internals.StopEverywhere(this, allowFadeOut);
            }
        }

        /// <summary>
        /// Stops the all <see cref="SoundEventBase">SoundEvents</see>
        /// </summary>
        /// <param name='allowFadeOut'>
        /// If the <see cref="SoundEventBase">SoundEvents</see> should be allowed to fade out. Otherwise they are going to be stopped immediately
        /// </param>
        public void StopEverything(bool allowFadeOut = true) {
            if (SoundManagerBase.Instance == null) {
                Debug.LogWarning($"Sonity.{nameof(NameOf.SoundManager)} is null. Add one to the scene.");
            } else {
                SoundManagerBase.Instance.Internals.StopEverything(allowFadeOut);
            }
        }

        /// <summary>
        /// Plays the <see cref="SoundEventBase">SoundEvent</see> with the 2D <see cref="Transform"/> as owner
        /// Useful to play e.g. UI or other 2D sounds without having to pass a <see cref="Transform"/>
        /// To make the sound 2D you still need to disable distance and set spatial blend to 0 in the <see cref="SoundContainerBase">SoundContainer</see>
        /// </summary>
        public void Play2D() {
            if (SoundManagerBase.Instance == null) {
                Debug.LogWarning($"Sonity.{nameof(NameOf.SoundManager)} is null. Add one to the scene.");
            } else {
                SoundManagerBase.Instance.Internals.Play2D(this);
            }
        }

        /// <summary>
        /// Plays the <see cref="SoundEventBase">SoundEvent</see> with the Local <see cref="SoundTagBase">SoundTag</see> with the 2D <see cref="Transform"/> as owner
        /// Useful to play e.g. UI or other 2D sounds without having to pass a <see cref="Transform"/>
        /// To make the sound 2D you still need to disable distance and set spatial blend to 0 in the <see cref="SoundContainerBase">SoundContainer</see>
        /// </summary>
        /// <param name='localSoundTag'>
        /// The <see cref="SoundTagBase">SoundTag</see> which will determine the Local <see cref="SoundTagBase">SoundTag</see> of the <see cref="SoundEventBase">SoundEvent</see>
        /// </param>
        public void Play2D(SoundTagBase localSoundTag) {
            if (SoundManagerBase.Instance == null) {
                Debug.LogWarning($"Sonity.{nameof(NameOf.SoundManager)} is null. Add one to the scene.");
            } else {
                SoundManagerBase.Instance.Internals.Play2D(this, localSoundTag);
            }
        }

        /// <summary>
        /// Plays the <see cref="SoundEventBase">SoundEvent</see> with <see cref="SoundParameterInternals"/> with the 2D <see cref="Transform"/> as owner
        /// Useful to play e.g. UI or other 2D sounds without having to pass a <see cref="Transform"/>
        /// To make the sound 2D you still need to disable distance and set spatial blend to 0 in the <see cref="SoundContainerBase">SoundContainer</see>
        /// </summary>
        /// <param name='soundParameters'>
        /// For example <see cref="SoundParameterVolumeDecibel"/> is used to modify how the <see cref="SoundEventBase">SoundEvent</see> is played
        /// </param>
        public void Play2D(params SoundParameterInternals[] soundParameters) {
            if (SoundManagerBase.Instance == null) {
                Debug.LogWarning($"Sonity.{nameof(NameOf.SoundManager)} is null. Add one to the scene.");
            } else {
                SoundManagerBase.Instance.Internals.Play2D(this, soundParameters);
            }
        }

        /// <summary>
        /// Plays the <see cref="SoundEventBase">SoundEvent</see> with <see cref="SoundParameterInternals"/> with the Local <see cref="SoundTagBase">SoundTag</see> with the 2D <see cref="Transform"/> as owner
        /// Useful to play e.g. UI or other 2D sounds without having to pass a <see cref="Transform"/>
        /// To make the sound 2D you still need to disable distance and set spatial blend to 0 in the <see cref="SoundContainerBase">SoundContainer</see>
        /// </summary>
        /// <param name='localSoundTag'>
        /// The <see cref="SoundTagBase">SoundTag</see> which will determine the Local <see cref="SoundTagBase">SoundTag</see> of the <see cref="SoundEventBase">SoundEvent</see>
        /// </param>
        /// <param name='soundParameters'>
        /// For example <see cref="SoundParameterVolumeDecibel"/> is used to modify how the <see cref="SoundEventBase">SoundEvent</see> is played
        /// </param>
        public void Play2D(SoundTagBase localSoundTag, params SoundParameterInternals[] soundParameters) {
            if (SoundManagerBase.Instance == null) {
                Debug.LogWarning($"Sonity.{nameof(NameOf.SoundManager)} is null. Add one to the scene.");
            } else {
                SoundManagerBase.Instance.Internals.Play2D(this, localSoundTag, soundParameters);
            }
        }

        /// <summary>
        /// Stops the <see cref="SoundEventBase">SoundEvent</see> at the 2D <see cref="Transform"/>
        /// </summary>
        /// <param name='allowFadeOut'>
        /// If the <see cref="SoundEventBase">SoundEvent</see> should be allowed to fade out. Otherwise it is going to be stopped immediately
        /// </param>
        public void Stop2D(bool allowFadeOut = true) {
            if (SoundManagerBase.Instance == null) {
                Debug.LogWarning($"Sonity.{nameof(NameOf.SoundManager)} is null. Add one to the scene.");
            } else {
                SoundManagerBase.Instance.Internals.Stop2D(this, allowFadeOut);
            }
        }

        /// <summary>
        /// Stops all <see cref="SoundEventBase">SoundEvents</see> at the 2D <see cref="Transform"/>
        /// </summary>
        /// <param name='allowFadeOut'>
        /// If the <see cref="SoundEventBase">SoundEvent</see> should be allowed to fade out. Otherwise it is going to be stopped immediately
        /// </param>
        public void StopAllAt2D(bool allowFadeOut = true) {
            if (SoundManagerBase.Instance == null) {
                Debug.LogWarning($"Sonity.{nameof(NameOf.SoundManager)} is null. Add one to the scene.");
            } else {
                SoundManagerBase.Instance.Internals.StopAllAt2D(allowFadeOut);
            }
        }

        /// <summary>
        /// Plays the <see cref="SoundEventBase">SoundEvent</see> at the music <see cref="Transform"/>
        /// </summary>
        /// <param name="stopAllOtherMusic">
        /// If all other <see cref="SoundEventBase">SoundEvents</see> played at the <see cref="SoundManagerBase">SoundManagers</see> music <see cref="Transform"/> should be stopped
        /// </param>
        /// <param name="allowFadeOut">
        /// If the other stopped <see cref="SoundEventBase">SoundEvent</see> should be allowed to fade out. Otherwise they are going to be stopped immediately
        /// </param>
        public void PlayMusic(bool stopAllOtherMusic = true, bool allowFadeOut = true) {
            if (SoundManagerBase.Instance == null) {
                Debug.LogWarning($"Sonity.{nameof(NameOf.SoundManager)} is null. Add one to the scene.");
            } else {
                SoundManagerBase.Instance.Internals.PlayMusic(this, stopAllOtherMusic, allowFadeOut);
            }
        }

        /// <summary>
        /// Plays the <see cref="SoundEventBase">SoundEvent</see> at the music <see cref="Transform"/>
        /// </summary>
        /// <param name="stopAllOtherMusic">
        /// If all other <see cref="SoundEventBase">SoundEvents</see> played at the <see cref="SoundManagerBase">SoundManagers</see> music <see cref="Transform"/> should be stopped
        /// </param>
        /// <param name="allowFadeOut">
        /// If the other stopped <see cref="SoundEventBase">SoundEvent</see> should be allowed to fade out. Otherwise they are going to be stopped immediately
        /// </param>
        /// <param name="soundParameters">
        /// For example <see cref="SoundParameterVolumeDecibel"/> is used to modify how the <see cref="SoundEventBase">SoundEvent</see> is played
        /// </param>
        public void PlayMusic(bool stopAllOtherMusic = true, bool allowFadeOut = true, params SoundParameterInternals[] soundParameters) {
            if (SoundManagerBase.Instance == null) {
                Debug.LogWarning($"Sonity.{nameof(NameOf.SoundManager)} is null. Add one to the scene.");
            } else {
                SoundManagerBase.Instance.Internals.PlayMusic(this, stopAllOtherMusic, allowFadeOut, soundParameters);
            }
        }

        /// <summary>
        /// Stops the <see cref="SoundEventBase">SoundEvent</see> played with <see cref="PlayMusic()"/>
        /// </summary>
        /// <param name='allowFadeOut'>
        /// If the other stopped <see cref="SoundEventBase">SoundEvent</see> should be allowed to fade out. Otherwise they are going to be stopped immediately
        /// </param>
        public void StopMusic(bool allowFadeOut = true) {
            if (SoundManagerBase.Instance == null) {
                Debug.LogWarning($"Sonity.{nameof(NameOf.SoundManager)} is null. Add one to the scene.");
            } else {
                SoundManagerBase.Instance.Internals.StopMusic(this, allowFadeOut);
            }
        }

        /// <summary>
        /// Stops the all <see cref="SoundEventBase">SoundEvents</see> played with MusicPlay
        /// </summary>
        /// <param name='allowFadeOut'>
        /// If the <see cref="SoundEventBase">SoundEvent</see> should be allowed to fade out. Otherwise it is going to be stopped immediately
        /// </param>
        public void StopAllMusic(bool allowFadeOut = true) {
            if (SoundManagerBase.Instance == null) {
                Debug.LogWarning($"Sonity.{nameof(NameOf.SoundManager)} is null. Add one to the scene.");
            } else {
                SoundManagerBase.Instance.Internals.StopAllMusic(allowFadeOut);
            }
        }

        /// <summary>
        /// <para> If playing it returns <see cref="SoundEventState.Playing"/> </para> 
        /// <para> If not playing, but it is delayed it returns <see cref="SoundEventState.Delayed"/> </para> 
        /// <para> If not playing and it is not delayed it returns <see cref="SoundEventState.NotPlaying"/> </para> 
        /// <para> If the <see cref="SoundEventBase">SoundEvent</see> or <see cref="Transform"/> is null it returns <see cref="SoundEventState.NotPlaying"/> </para> 
        /// </summary>
        /// <param name="owner"> The owner <see cref="Transform"/> </param>
        /// <returns> Returns <see cref="SoundEventState"/> of the <see cref="SoundEventBase">SoundEvents</see> <see cref="SoundEventInstance"/> </returns>
        public SoundEventState GetSoundEventState(Transform owner) {
            if (SoundManagerBase.Instance == null) {
                Debug.LogWarning($"Sonity.{nameof(NameOf.SoundManager)} is null. Add one to the scene.");
            } else {
                return SoundManagerBase.Instance.Internals.GetSoundEventState(this, owner);
            }
            return SoundEventState.NotPlaying;
        }

        /// <summary>
        /// <para> Returns the length (in seconds) of the <see cref="AudioClip"/> in the last played <see cref="AudioSource"/> </para>
        /// <para> Returns 0 if the <see cref="SoundEventInstance"/> is not playing </para>
        /// <para> Returns 0 if the <see cref="SoundEventBase">SoundEvent</see> or <see cref="Transform"/> is null </para>
        /// <para> If it should be scaled by pitch. E.g. -12 semitones will be twice as long </para>
        /// </summary>
        /// <param name="owner"> The owner <see cref="Transform"/> </param>
        /// <param name="pitchSpeed"> If it should be scaled by pitch. E.g. -12 semitones will be twice as long </param>
        /// <returns> Length in seconds </returns>
        public float GetLastPlayedClipLength(Transform owner, bool pitchSpeed) {
            if (SoundManagerBase.Instance == null) {
                Debug.LogWarning($"Sonity.{nameof(NameOf.SoundManager)} is null. Add one to the scene.");
            } else {
                return SoundManagerBase.Instance.Internals.GetLastPlayedClipLength(this, owner, pitchSpeed);
            }
            return 0f;
        }

        /// <summary>
        /// <para> Returns the current time (in seconds) of the <see cref="AudioClip"/> in the last played <see cref="AudioSource"/> </para>
        /// <para> Returns 0 if the <see cref="SoundEventInstance"/> is not playing </para>
        /// <para> Returns 0 if the <see cref="SoundEventBase">SoundEvent</see> or <see cref="Transform"/> is null </para>
        /// <para> If it should be scaled by pitch. E.g. -12 semitones will be twice as long </para>
        /// </summary>
        /// <param name="owner"> The owner <see cref="Transform"/> </param>
        /// <param name="pitchSpeed"> If it should be scaled by pitch. E.g. -12 semitones will be twice as long </param>
        /// <returns> Time in seconds </returns>
        public float GetLastPlayedClipTime(Transform owner, bool pitchSpeed) {
            if (SoundManagerBase.Instance == null) {
                Debug.LogWarning($"Sonity.{nameof(NameOf.SoundManager)} is null. Add one to the scene.");
            } else {
                return SoundManagerBase.Instance.Internals.GetLastPlayedClipTime(this, owner, pitchSpeed);
            }
            return 0f;
        }

        /// <summary>
        /// <para> Provides a block of spectrum data from <see cref="AudioSource"/>s </para>
        /// </summary>
        /// <param name="soundEvent"> The <see cref="SoundEventBase">SoundEvent</see> to get the spectrum data from </param>
        /// <param name="owner"> The owner <see cref="Transform"/> </param>
        /// <param name="samples"> The array to populate with audio samples. Its length must be a power of 2 </param>
        /// <param name="channel"> The channel to sample from </param>
        /// <param name="window"> The <see cref="FFTWindow"/> type to use when sampling </param>
        /// <param name="spectrumDataFrom"> Where to get the spectrum data from </param>
        public void GetSpectrumData(SoundEvent soundEvent, Transform owner, ref float[] samples, int channel, FFTWindow window, SpectrumDataFrom spectrumDataFrom) {
            if (SoundManagerBase.Instance == null) {
                Debug.LogWarning($"Sonity.{nameof(NameOf.SoundManager)} is null. Add one to the scene.");
            } else {
                SoundManagerBase.Instance.Internals.GetSpectrumData(soundEvent, owner, ref samples, channel, window, spectrumDataFrom);
            }
        }

        /// <summary>
        /// <para> Returns the max length (in seconds) of the <see cref="SoundEventBase">SoundEvent</see> (calculated from the longest audioClip) </para>
        /// <para> Is scaled by the pitch of the <see cref="SoundEventBase">SoundEvent</see> and <see cref="SoundContainerBase">SoundContainer</see> </para>
        /// <para> Does not take into account random, intensity or parameter pitch </para>
        /// </summary>
        /// <returns> The max length in seconds </returns>
        public float GetMaxLength() {
            if (SoundManagerBase.Instance == null) {
                Debug.LogWarning($"Sonity.{nameof(NameOf.SoundManager)} is null. Add one to the scene.");
            } else {
                return SoundManagerBase.Instance.Internals.InternalGetMaxLength(this);
            }
            return 0f;
        }

        /// <summary>
        /// <para> Returns the time (in seconds) since the <see cref="SoundEventBase">SoundEvent</see> was played </para>
        /// <para> Calculated using <see cref="Time.realtimeSinceStartup"/> </para>
        /// <para> Returns 0 if the <see cref="SoundEventInstance"/> is not playing </para>
        /// <para> Returns 0 if the <see cref="SoundEventBase">SoundEvent</see> or <see cref="Transform"/> is null </para>
        /// </summary>
        /// <param name="owner"> The owner <see cref="Transform"/> </param>
        /// <returns> Time in seconds </returns>
        public float GetTimePlayed(Transform owner) {
            if (SoundManagerBase.Instance == null) {
                Debug.LogWarning($"Sonity.{nameof(NameOf.SoundManager)} is null. Add one to the scene.");
            } else {
                return SoundManagerBase.Instance.Internals.GetTimePlayed(this, owner);
            }
            return 0f;
        }

        /// <summary>
        /// Loads the audio data of any <see cref="AudioClip"/>s assigned to the <see cref="SoundContainerBase">SoundContainers</see> of this <see cref="SoundEventBase">SoundEvent</see>
        /// </summary>
        new public void LoadAudioData() {
            LoadUnloadAudioData(true);
        }

        /// <summary>
        /// Unloads the audio data of any <see cref="AudioClip"/>s assigned to the <see cref="SoundContainerBase">SoundContainers</see> of this <see cref="SoundEventBase">SoundEvent</see>
        /// </summary>
        new public void UnloadAudioData() {
            LoadUnloadAudioData(false);
        }

        private void LoadUnloadAudioData(bool load) {
            if (internals.soundContainers.Length == 0) {
                if (ShouldDebug.Warnings()) {
                    Debug.LogWarning($"{nameof(NameOf.SoundEvent)} " + name + $" has no {nameof(NameOf.SoundContainer)}.", this);
                }
            } else {
                for (int i = 0; i < internals.soundContainers.Length; i++) {
                    if (internals.soundContainers[i] == null) {
                        if (ShouldDebug.Warnings()) {
                            Debug.LogWarning($"{nameof(NameOf.SoundEvent)} " + name + $" {nameof(NameOf.SoundContainer)} " + i + " is null.", this);
                        }
                    } else {
                        internals.soundContainers[i].internals.LoadUnloadAudioData(load, internals.soundContainers[i]);
                    }
                }
            }
        }
    }
}