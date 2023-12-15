// Created by Victor Engström
// Copyright 2023 Sonigon AB
// http://www.sonity.org/

using UnityEngine;
using System;
using Sonity.Internal;

namespace Sonity {

    /// <summary>
    /// The <see cref="SoundManagerBase">SoundManager</see> is the master object which is used to play sounds and manage global settings.
    /// An instance of this object is required in the scene in order to play <see cref="SoundEventBase">SoundEvents</see>.
    /// You can add the pre-made prefab called “SoundManager” found in “Assets\Plugins\Sonity\Prefabs” to your scene.
    /// Or you can add the “Sonity - Sound Manager” component to an empty <see cref="GameObject"/> in the scene, it works just as well.
    /// </summary>
    [Serializable]
    [AddComponentMenu("Sonity/Sonity - Sound Manager")]
    public class SoundManager : SoundManagerBase {

        /// <summary>
        /// The static instance of the <see cref="SoundManagerBase">SoundManager</see>
        /// </summary>
        new public static SoundManager Instance { get { return (SoundManager)SoundManagerBase.Instance; } }

        /// <summary>
        /// Plays the <see cref="SoundEventBase">SoundEvent</see> at the <see cref="Transform"/> position
        /// </summary>
        /// <param name='soundEvent'>
        /// The <see cref="SoundEventBase">SoundEvent</see> to play
        /// </param>
        /// <param name='owner'>
        /// The owner <see cref="Transform"/> (can follow positon)
        /// </param>
        public void Play(SoundEvent soundEvent, Transform owner) {
            if (owner == null) {
                if (ShouldDebug.Warnings()) {
                    Debug.LogWarning($"Sonity.{nameof(NameOf.SoundManager)}: The {nameof(Transform)} is null." + Internals.DebugInfoString(soundEvent, owner, null, null), soundEvent);
                }
            } else {
                if (soundEvent == null) {
                    if (ShouldDebug.Warnings()) {
                        Debug.LogWarning($"Sonity.{nameof(NameOf.SoundManager)}: The {nameof(NameOf.SoundEvent)} is null." + Internals.DebugInfoString(soundEvent, owner, null, null), owner);
                    }
                } else {
                    Internals.InternalPlay(soundEvent, SoundEventPlayType.Play, owner, null, null, null, null, null, null, null);
                }
            }
        }

        /// <summary>
        /// Plays the <see cref="SoundEventBase">SoundEvent</see> at the <see cref="Transform"/> position with the Local <see cref="SoundTagBase">SoundTag</see>
        /// </summary>
        /// <param name='soundEvent'>
        /// The <see cref="SoundEventBase">SoundEvent</see> to play
        /// </param>
        /// <param name='owner'>
        /// The owner <see cref="Transform"/> (can follow positon)
        /// </param>
        /// <param name='localSoundTag'>
        /// The <see cref="SoundTagBase">SoundTag</see> which will determine the Local <see cref="SoundTagBase">SoundTag</see> of the <see cref="SoundEventBase">SoundEvent</see>
        /// </param>
        public void Play(SoundEvent soundEvent, Transform owner, SoundTag localSoundTag) {
            if (owner == null) {
                if (ShouldDebug.Warnings()) {
                    Debug.LogWarning($"Sonity.{nameof(NameOf.SoundManager)}: The {nameof(Transform)} is null." + Internals.DebugInfoString(soundEvent, owner, null, null), soundEvent);
                }
            } else {
                if (soundEvent == null) {
                    if (ShouldDebug.Warnings()) {
                        Debug.LogWarning($"Sonity.{nameof(NameOf.SoundManager)}: The {nameof(NameOf.SoundEvent)} is null." + Internals.DebugInfoString(soundEvent, owner, null, null), owner);
                    }
                } else {
                    Internals.InternalPlay(soundEvent, SoundEventPlayType.Play, owner, null, null, null, null, null, null, localSoundTag);
                }
            }
        }

        /// <summary>
        /// Plays the <see cref="SoundEventBase">SoundEvent</see> with <see cref="SoundParameterInternals"/> at the <see cref="Transform"/> position
        /// </summary>
        /// <param name='soundEvent'>
        /// The <see cref="SoundEventBase">SoundEvent</see> to play
        /// </param>
        /// <param name='owner'>
        /// The owner <see cref="Transform"/> (can follow positon)
        /// </param>
        /// <param name='soundParameters'>
        /// For example <see cref="SoundParameterVolumeDecibel"/> is used to modify how the <see cref="SoundEventBase">SoundEvent</see> is played
        /// </param>
        public void Play(SoundEvent soundEvent, Transform owner, params SoundParameterInternals[] soundParameters) {
            if (owner == null) {
                if (ShouldDebug.Warnings()) {
                    Debug.LogWarning($"Sonity.{nameof(NameOf.SoundManager)}: The {nameof(Transform)} is null." + Internals.DebugInfoString(soundEvent, owner, null, null), soundEvent);
                }
            } else {
                if (soundEvent == null) {
                    if (ShouldDebug.Warnings()) {
                        Debug.LogWarning($"Sonity.{nameof(NameOf.SoundManager)}: The {nameof(NameOf.SoundEvent)} is null." + Internals.DebugInfoString(soundEvent, owner, null, null), owner);
                    }
                } else {
                    Internals.InternalPlay(soundEvent, SoundEventPlayType.Play, owner, null, null, null, null, soundParameters, null, null);
                }
            }
        }

        /// <summary>
        /// Plays the <see cref="SoundEventBase">SoundEvent</see> with <see cref="SoundParameterInternals"/> at the <see cref="Transform"/> position with the Local <see cref="SoundTagBase">SoundTag</see>
        /// </summary>
        /// <param name='soundEvent'>
        /// The <see cref="SoundEventBase">SoundEvent</see> to play
        /// </param>
        /// <param name='owner'>
        /// The owner <see cref="Transform"/> (can follow positon)
        /// </param>
        /// <param name='localSoundTag'>
        /// The <see cref="SoundTagBase">SoundTag</see> which will determine the Local <see cref="SoundTagBase">SoundTag</see> of the <see cref="SoundEventBase">SoundEvent</see>
        /// </param>
        /// <param name='soundParameters'>
        /// For example <see cref="SoundParameterVolumeDecibel"/> is used to modify how the <see cref="SoundEventBase">SoundEvent</see> is played
        /// </param>
        public void Play(SoundEvent soundEvent, Transform owner, SoundTag localSoundTag, params SoundParameterInternals[] soundParameters) {
            if (owner == null) {
                if (ShouldDebug.Warnings()) {
                    Debug.LogWarning($"Sonity.{nameof(NameOf.SoundManager)}: The {nameof(Transform)} is null." + Internals.DebugInfoString(soundEvent, owner, null, null), soundEvent);
                }
            } else {
                if (soundEvent == null) {
                    if (ShouldDebug.Warnings()) {
                        Debug.LogWarning($"Sonity.{nameof(NameOf.SoundManager)}: The {nameof(NameOf.SoundEvent)} is null." + Internals.DebugInfoString(soundEvent, owner, null, null), owner);
                    }
                } else {
                    Internals.InternalPlay(soundEvent, SoundEventPlayType.Play, owner, null, null, null, null, soundParameters, null, localSoundTag);
                }
            }
        }

        /// <summary>
        /// Plays the <see cref="SoundEventBase">SoundEvent</see> at the <see cref="Transform"/> position with another <see cref="Transform"/> owner
        /// </summary>
        /// <param name='soundEvent'>
        /// The <see cref="SoundEventBase">SoundEvent</see> to play
        /// </param>
        /// <param name='owner'>
        /// The owner <see cref="Transform"/>
        /// </param>
        /// <param name='position'>
        /// The position <see cref="Transform"/> (can follow position)
        /// </param>
        public void PlayAtPosition(SoundEvent soundEvent, Transform owner, Transform position) {
            if (position == null) {
                if (ShouldDebug.Warnings()) {
                    Debug.LogWarning($"Sonity.{nameof(NameOf.SoundManager)}: The position {nameof(Transform)} is null." + Internals.DebugInfoString(soundEvent, null, position, owner), owner);
                }
            } else {
                if (owner == null) {
                    if (ShouldDebug.Warnings()) {
                        Debug.LogWarning($"Sonity.{nameof(NameOf.SoundManager)}: The owner {nameof(Transform)} is null." + Internals.DebugInfoString(soundEvent, null, position, owner), soundEvent);
                    }
                } else {
                    if (soundEvent == null) {
                        if (ShouldDebug.Warnings()) {
                            Debug.LogWarning($"Sonity.{nameof(NameOf.SoundManager)}: The {nameof(NameOf.SoundEvent)} is null." + Internals.DebugInfoString(soundEvent, null, position, owner), owner);
                        }
                    } else {
                        Internals.InternalPlay(soundEvent, SoundEventPlayType.PlayAtTransform, owner, null, position, null, null, null, null, null);
                    }
                }
            }
        }

        /// <summary>
        /// Plays the <see cref="SoundEventBase">SoundEvent</see> at the <see cref="Transform"/> position with another <see cref="Transform"/> owner with the Local <see cref="SoundTagBase">SoundTag</see>
        /// </summary>
        /// <param name='soundEvent'>
        /// The <see cref="SoundEventBase">SoundEvent</see> to play
        /// </param>
        /// <param name='owner'>
        /// The owner <see cref="Transform"/>
        /// </param>
        /// <param name='position'>
        /// The position <see cref="Transform"/> (can follow position)
        /// </param>
        /// <param name='localSoundTag'>
        /// The <see cref="SoundTagBase">SoundTag</see> which will determine the Local <see cref="SoundTagBase">SoundTag</see> of the <see cref="SoundEventBase">SoundEvent</see>
        /// </param>
        public void PlayAtPosition(SoundEvent soundEvent, Transform owner, Transform position, SoundTag localSoundTag) {
            if (position == null) {
                if (ShouldDebug.Warnings()) {
                    Debug.LogWarning($"Sonity.{nameof(NameOf.SoundManager)}: The position {nameof(Transform)} is null." + Internals.DebugInfoString(soundEvent, null, position, owner), owner);
                }
            } else {
                if (owner == null) {
                    if (ShouldDebug.Warnings()) {
                        Debug.LogWarning($"Sonity.{nameof(NameOf.SoundManager)}: The owner {nameof(Transform)} is null." + Internals.DebugInfoString(soundEvent, null, position, owner), soundEvent);
                    }
                } else {
                    if (soundEvent == null) {
                        if (ShouldDebug.Warnings()) {
                            Debug.LogWarning($"Sonity.{nameof(NameOf.SoundManager)}: The {nameof(NameOf.SoundEvent)} is null." + Internals.DebugInfoString(soundEvent, null, position, owner), owner);
                        }
                    } else {
                        Internals.InternalPlay(soundEvent, SoundEventPlayType.PlayAtTransform, owner, null, position, null, null, null, null, localSoundTag);
                    }
                }
            }
        }

        /// <summary>
        /// Plays the <see cref="SoundEventBase">SoundEvent</see> with <see cref="SoundParameterInternals"/> at the <see cref="Transform"/> position with another <see cref="Transform"/> owner
        /// </summary>
        /// <param name='soundEvent'>
        /// The <see cref="SoundEventBase">SoundEvent</see> to play
        /// </param>
        /// <param name='owner'>
        /// The owner <see cref="Transform"/>
        /// </param>
        /// <param name='position'>
        /// The position <see cref="Transform"/> (can follow position)
        /// </param>
        /// <param name='soundParameters'>
        /// For example <see cref="SoundParameterVolumeDecibel"/> is used to modify how the <see cref="SoundEventBase">SoundEvent</see> is played
        /// </param>
        public void PlayAtPosition(SoundEvent soundEvent, Transform owner, Transform position, params SoundParameterInternals[] soundParameters) {
            if (position == null) {
                if (ShouldDebug.Warnings()) {
                    Debug.LogWarning($"Sonity.{nameof(NameOf.SoundManager)}: The position {nameof(Transform)} is null." + Internals.DebugInfoString(soundEvent, null, position, owner), owner);
                }
            } else {
                if (owner == null) {
                    if (ShouldDebug.Warnings()) {
                        Debug.LogWarning($"Sonity.{nameof(NameOf.SoundManager)}: The owner {nameof(Transform)} is null." + Internals.DebugInfoString(soundEvent, null, position, owner), soundEvent);
                    }
                } else {
                    if (soundEvent == null) {
                        if (ShouldDebug.Warnings()) {
                            Debug.LogWarning($"Sonity.{nameof(NameOf.SoundManager)}: The {nameof(NameOf.SoundEvent)} is null." + Internals.DebugInfoString(soundEvent, null, position, owner), owner);
                        }
                    } else {
                        Internals.InternalPlay(soundEvent, SoundEventPlayType.PlayAtTransform, owner, null, position, null, null, soundParameters, null, null);
                    }
                }
            }
        }

        /// <summary>
        /// Plays the <see cref="SoundEventBase">SoundEvent</see> with <see cref="SoundParameterInternals"/> at the <see cref="Transform"/> position with another <see cref="Transform"/> owner with the Local <see cref="SoundTagBase">SoundTag</see>
        /// </summary>
        /// <param name='soundEvent'>
        /// The <see cref="SoundEventBase">SoundEvent</see> to play
        /// </param>
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
        public void PlayAtPosition(SoundEvent soundEvent, Transform owner, Transform position, SoundTag localSoundTag, params SoundParameterInternals[] soundParameters) {
            if (position == null) {
                if (ShouldDebug.Warnings()) {
                    Debug.LogWarning($"Sonity.{nameof(NameOf.SoundManager)}: The position {nameof(Transform)} is null." + Internals.DebugInfoString(soundEvent, null, position, owner), owner);
                }
            } else {
                if (owner == null) {
                    if (ShouldDebug.Warnings()) {
                        Debug.LogWarning($"Sonity.{nameof(NameOf.SoundManager)}: The owner {nameof(Transform)} is null." + Internals.DebugInfoString(soundEvent, null, position, owner), soundEvent);
                    }
                } else {
                    if (soundEvent == null) {
                        if (ShouldDebug.Warnings()) {
                            Debug.LogWarning($"Sonity.{nameof(NameOf.SoundManager)}: The {nameof(NameOf.SoundEvent)} is null." + Internals.DebugInfoString(soundEvent, null, position, owner), owner);
                        }
                    } else {
                        Internals.InternalPlay(soundEvent, SoundEventPlayType.PlayAtTransform, owner, null, position, null, null, soundParameters, null, localSoundTag);
                    }
                }
            }
        }

        /// <summary>
        /// Plays the <see cref="SoundEventBase">SoundEvent</see> at the <see cref="Vector3"/> position
        /// </summary>
        /// <param name='soundEvent'>
        /// The <see cref="SoundEventBase">SoundEvent</see> to play
        /// </param>
        /// <param name='owner'>
        /// The owner <see cref="Transform"/>
        /// </param>
        /// <param name='position'>
        /// The position <see cref="Vector3"/> (can't follow position)
        /// </param>
        public void PlayAtPosition(SoundEvent soundEvent, Transform owner, Vector3 position) {
            if (owner == null) {
                if (ShouldDebug.Warnings()) {
                    Debug.LogWarning($"Sonity.{nameof(NameOf.SoundManager)}: The {nameof(Transform)} is null." + Internals.DebugInfoString(soundEvent, null, null, owner), soundEvent);
                }
            } else {
                if (soundEvent == null) {
                    if (ShouldDebug.Warnings()) {
                        Debug.LogWarning($"Sonity.{nameof(NameOf.SoundManager)}: The {nameof(NameOf.SoundEvent)} is null." + Internals.DebugInfoString(soundEvent, null, null, owner), owner);
                    }
                } else {
                    Internals.InternalPlay(soundEvent, SoundEventPlayType.PlayAtVector, owner, position, null, null, null, null, null, null);
                }
            }
        }

        /// <summary>
        /// Plays the <see cref="SoundEventBase">SoundEvent</see> at the <see cref="Vector3"/> position with the Local <see cref="SoundTagBase">SoundTag</see>
        /// </summary>
        /// <param name='soundEvent'>
        /// The <see cref="SoundEventBase">SoundEvent</see> to play
        /// </param>
        /// <param name='owner'>
        /// The owner <see cref="Transform"/>
        /// </param>
        /// <param name='position'>
        /// The position <see cref="Vector3"/> (can't follow position)
        /// </param>
        /// <param name='localSoundTag'>
        /// The <see cref="SoundTagBase">SoundTag</see> which will determine the Local <see cref="SoundTagBase">SoundTag</see> of the <see cref="SoundEventBase">SoundEvent</see>
        /// </param>
        public void PlayAtPosition(SoundEvent soundEvent, Transform owner, Vector3 position, SoundTag localSoundTag) {
            if (owner == null) {
                if (ShouldDebug.Warnings()) {
                    Debug.LogWarning($"Sonity.{nameof(NameOf.SoundManager)}: The {nameof(Transform)} is null." + Internals.DebugInfoString(soundEvent, null, null, owner), soundEvent);
                }
            } else {
                if (soundEvent == null) {
                    if (ShouldDebug.Warnings()) {
                        Debug.LogWarning($"Sonity.{nameof(NameOf.SoundManager)}: The {nameof(NameOf.SoundEvent)} is null." + Internals.DebugInfoString(soundEvent, null, null, owner), owner);
                    }
                } else {
                    Internals.InternalPlay(soundEvent, SoundEventPlayType.PlayAtVector, owner, position, null, null, null, null, null, localSoundTag);
                }
            }
        }

        /// <summary>
        /// Plays the <see cref="SoundEventBase">SoundEvent</see> with <see cref="SoundParameterInternals"/> at the <see cref="Vector3"/> position
        /// </summary>
        /// <param name='soundEvent'>
        /// The <see cref="SoundEventBase">SoundEvent</see> to play
        /// </param>
        /// <param name='owner'>
        /// The owner <see cref="Transform"/>
        /// </param>
        /// <param name='position'>
        /// The position <see cref="Vector3"/> (can't follow position)
        /// </param>
        /// <param name='soundParameters'>
        /// For example <see cref="SoundParameterVolumeDecibel"/> is used to modify how the <see cref="SoundEventBase">SoundEvent</see> is played
        /// </param>
        public void PlayAtPosition(SoundEvent soundEvent, Transform owner, Vector3 position, params SoundParameterInternals[] soundParameters) {
            if (owner == null) {
                if (ShouldDebug.Warnings()) {
                    Debug.LogWarning($"Sonity.{nameof(NameOf.SoundManager)}: The {nameof(Transform)} is null." + Internals.DebugInfoString(soundEvent, null, null, owner), soundEvent);
                }
            } else {
                if (soundEvent == null) {
                    if (ShouldDebug.Warnings()) {
                        Debug.LogWarning($"Sonity.{nameof(NameOf.SoundManager)}: The {nameof(NameOf.SoundEvent)} is null." + Internals.DebugInfoString(soundEvent, null, null, owner), owner);
                    }
                } else {
                    Internals.InternalPlay(soundEvent, SoundEventPlayType.PlayAtVector, owner, position, null, null, null, soundParameters, null, null);
                }
            }
        }

        /// <summary>
        /// Plays the <see cref="SoundEventBase">SoundEvent</see> with <see cref="SoundParameterInternals"/> at the <see cref="Vector3"/> position with the Local <see cref="SoundTagBase">SoundTag</see>
        /// </summary>
        /// <param name='soundEvent'>
        /// The <see cref="SoundEventBase">SoundEvent</see> to play
        /// </param>
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
        public void PlayAtPosition(SoundEvent soundEvent, Transform owner, Vector3 position, SoundTag localSoundTag, params SoundParameterInternals[] soundParameters) {
            if (owner == null) {
                if (ShouldDebug.Warnings()) {
                    Debug.LogWarning($"Sonity.{nameof(NameOf.SoundManager)}: The {nameof(Transform)} is null" + Internals.DebugInfoString(soundEvent, null, null, owner), soundEvent);
                }
            } else {
                if (soundEvent == null) {
                    if (ShouldDebug.Warnings()) {
                        Debug.LogWarning($"Sonity.{nameof(NameOf.SoundManager)}: The {nameof(NameOf.SoundEvent)} is null" + Internals.DebugInfoString(soundEvent, null, null, owner), owner);
                    }
                } else {
                    Internals.InternalPlay(soundEvent, SoundEventPlayType.PlayAtVector, owner, position, null, null, null, soundParameters, null, localSoundTag);
                }
            }
        }

        /// <summary>
        /// Stops the <see cref="SoundEventBase">SoundEvent</see> with the owner <see cref="Transform"/>
        /// </summary>
        /// <param name='soundEvent'>
        /// The <see cref="SoundEventBase">SoundEvent</see> to stop
        /// </param>
        /// <param name='owner'>
        /// The owner <see cref="Transform"/>
        /// </param>
        /// <param name='allowFadeOut'>
        /// If the <see cref="SoundEventBase">SoundEvent</see> should be allowed to fade out. Otherwise it is going to be stopped immediately
        /// </param>
        public void Stop(SoundEvent soundEvent, Transform owner, bool allowFadeOut = true) {
            if (owner == null) {
                if (ShouldDebug.Warnings()) {
                    Debug.LogWarning($"Sonity.{nameof(NameOf.SoundManager)}: The {nameof(Transform)} is null" + Internals.DebugInfoString(soundEvent, owner, null, null), soundEvent);
                }
            } else {
                if (soundEvent == null) {
                    if (ShouldDebug.Warnings()) {
                        Debug.LogWarning($"Sonity.{nameof(NameOf.SoundManager)}: The {nameof(NameOf.SoundEvent)} is null" + Internals.DebugInfoString(soundEvent, owner, null, null), owner);
                    }
                } else {
                    Internals.InternalStop(soundEvent, owner, allowFadeOut);
                }
            }
        }

        /// <summary>
        /// Stops the <see cref="SoundEventBase">SoundEvent</see> played at the position <see cref="Transform"/>.
        /// </summary>
        /// <param name='soundEvent'>
        /// The <see cref="SoundEventBase">SoundEvent</see> to stop
        /// </param>
        /// <param name='position'>
        /// The position <see cref="Transform"/>
        /// </param>
        /// <param name='allowFadeOut'>
        /// If the <see cref="SoundEventBase">SoundEvent</see> should be allowed to fade out. Otherwise it is going to be stopped immediately
        /// </param>
        public void StopAtPosition(SoundEvent soundEvent, Transform position, bool allowFadeOut = true) {
            if (position == null) {
                if (ShouldDebug.Warnings()) {
                    Debug.LogWarning($"Sonity.{nameof(NameOf.SoundManager)}: The {nameof(Transform)} is null" + Internals.DebugInfoString(soundEvent, position, null, null), soundEvent);
                }
            } else {
                if (soundEvent == null) {
                    if (ShouldDebug.Warnings()) {
                        Debug.LogWarning($"Sonity.{nameof(NameOf.SoundManager)}: The {nameof(NameOf.SoundEvent)} is null" + Internals.DebugInfoString(soundEvent, position, null, null), position);
                    }
                } else {
                    Internals.InternalStopAtPosition(soundEvent, position, allowFadeOut);
                }
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
            if (owner == null) {
                if (ShouldDebug.Warnings()) {
                    Debug.LogWarning($"Sonity.{nameof(NameOf.SoundManager)}: The {nameof(Transform)} is null" + Internals.DebugInfoString(null, owner, null, null));
                }
            } else {
                Internals.InternalStopAllAtOwner(owner, allowFadeOut);
            }
        }

        /// <summary>
        /// Stops the <see cref="SoundEventBase">SoundEvent</see> everywhere
        /// </summary>
        /// <param name='soundEvent'>
        /// The <see cref="SoundEventBase">SoundEvent</see> which to stop
        /// </param>
        /// <param name='allowFadeOut'>
        /// If the <see cref="SoundEventBase">SoundEvent</see> should be allowed to fade out. Otherwise it is going to be stopped immediately
        /// </param>
        public void StopEverywhere(SoundEvent soundEvent, bool allowFadeOut = true) {
            if (soundEvent == null) {
                if (ShouldDebug.Warnings()) {
                    Debug.LogWarning($"Sonity.{nameof(NameOf.SoundManager)}: The {nameof(NameOf.SoundEvent)} is null" + Internals.DebugInfoString(soundEvent, null, null, null));
                }
            } else {
                Internals.InternalStopEverywhere(soundEvent, allowFadeOut);
            }
        }

        /// <summary>
        /// Stops the all <see cref="SoundEventBase">SoundEvents</see>
        /// </summary>
        /// <param name='allowFadeOut'>
        /// If the <see cref="SoundEventBase">SoundEvents</see> should be allowed to fade out. Otherwise they are going to be stopped immediately
        /// </param>
        public void StopEverything(bool allowFadeOut = true) {
            Internals.InternalStopEverything(allowFadeOut);
        }

        /// <summary>
        /// Plays the <see cref="SoundEventBase">SoundEvent</see> with the 2D <see cref="Transform"/> as owner
        /// Useful to play e.g. UI or other 2D sounds without having to pass a <see cref="Transform"/>
        /// To make the sound 2D you still need to disable distance and set spatial blend to 0 in the <see cref="SoundContainerBase">SoundContainer</see>
        /// </summary>
        /// <param name='soundEvent'>
        /// The <see cref="SoundEventBase">SoundEvent</see> to play
        /// </param>
        public void Play2D(SoundEvent soundEvent) {
            Play(soundEvent, Internals.cached2DTransform);
        }

        /// <summary>
        /// Plays the <see cref="SoundEventBase">SoundEvent</see> with the Local <see cref="SoundTagBase">SoundTag</see> with the 2D <see cref="Transform"/> as owner
        /// Useful to play e.g. UI or other 2D sounds without having to pass a <see cref="Transform"/>
        /// To make the sound 2D you still need to disable distance and set spatial blend to 0 in the <see cref="SoundContainerBase">SoundContainer</see>
        /// </summary>
        /// <param name='soundEvent'>
        /// The <see cref="SoundEventBase">SoundEvent</see> to play
        /// </param>
        /// <param name='localSoundTag'>
        /// The <see cref="SoundTagBase">SoundTag</see> which will determine the Local <see cref="SoundTagBase">SoundTag</see> of the <see cref="SoundEventBase">SoundEvent</see>
        /// </param>
        public void Play2D(SoundEvent soundEvent, SoundTag localSoundTag) {
            Play(soundEvent, Internals.cached2DTransform, localSoundTag);
        }

        /// <summary>
        /// Plays the <see cref="SoundEventBase">SoundEvent</see> with <see cref="SoundParameterInternals"/> with the 2D <see cref="Transform"/> as owner
        /// Useful to play e.g. UI or other 2D sounds without having to pass a <see cref="Transform"/>
        /// To make the sound 2D you still need to disable distance and set spatial blend to 0 in the <see cref="SoundContainerBase">SoundContainer</see>
        /// </summary>
        /// <param name='soundEvent'>
        /// The <see cref="SoundEventBase">SoundEvent</see> to play
        /// </param>
        /// <param name='soundParameters'>
        /// For example <see cref="SoundParameterVolumeDecibel"/> is used to modify how the <see cref="SoundEventBase">SoundEvent</see> is played
        /// </param>
        public void Play2D(SoundEvent soundEvent, params SoundParameterInternals[] soundParameters) {
            Play(soundEvent, Internals.cached2DTransform, soundParameters);
        }

        /// <summary>
        /// Plays the <see cref="SoundEventBase">SoundEvent</see> with <see cref="SoundParameterInternals"/> with the Local <see cref="SoundTagBase">SoundTag</see> with the 2D <see cref="Transform"/> as owner
        /// Useful to play e.g. UI or other 2D sounds without having to pass a <see cref="Transform"/>
        /// To make the sound 2D you still need to disable distance and set spatial blend to 0 in the <see cref="SoundContainerBase">SoundContainer</see>
        /// </summary>
        /// <param name='soundEvent'>
        /// The <see cref="SoundEventBase">SoundEvent</see> to play
        /// </param>
        /// <param name='localSoundTag'>
        /// The <see cref="SoundTagBase">SoundTag</see> which will determine the Local <see cref="SoundTagBase">SoundTag</see> of the <see cref="SoundEventBase">SoundEvent</see>
        /// </param>
        /// <param name='soundParameters'>
        /// For example <see cref="SoundParameterVolumeDecibel"/> is used to modify how the <see cref="SoundEventBase">SoundEvent</see> is played
        /// </param>
        public void Play2D(SoundEvent soundEvent, SoundTag localSoundTag, params SoundParameterInternals[] soundParameters) {
            Play(soundEvent, Internals.cached2DTransform, localSoundTag, soundParameters);
        }

        /// <summary>
        /// Stops the <see cref="SoundEventBase">SoundEvent</see> at the 2D <see cref="Transform"/>
        /// </summary>
        /// <param name='soundEvent'>
        /// The <see cref="SoundEventBase">SoundEvent</see> to stop
        /// </param>
        /// <param name='allowFadeOut'>
        /// If the <see cref="SoundEventBase">SoundEvent</see> should be allowed to fade out. Otherwise it is going to be stopped immediately
        /// </param>
        public void Stop2D(SoundEvent soundEvent, bool allowFadeOut = true) {
            Stop(soundEvent, Internals.cached2DTransform, allowFadeOut);
        }

        /// <summary>
        /// Stops all <see cref="SoundEventBase">SoundEvents</see> at the 2D <see cref="Transform"/>
        /// </summary>
        /// <param name='allowFadeOut'>
        /// If the <see cref="SoundEventBase">SoundEvent</see> should be allowed to fade out. Otherwise it is going to be stopped immediately
        /// </param>
        public void StopAllAt2D(bool allowFadeOut = true) {
            StopAllAtOwner(Internals.cached2DTransform, allowFadeOut);
        }

        /// <summary>
        /// Plays the <see cref="SoundEventBase">SoundEvent</see> at the <see cref="SoundManagerBase">SoundManagers</see> music <see cref="Transform"/>
        /// </summary>
        /// <param name="soundEvent">
        /// The <see cref="SoundEventBase">SoundEvent</see> to play
        /// </param>
        /// <param name="stopAllOtherMusic">
        /// If all other <see cref="SoundEventBase">SoundEvents</see> played at the <see cref="SoundManagerBase">SoundManagers</see> music <see cref="Transform"/> should be stopped
        /// </param>
        /// <param name="allowFadeOut">
        /// If the other stopped <see cref="SoundEventBase">SoundEvent</see> should be allowed to fade out. Otherwise they are going to be stopped immediately
        /// </param>
        public void PlayMusic(SoundEvent soundEvent, bool stopAllOtherMusic = true, bool allowFadeOut = true) {
            if (stopAllOtherMusic) {
                StopAllMusic(allowFadeOut);
            }
            Play(soundEvent, Internals.cachedMusicTransform);
        }

        /// <summary>
        /// Plays the <see cref="SoundEventBase">SoundEvent</see> at the <see cref="SoundManagerBase">SoundManagers</see> music <see cref="Transform"/>
        /// </summary>
        /// <param name="soundEvent">
        /// The <see cref="SoundEventBase">SoundEvent</see> to play
        /// </param>
        /// <param name="stopAllOtherMusic">
        /// If all other <see cref="SoundEventBase">SoundEvents</see> played at the <see cref="SoundManagerBase">SoundManagers</see> music <see cref="Transform"/> should be stopped
        /// </param>
        /// <param name="allowFadeOut">
        /// If the other stopped <see cref="SoundEventBase">SoundEvent</see> should be allowed to fade out. Otherwise they are going to be stopped immediately
        /// </param>
        /// <param name="soundParameters">
        /// For example <see cref="SoundParameterVolumeDecibel"/> is used to modify how the <see cref="SoundEventBase">SoundEvent</see> is played
        /// </param>
        public void PlayMusic(SoundEvent soundEvent, bool stopAllOtherMusic = true, bool allowFadeOut = true, params SoundParameterInternals[] soundParameters) {
            if (stopAllOtherMusic) {
                StopAllMusic(allowFadeOut);
            }
            Play(soundEvent, Internals.cachedMusicTransform, soundParameters);
        }

        /// <summary>
        /// Stops the <see cref="SoundEventBase">SoundEvent</see> playing at the <see cref="SoundManagerBase">SoundManagers</see> music <see cref="Transform"/>
        /// </summary>
        /// <param name='soundEvent'>
        /// The <see cref="SoundEventBase">SoundEvent</see> to stop
        /// </param>
        /// <param name='allowFadeOut'>
        /// If the other stopped <see cref="SoundEventBase">SoundEvent</see> should be allowed to fade out. Otherwise they are going to be stopped immediately
        /// </param>
        public void StopMusic(SoundEvent soundEvent, bool allowFadeOut = true) {
            Stop(soundEvent, Internals.cachedMusicTransform, allowFadeOut);
        }

        /// <summary>
        /// Stops the all <see cref="SoundEventBase">SoundEvents</see> playing at the <see cref="SoundManagerBase">SoundManagers</see> music <see cref="Transform"/>
        /// </summary>
        /// <param name='allowFadeOut'>
        /// If the <see cref="SoundEventBase">SoundEvent</see> should be allowed to fade out. Otherwise it is going to be stopped immediately
        /// </param>
        public void StopAllMusic(bool allowFadeOut = true) {
            StopAllAtOwner(Internals.cachedMusicTransform, allowFadeOut);
        }

        /// <summary>
        /// <para> If playing it returns <see cref="SoundEventState.Playing"/> </para> 
        /// <para> If not playing, but it is delayed it returns <see cref="SoundEventState.Delayed"/> </para> 
        /// <para> If not playing and it is not delayed it returns <see cref="SoundEventState.NotPlaying"/> </para> 
        /// <para> If the <see cref="SoundEventBase">SoundEvent</see> or <see cref="Transform"/> is null it returns <see cref="SoundEventState.NotPlaying"/> </para> 
        /// </summary>
        /// <param name="soundEvent"> The <see cref="SoundEventBase">SoundEvent</see> get the <see cref="SoundEventState"/> from </param>
        /// <param name="owner"> The owner <see cref="Transform"/> </param>
        /// <returns> Returns <see cref="SoundEventState"/> of the <see cref="SoundEventBase">SoundEvents</see> <see cref="SoundEventInstance"/> </returns>
        public SoundEventState GetSoundEventState(SoundEvent soundEvent, Transform owner) {
            return Internals.InternalGetSoundEventState(soundEvent, owner);
        }

        /// <summary>
        /// <para> Returns the length (in seconds) of the <see cref="AudioClip"/> in the last played <see cref="AudioSource"/> </para>
        /// <para> Returns 0 if the <see cref="SoundEventInstance"/> is not playing </para>
        /// <para> Returns 0 if the <see cref="SoundEventBase">SoundEvent</see> or <see cref="Transform"/> is null </para>
        /// <para> If it should be scaled by pitch. E.g. -12 semitones will be twice as long </para>
        /// </summary>
        /// <param name="soundEvent"> The <see cref="SoundEventBase">SoundEvent</see> get the length from </param>
        /// <param name="owner"> The owner <see cref="Transform"/> </param>
        /// <param name="pitchSpeed"> If it should be scaled by pitch. E.g. -12 semitones will be twice as long </param>
        /// <returns> Length in seconds </returns>
        public float GetLastPlayedClipLength(SoundEvent soundEvent, Transform owner, bool pitchSpeed) {
            return Internals.InternalGetLastPlayedClipLength(soundEvent, owner, pitchSpeed);
        }

        /// <summary>
        /// <para> Returns the current time (in seconds) of the <see cref="AudioClip"/> in the last played <see cref="AudioSource"/> </para>
        /// <para> Returns 0 if the <see cref="SoundEventInstance"/> is not playing </para>
        /// <para> Returns 0 if the <see cref="SoundEventBase">SoundEvent</see> or <see cref="Transform"/> is null </para>
        /// <para> If it should be scaled by pitch. E.g. -12 semitones will be twice as long </para>
        /// </summary>
        /// <param name="soundEvent"> The <see cref="SoundEventBase">SoundEvent</see> get the time from </param>
        /// <param name="owner"> The owner <see cref="Transform"/> </param>
        /// <param name="pitchSpeed"> If it should be scaled by pitch. E.g. -12 semitones will be twice as long </param>
        /// <returns> Time in seconds </returns>
        public float GetLastPlayedClipTime(SoundEvent soundEvent, Transform owner, bool pitchSpeed) {
            return Internals.InternalGetLastPlayedClipTime(soundEvent, owner, pitchSpeed);
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
            Internals.InternalGetSpectrumData(soundEvent, owner, ref samples, channel, window, spectrumDataFrom);
        }

        /// <summary>
        /// <para> Returns the max length (in seconds) of the <see cref="SoundEventBase">SoundEvent</see> (calculated from the longest audioClip) </para>
        /// <para> Is scaled by the pitch of the <see cref="SoundEventBase">SoundEvent</see> and <see cref="SoundContainerBase">SoundContainer</see> </para>
        /// <para> Does not take into account random, intensity or parameter pitch </para>
        /// <para> Returns 0 if the <see cref="SoundEventBase">SoundEvent</see> is null </para>
        /// </summary>
        /// <param name="soundEvent"> The <see cref="SoundEventBase">SoundEvent</see> get the length from </param>
        /// <returns> The max length in seconds </returns>
        public float GetMaxLength(SoundEvent soundEvent) {
            return Internals.InternalGetMaxLength(soundEvent);
        }

        /// <summary>
        /// <para> Returns the time (in seconds) since the <see cref="SoundEventBase">SoundEvent</see> was played </para>
        /// <para> Calculated using <see cref="Time.realtimeSinceStartup"/> </para>
        /// <para> Returns 0 if the <see cref="SoundEventInstance"/> is not playing </para>
        /// <para> Returns 0 if the <see cref="SoundEventBase">SoundEvent</see> or <see cref="Transform"/> is null </para>
        /// </summary>
        /// <param name="soundEvent"> The <see cref="SoundEventBase">SoundEvent</see> get the time played from </param>
        /// <param name="owner"> The owner <see cref="Transform"/> </param>
        /// <returns> Time in seconds </returns>
        public float GetTimePlayed(SoundEvent soundEvent, Transform owner) {
            return Internals.InternalGetTimePlayed(soundEvent, owner);
        }

        /// <summary>
        /// Sets the global <see cref="SoundTagBase">SoundTag</see>
        /// </summary>
        /// <param name='soundTag'>
        /// The <see cref="SoundTagBase">SoundTag</see> to change to
        /// </param>
        public void SetGlobalSoundTag(SoundTag soundTag) {
            Internals.settings.globalSoundTag = soundTag;
        }

        /// <summary>
        /// Returns the global <see cref="SoundTagBase">SoundTag</see>
        /// </summary>
        /// <returns> The global <see cref="SoundTagBase">SoundTag</see> </returns>
        public SoundTag GetGlobalSoundTag() {
            return (SoundTag)Internals.settings.globalSoundTag;
        }

        /// <summary>
        /// Sets the global distance scale (default is a scale of 100 units)
        /// </summary>
        /// <param name='distanceScale'>
        /// The new range scale
        /// </param>
        public void SetGlobalDistanceScale(float distanceScale) {
            Internals.settings.distanceScale = Mathf.Clamp(distanceScale, 0f, Mathf.Infinity);
        }

        /// <summary>
        /// Returns the global distance scale
        /// </summary>
        public float GetGlobalDistanceScale() {
            return Internals.settings.distanceScale;
        }

        /// <summary>
        /// Set if speed of sound should be enabled
        /// </summary>
        /// <param name='speedOfSoundEnabled'>
        /// Should speed of Sound be active
        /// </param>
        public void SetSpeedOfSoundEnabled(bool speedOfSoundEnabled) {
            Internals.settings.speedOfSoundEnabled = speedOfSoundEnabled;
        }

        /// <summary>
        /// <para> Set the speed of sound scale </para>
        /// <para> The default is a multiplier of 1 (by the base value of 340 unity units per second) </para>
        /// </summary>
        /// <param name='speedOfSoundScale'>
        /// The scale of speed of Sound (default is a multipler of 1)
        /// </param>
        public void SetSpeedOfSoundScale(float speedOfSoundScale) {
            Internals.settings.speedOfSoundScale = Mathf.Clamp(speedOfSoundScale, 0f, Mathf.Infinity);
        }

        /// <summary>
        /// Returns the speed of sound scale
        /// </summary>
        public float GetSpeedOfSoundScale() {
            return Internals.settings.speedOfSoundScale;
        }

        /// <summary>
        /// Sets the <see cref="Voice"/> limit
        /// </summary>
        /// <param name='voiceLimit'>
        /// The maximum number of <see cref="Voice"/>s which can be played at the same time
        /// </param>
        public void SetVoiceLimit(int voiceLimit) {
            Internals.settings.voiceLimit = Mathf.Clamp(voiceLimit, 0, int.MaxValue);
        }

        /// <summary>
        /// Returns the <see cref="Voice"/> limit
        /// </summary>
        public int GetVoiceLimit() {
            return Internals.settings.voiceLimit;
        }

        /// <summary>
        /// Sets the <see cref="VoiceEffect"/> limit
        /// </summary>
        public void SetVoiceEffectLimit(int voiceEffectLimit) {
            Internals.settings.voiceEffectLimit = Mathf.Clamp(voiceEffectLimit, 0, int.MaxValue);
        }

        /// <summary>
        /// Returns the <see cref="VoiceEffect"/> limit
        /// </summary>
        public int GetVoiceEffectLimit() {
            return Internals.settings.voiceEffectLimit;
        }

        /// <summary>
        /// Disables/enables all the Play/PlayAtPosition functionality
        /// </summary>
        /// <param name="disablePlayingSounds"></param>
        public void SetDisablePlayingSounds(bool disablePlayingSounds) {
            Internals.settings.disablePlayingSounds = disablePlayingSounds;
        }

        /// <summary>
        /// If the Play/PlayAtPosition functionality is disabled
        /// </summary>
        public bool GetDisablePlayingSounds() {
            return Internals.settings.disablePlayingSounds;
        }
    }
}