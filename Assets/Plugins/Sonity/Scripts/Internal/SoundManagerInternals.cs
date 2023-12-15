// Created by Victor Engström
// Copyright 2023 Sonigon AB
// http://www.sonity.org/

using UnityEngine;
using System.Collections.Generic;
using System;

namespace Sonity.Internal {

    [Serializable]
    public class SoundEventInstanceDictionaryValue {
#if UNITY_EDITOR
        public SoundEventBase statisticsSoundEvent;
#endif
        // The int is the transform instance ID
        public Dictionary<int, SoundEventInstance> instanceDictionary = new Dictionary<int, SoundEventInstance>();
        public Stack<SoundEventInstance> unusedInstanceStack = new Stack<SoundEventInstance>();
    }

    public class CheckedDictionaryValueSoundEvent {

        public bool nullChecked = false;
        public bool isNull = false;

        public bool savedMaxChecked = false;
        public float savedMaxLength = 0f;

        public bool savedMinLengthChecked = false;
        // Is MinLengthWithoutPitchOnlyFirstSoundContainer
        public float savedMinLength = 0f;

        public bool triggerOnPlayIsInfiniteLoop = false;
        public bool triggerOnPlayIsInfiniteLoopChecked = false;
        public bool triggerOnStopIsInfiniteLoop = false;
        public bool triggerOnStopIsInfiniteLoopChecked = false;
        // TriggerOnTail should be allowed to loop for e.g. music

        public int statisticsNumberOfPlays = 0;
    }

    public class CheckedDictionaryValueSoundDataGroup {
        public bool isInfiniteLoopChecked = false;
        public bool isInfiniteLoop = false;
    }

    public class CheckedDictionaryValueSoundMix {
        public bool isInfiniteLoopChecked = false;
        public bool isInfiniteLoop = false;
    }

    [Serializable]
    public class SoundManagerInternals {

        public SoundManagerInternalsSettings settings = new SoundManagerInternalsSettings();
#if UNITY_EDITOR
        public SoundManagerInternalsStatistics statistics = new SoundManagerInternalsStatistics();
#endif

        [NonSerialized]
        public bool isGoingToDelete = false;
        [NonSerialized]
        public bool applicationIsQuitting = false;

        [NonSerialized]
        public Dictionary<SoundEventBase, SoundEventInstanceDictionaryValue> soundEventDictionary = new Dictionary<SoundEventBase, SoundEventInstanceDictionaryValue>();
        [NonSerialized]
        public SoundManagerVoicePool voicePool = new SoundManagerVoicePool();
        [NonSerialized]
        public SoundManagerVoiceEffectPool voiceEffectPool = new SoundManagerVoiceEffectPool();

        [NonSerialized]
        public Dictionary<SoundEventBase, CheckedDictionaryValueSoundEvent> checkedDictionarySoundEvent = new Dictionary<SoundEventBase, CheckedDictionaryValueSoundEvent>();
        [NonSerialized]
        public Dictionary<SoundDataGroupBase, CheckedDictionaryValueSoundDataGroup> checkedDictionarySoundDataGroup = new Dictionary<SoundDataGroupBase, CheckedDictionaryValueSoundDataGroup>();
        [NonSerialized]
        public Dictionary<SoundMixBase, CheckedDictionaryValueSoundMix> checkedDictionarySoundMix = new Dictionary<SoundMixBase, CheckedDictionaryValueSoundMix>();

        // Chached Objects
        [NonSerialized]
        public Transform cachedSoundManagerTransform;
        [NonSerialized]
        public GameObject cachedSoundEventPoolGameObject;
        [NonSerialized]
        public Transform cachedSoundEventPoolTransform;
        [NonSerialized]
        public GameObject cachedVoicePoolGameObject;
        [NonSerialized]
        public Transform cachedVoicePoolTransform;
        [NonSerialized]
        public GameObject cachedMusicGameObject;
        [NonSerialized]
        public Transform cachedMusicTransform;
        [NonSerialized]
        public GameObject cached2DGameObject;
        [NonSerialized]
        public Transform cached2DTransform;
        [NonSerialized]
        public AudioListener cachedAudioListener;
        [NonSerialized]
        public Transform cachedAudioListenerTransform;
        [NonSerialized]
        public AudioListenerDistanceBase cachedAudioListenerDistance;
        [NonSerialized]
        public Transform cachedAudioListenerDistanceTransform;

        public void AwakeCheck() {
            FindAudioListener();
            FindAudioListenerDistance();
            if (settings.dontDestroyOnLoad) {
                // DontDestroyOnLoad only works for root GameObjects
                SoundManagerBase.Instance.gameObject.transform.parent = null;
                UnityEngine.Object.DontDestroyOnLoad(SoundManagerBase.Instance.gameObject);
            }
            if (cachedSoundEventPoolGameObject == null) {
                cachedSoundEventPoolGameObject = new GameObject($"SonitySoundEventPool");
                cachedSoundEventPoolTransform = cachedSoundEventPoolGameObject.transform;
                cachedSoundEventPoolTransform.parent = cachedSoundManagerTransform;
            }
            if (cachedVoicePoolGameObject == null) {
                cachedVoicePoolGameObject = new GameObject("SonityVoicePool");
                cachedVoicePoolTransform = cachedVoicePoolGameObject.transform;
                cachedVoicePoolTransform.parent = cachedSoundManagerTransform;
                // Preload Voices on Awake
                voicePool.CreateVoice(settings.voicePreload, true);
            }
            if (cachedMusicGameObject == null) {
                cachedMusicGameObject = new GameObject("SonityMusic");
                cachedMusicTransform = cachedMusicGameObject.transform;
                cachedMusicTransform.parent = cachedSoundManagerTransform;
            }
            if (cached2DGameObject == null) {
                cached2DGameObject = new GameObject("Sonity2D");
                cached2DTransform = cached2DGameObject.transform;
                cached2DTransform.parent = cachedSoundManagerTransform;
            }
            cachedSoundManagerTransform.position = Vector3.zero;
            cachedSoundEventPoolTransform.position = Vector3.zero;
            cachedVoicePoolTransform.position = Vector3.zero;
            cachedMusicTransform.position = Vector3.zero;
            cached2DTransform.position = Vector3.zero;
        }

        public void Destroy() {
            isGoingToDelete = true;
            InternalOnDestroyForceStopEverything();
            if (cachedSoundEventPoolGameObject != null) {
                UnityEngine.Object.Destroy(cachedSoundEventPoolGameObject);
            }
            if (cachedMusicGameObject != null) {
                UnityEngine.Object.Destroy(cachedMusicGameObject);
            }
            if (cached2DGameObject != null) {
                UnityEngine.Object.Destroy(cached2DGameObject);
            }
        }

        public void FindAudioListener() {
            if (cachedAudioListener == null || !cachedAudioListener.isActiveAndEnabled) {
                cachedAudioListener = UnityEngine.Object.FindObjectOfType<AudioListener>();
                if (cachedAudioListener == null) {
                    if (ShouldDebug.Warnings()) {
                        Debug.LogWarning(
                            $"Sonity.{nameof(NameOf.SoundManager)} could find no active {nameof(AudioListener)} in the scene." +
                            $"If you are creating one in runtime, make sure it is created in Awake()."
                            );
                    }
                } else {
                    cachedAudioListenerTransform = cachedAudioListener.transform;
                }
            }
        }

        private void FindAudioListenerDistance() {
            if (settings.overrideListenerDistance) {
                if (cachedAudioListenerDistance == null || !cachedAudioListenerDistance.isActiveAndEnabled) {
                    cachedAudioListenerDistance = UnityEngine.Object.FindObjectOfType<AudioListenerDistanceBase>();
                    if (cachedAudioListenerDistance == null) {
                        if (ShouldDebug.Warnings()) {
                            Debug.LogWarning(
                                $"Sonity.{nameof(NameOf.SoundManager)} could find no active {nameof(NameOf.AudioListenerDistance)} in the scene." +
                                $"If you are creating one in runtime, make sure it is created in Awake()."
                                );
                        }
                    } else {
                        cachedAudioListenerDistanceTransform = cachedAudioListenerDistance.transform;
                    }
                }
            }
        }

        public float GetDistanceToAudioListener(Vector3 position) {
            if (settings.overrideListenerDistance && settings.overrideListenerDistanceAmount > 0f) {
                if (settings.overrideListenerDistanceAmount < 100f) {
                    if (cachedAudioListener == null) {
                        FindAudioListener();
                        if (cachedAudioListener == null) {
                            return 0f;
                        }
                    }
                    if (cachedAudioListenerDistance == null) {
                        FindAudioListenerDistance();
                        if (cachedAudioListenerDistance == null) {
                            return 0f;
                        }
                    }
                    // Position between the AudioListener and Audio Listener Distance
                    return Vector3.Distance(position, Vector3.Lerp(cachedAudioListenerTransform.position, cachedAudioListenerDistanceTransform.position, settings.overrideListenerDistanceAmount * 0.01f));
                } else {
                    if (cachedAudioListenerDistance == null) {
                        FindAudioListenerDistance();
                        if (cachedAudioListenerDistance == null) {
                            return 0f;
                        }
                    }
                    // Position at the AudioListenerDistance
                    return Vector3.Distance(position, cachedAudioListenerDistanceTransform.position);
                }
            } else {
                if (cachedAudioListener == null) {
                    FindAudioListener();
                    if (cachedAudioListener == null) {
                        return 0f;
                    }
                }
                // Position at the AudioListener
                return Vector3.Distance(position, cachedAudioListenerTransform.position);
            }
        }

        public float GetAngleToAudioListener(Vector3 position) {
            if (cachedAudioListener == null) {
                if (ShouldDebug.Warnings()) {
                    Debug.LogWarning(
                        $"Sonity.{nameof(NameOf.SoundManager)} could find no {nameof(AudioListener)} in the scene. " +
                        $"If you are creating one in runtime, make sure it is created in Awake()."
                        );
                }
            } else {
                return AngleAroundAxis.Get(position - cachedAudioListenerTransform.position, cachedAudioListenerTransform.forward, Vector3.up);
            }
            return 0f;
        }

        // Managed update has its own temp variables so nothing else can change them while its running
        [NonSerialized]
        private SoundEventInstance managedUpdateTempInstance;
        [NonSerialized]
        private Dictionary<SoundEventBase, SoundEventInstanceDictionaryValue>.Enumerator managedUpdateTempInstanceDictionaryValueEnumerator;
        [NonSerialized]
        private Dictionary<int, SoundEventInstance>.Enumerator managedUpdateTempInstanceEnumerator;

        public void ManagedUpdate() {
            // Managed Update of the SoundEvent Instances
            managedUpdateTempInstanceDictionaryValueEnumerator = soundEventDictionary.GetEnumerator();
            while (managedUpdateTempInstanceDictionaryValueEnumerator.MoveNext()) {
                managedUpdateTempInstanceEnumerator = managedUpdateTempInstanceDictionaryValueEnumerator.Current.Value.instanceDictionary.GetEnumerator();
                while (managedUpdateTempInstanceEnumerator.MoveNext()) {
                    managedUpdateTempInstance = managedUpdateTempInstanceEnumerator.Current.Value;
                    managedUpdateTempInstance.ManagedUpdate();
                }
            }

            // Waiting to Play (for TriggerOnTail)
            if (toPlayOnTailAdded) {
                toPlayOnTailAdded = false;
                for (int i = 0; i < toPlayOnTailInstances.Length; i++) {
                    if (toPlayOnTailInstances[i] != null) {
                        if (toPlayOnTailInstances[i].managedUpdateWaitingToPlayOnTail) {
                            toPlayOnTailInstances[i].managedUpdateWaitingToPlayOnTail = false;
                            toPlayOnTailInstances[i].TriggerOnTail();
                        }
                        toPlayOnTailInstances[i] = null;
                    }
                }
            }

            // Waiting to pool
            if (toPoolAdded) {
                toPoolAdded = false;
                for (int i = 0; i < toPoolInstances.Length; i++) {
                    if (toPoolInstances[i] != null) {
                        if (toPoolInstances[i].waitingForPooling) {
                            toPoolInstances[i].waitingForPooling = false;
                            // Move instance to stack
                            soundEventDictionary[toPoolInstances[i].soundEvent].instanceDictionary.Remove(toPoolInstances[i].ownerTransformInstanceID);
                            soundEventDictionary[toPoolInstances[i].soundEvent].unusedInstanceStack.Push(toPoolInstances[i]);
                            toPoolInstances[i].gameObject.SetActive(false);
                        }
                        toPoolInstances[i] = null;
                    }
                }
            }

            // Stops voices after a certain time
            for (int i = 0; i < voicePool.voiceIndexStopQueue.Count; i++) {
                if (voicePool.voicePool[voicePool.voiceIndexStopQueue.Peek()].GetState() == VoiceState.Pause) {
                    if (voicePool.voicePool[voicePool.voiceIndexStopQueue.Peek()].stopTime < Time.realtimeSinceStartup) {
                        voicePool.voicePool[voicePool.voiceIndexStopQueue.Peek()].SetState(VoiceState.Stop, true);
                        voicePool.voicePool[voicePool.voiceIndexStopQueue.Peek()].cachedGameObject.SetActive(false);
                        voicePool.voiceIndexStopQueue.Dequeue();
                    } else {
                        break;
                    }
                } else {
                    voicePool.voiceIndexStopQueue.Dequeue();
                }
            }
        }

        [NonSerialized]
        private bool toPlayOnTailAdded = false;
        [NonSerialized]
        private SoundEventInstance[] toPlayOnTailInstances = new SoundEventInstance[0];

        public void AddManagedUpdateToPlayOnTail(SoundEventInstance soundEventInstance) {
            toPlayOnTailAdded = true;
            for (int i = 0; i < toPlayOnTailInstances.Length; i++) {
                // Find empty slot
                if (toPlayOnTailInstances[i] == null) {
                    toPlayOnTailInstances[i] = soundEventInstance;
                    return;
                } else {
                    // Done slot
                    if (!toPlayOnTailInstances[i].managedUpdateWaitingToPlayOnTail) {
                        toPlayOnTailInstances[i] = soundEventInstance;
                        return;
                    }
                }
            }
            // If all used, add new slot
            Array.Resize(ref toPlayOnTailInstances, toPlayOnTailInstances.Length + 1);
            toPlayOnTailInstances[toPlayOnTailInstances.Length - 1] = soundEventInstance;
        }

        [NonSerialized]
        private bool toPoolAdded = false;
        [NonSerialized]
        private SoundEventInstance[] toPoolInstances = new SoundEventInstance[0];

        public void AddManagedUpdateToPool(SoundEventInstance soundEventInstance) {
            toPoolAdded = true;
            for (int i = 0; i < toPoolInstances.Length; i++) {
                // Find empty slot
                if (toPoolInstances[i] == null) {
                    toPoolInstances[i] = soundEventInstance;
                    return;
                } else {
                    // Done slot
                    if (!toPoolInstances[i].waitingForPooling) {
                        toPoolInstances[i] = soundEventInstance;
                        return;
                    }
                }
            }
            // If all used, add new slot
            Array.Resize(ref toPoolInstances, toPoolInstances.Length + 1);
            toPoolInstances[toPoolInstances.Length - 1] = soundEventInstance;
        }

        [NonSerialized]
        private SoundEventInstance tempInstance;
        [NonSerialized]
        private SoundEventInstanceDictionaryValue tempInstanceDictionaryValue;
        [NonSerialized]
        private Dictionary<SoundEventBase, SoundEventInstanceDictionaryValue>.Enumerator tempInstanceDictionaryValueEnumerator;
        [NonSerialized]
        private Dictionary<int, SoundEventInstance>.Enumerator tempInstanceEnumerator;
        [NonSerialized]
        private CheckedDictionaryValueSoundEvent checkedDictionaryValueSoundEvent;
        [NonSerialized]
        private CheckedDictionaryValueSoundDataGroup checkedDictionaryValueSoundDataGroup;
        [NonSerialized]
        private CheckedDictionaryValueSoundMix checkedDictionaryValueSoundMix;

        public void InternalPlay(
            SoundEventBase soundEvent, SoundEventPlayType playType, Transform owner, Vector3? positionVector, Transform positionTransform,
            SoundEventModifier soundEventModifierSoundPicker, SoundEventModifier soundEventModifierSoundTag, 
            SoundParameterInternals[] soundParameters, SoundParameterInternals soundParameterDistanceScale, SoundTagBase localSoundTag) {
            if (!settings.disablePlayingSounds && !applicationIsQuitting) {
                // If polyphony should be limited globally
                if (soundEvent.internals.data.polyphonyMode == PolyphonyMode.LimitedGlobally) {
                    // Play only has the owner as position
                    if (playType == SoundEventPlayType.Play) {
                        positionTransform = owner;
                        playType = SoundEventPlayType.PlayAtTransform;
                    }
                    owner = SoundManagerBase.Instance.Internals.cachedSoundManagerTransform;
                }

                if (soundEventDictionary.TryGetValue(soundEvent, out tempInstanceDictionaryValue)) {
                    // Checks if the SoundEvent should retrigger itself
                    if (tempInstanceDictionaryValue.instanceDictionary.TryGetValue(owner.GetInstanceID(), out tempInstance)) {
                        if (!tempInstance.gameObject.activeSelf) {
                            tempInstance.gameObject.SetActive(true);
                        }
                        tempInstance.Play(playType, owner, positionVector, positionTransform, soundEventModifierSoundPicker, soundEventModifierSoundTag, soundParameters, soundParameterDistanceScale, localSoundTag);
                        return;
                    } else {
                        // Checks if there is a unused instance to use
                        if (tempInstanceDictionaryValue.unusedInstanceStack.Count > 0) {
                            tempInstance = tempInstanceDictionaryValue.unusedInstanceStack.Pop();
                            tempInstanceDictionaryValue.instanceDictionary.Add(owner.GetInstanceID(), tempInstance);
                            if (!tempInstance.gameObject.activeSelf) {
                                tempInstance.gameObject.SetActive(true);
                            }
                            tempInstance.Play(playType, owner, positionVector, positionTransform, soundEventModifierSoundPicker, soundEventModifierSoundTag, soundParameters, soundParameterDistanceScale, localSoundTag);
                            return;
                        }
                        // Create a new instance
                        GameObject tempGameObject = new GameObject();
                        tempGameObject.AddComponent<SoundEventInstance>();
                        tempInstance = tempGameObject.GetComponent<SoundEventInstance>();
                        tempInstance.Initialize(soundEvent);
                        tempInstance.GetTransform().parent = cachedSoundEventPoolTransform;
                        tempInstanceDictionaryValue.instanceDictionary.Add(owner.GetInstanceID(), tempInstance);
                        tempInstance.Play(playType, owner, positionVector, positionTransform, soundEventModifierSoundPicker, soundEventModifierSoundTag, soundParameters, soundParameterDistanceScale, localSoundTag);
                        return;
                    }
                } else {
                    // Nullcheck SoundEvent
                    if (!checkedDictionarySoundEvent.TryGetValue(soundEvent, out checkedDictionaryValueSoundEvent)) {
                        checkedDictionaryValueSoundEvent = new CheckedDictionaryValueSoundEvent();
                        checkedDictionarySoundEvent.Add(soundEvent, checkedDictionaryValueSoundEvent);
                    }
                    if (!checkedDictionaryValueSoundEvent.nullChecked) {
                        checkedDictionaryValueSoundEvent.nullChecked = true;
                        checkedDictionaryValueSoundEvent.isNull = soundEvent.IsNull();
#if UNITY_EDITOR
                        // So the problem can be fixed in runtime
                        checkedDictionaryValueSoundEvent.nullChecked = false;
#endif
                    }
                    if (checkedDictionaryValueSoundEvent.isNull) {
                        return;
                    }
                    // Create a new instance
                    GameObject tempGameObject = new GameObject();
                    tempGameObject.AddComponent<SoundEventInstance>();
                    tempInstance = tempGameObject.GetComponent<SoundEventInstance>();
                    tempInstance.Initialize(soundEvent);
                    tempInstance.GetTransform().parent = cachedSoundEventPoolTransform;
                    tempInstanceDictionaryValue = new SoundEventInstanceDictionaryValue();
#if UNITY_EDITOR
                    tempInstanceDictionaryValue.statisticsSoundEvent = soundEvent;
#endif
                    tempInstanceDictionaryValue.instanceDictionary.Add(owner.GetInstanceID(), tempInstance);
                    soundEventDictionary.Add(soundEvent, tempInstanceDictionaryValue);
                    tempInstance.Play(playType, owner, positionVector, positionTransform, soundEventModifierSoundPicker, soundEventModifierSoundTag, soundParameters, soundParameterDistanceScale, localSoundTag);
                    return;
                }
            }
        }

        public void InternalStop(SoundEventBase soundEvent, Transform owner, bool allowFadeOut) {
            if (soundEvent.internals.data.polyphonyMode == PolyphonyMode.LimitedPerOwner) {
                if (soundEventDictionary.TryGetValue(soundEvent, out tempInstanceDictionaryValue)) {
                    if (tempInstanceDictionaryValue.instanceDictionary.TryGetValue(owner.GetInstanceID(), out tempInstance)) {
                        tempInstance.PoolAllVoices(allowFadeOut, true, false);
                        return;
                    }
                }
            } else if (soundEvent.internals.data.polyphonyMode == PolyphonyMode.LimitedGlobally) {
                // If limited globally then it uses the SoundManager transform
                if (soundEventDictionary.TryGetValue(soundEvent, out tempInstanceDictionaryValue)) {
                    if (tempInstanceDictionaryValue.instanceDictionary.TryGetValue(SoundManagerBase.Instance.Internals.cachedSoundManagerTransform.GetInstanceID(), out tempInstance)) {
                        tempInstance.PoolAllVoices(allowFadeOut, true, false);
                        return;
                    }
                }
            }
        }

        public void InternalStopAtPosition(SoundEventBase soundEvent, Transform position, bool allowFadeOut) {
            if (soundEventDictionary.TryGetValue(soundEvent, out tempInstanceDictionaryValue)) {
                tempInstanceEnumerator = tempInstanceDictionaryValue.instanceDictionary.GetEnumerator();
                while (tempInstanceEnumerator.MoveNext()) {
                    tempInstanceEnumerator.Current.Value.PoolVoicesWithPositionTransform(position, allowFadeOut);
                }
            }
        }

        public void InternalStopAllAtOwner(Transform owner, bool allowFadeOut) {
            tempInstanceDictionaryValueEnumerator = soundEventDictionary.GetEnumerator();
            while (tempInstanceDictionaryValueEnumerator.MoveNext()) {
                if (tempInstanceDictionaryValueEnumerator.Current.Value.instanceDictionary.TryGetValue(owner.GetInstanceID(), out tempInstance)) {
                    tempInstance.PoolAllVoices(allowFadeOut, true, false);
                }
            }
        }

        public void InternalStopEverywhere(SoundEventBase soundEvent, bool allowFadeOut) {
            if (soundEventDictionary.TryGetValue(soundEvent, out tempInstanceDictionaryValue)) {
                tempInstanceEnumerator = tempInstanceDictionaryValue.instanceDictionary.GetEnumerator();
                while (tempInstanceEnumerator.MoveNext()) {
                    tempInstanceEnumerator.Current.Value.PoolAllVoices(allowFadeOut, true, false);
                }
            }
        }

        public void InternalStopEverything(bool allowFadeOut) {
            tempInstanceDictionaryValueEnumerator = soundEventDictionary.GetEnumerator();
            while (tempInstanceDictionaryValueEnumerator.MoveNext()) {
                tempInstanceEnumerator = tempInstanceDictionaryValueEnumerator.Current.Value.instanceDictionary.GetEnumerator();
                while (tempInstanceEnumerator.MoveNext()) {
                    tempInstanceEnumerator.Current.Value.PoolAllVoices(allowFadeOut, true, false);
                }
            }
        }

        public void InternalOnDestroyForceStopEverything() {
            try {
                tempInstanceDictionaryValueEnumerator = soundEventDictionary.GetEnumerator();
                while (tempInstanceDictionaryValueEnumerator.MoveNext()) {
                    tempInstanceEnumerator = tempInstanceDictionaryValueEnumerator.Current.Value.instanceDictionary.GetEnumerator();
                    while (tempInstanceEnumerator.MoveNext()) {
                        tempInstanceEnumerator.Current.Value.PoolAllVoices(false, true, true);
                    }
                }
            } catch {

            }
        }

        /// <summary>
        /// <para> If playing it returns <see cref="SoundEventState.Playing"/> </para> 
        /// <para> If not playing, but its delayed it returns <see cref="SoundEventState.Delayed"/> </para> 
        /// <para> If not playing and its not delayed it returns <see cref="SoundEventState.NotPlaying"/> </para> 
        /// <para> If the <see cref="SoundEventBase">SoundEvent</see> or <see cref="Transform"/> is null it returns <see cref="SoundEventState.NotPlaying"/> </para> 
        /// </summary>
        /// <param name="soundEvent"> The <see cref="SoundEventBase">SoundEvent</see> get the <see cref="SoundEventState"/> from </param>
        /// <param name="owner"> The owner <see cref="Transform"/> </param>
        /// <returns> Returns <see cref="SoundEventState"/> of the <see cref="SoundEventBase">SoundEvents</see> <see cref="SoundEventInstance"/> </returns>
        public SoundEventState InternalGetSoundEventState(SoundEventBase soundEvent, Transform owner) {
            if (soundEvent == null || owner == null) {
                return SoundEventState.NotPlaying;
            }
            if (soundEventDictionary.TryGetValue(soundEvent, out tempInstanceDictionaryValue)) {
                if (tempInstanceDictionaryValue.instanceDictionary.TryGetValue(owner.GetInstanceID(), out tempInstance)) {
                    if (tempInstance != null) {
                        return tempInstance.GetSoundEventState();
                    }
                }
            }
            return SoundEventState.NotPlaying;
        }

        /// <summary>
        /// <para> Returns the length of the <see cref="AudioClip"/> in the last played <see cref="AudioSource"/> </para>
        /// <para> Returns 0 if the <see cref="SoundEventInstance"/> is not playing </para>
        /// <para> Returns 0 if the <see cref="SoundEventBase">SoundEvent</see> or <see cref="Transform"/> is null </para>
        /// </summary>
        /// <param name="soundEvent"> The <see cref="SoundEventBase">SoundEvent</see> get the length from </param>
        /// <param name="owner"> The owner <see cref="Transform"/> </param>
        /// <param name="pitchSpeed"> If it should be scaled by pitch. E.g. -12 semitones will be twice as long </param>
        /// <returns> Length in seconds </returns>
        public float InternalGetLastPlayedClipLength(SoundEventBase soundEvent, Transform owner, bool pitchSpeed) {
            if (soundEvent == null || owner == null) {
                return 0f;
            }
            if (soundEventDictionary.TryGetValue(soundEvent, out tempInstanceDictionaryValue)) {
                if (tempInstanceDictionaryValue.instanceDictionary.TryGetValue(owner.GetInstanceID(), out tempInstance)) {
                    if (tempInstance != null) {
                        return tempInstance.GetLastPlayedAudioSourceClipLength(pitchSpeed);
                    }
                }
            }
            return 0f;
        }

        /// <summary>
        /// <para> Returns the current time of the <see cref="AudioClip"/> in the last played <see cref="AudioSource"/> </para>
        /// <para> Returns 0 if the <see cref="SoundEventInstance"/> is not playing </para>
        /// <para> Returns 0 if the <see cref="SoundEventBase">SoundEvent</see> or <see cref="Transform"/> is null </para>
        /// </summary>
        /// <param name="soundEvent"> The <see cref="SoundEventBase">SoundEvent</see> get the time from </param>
        /// <param name="owner"> The owner <see cref="Transform"/> </param>
        /// <param name="pitchSpeed"> If it should be scaled by pitch. E.g. -12 semitones will be twice as long </param>
        /// <returns> Time in seconds </returns>
        public float InternalGetLastPlayedClipTime(SoundEventBase soundEvent, Transform owner, bool pitchSpeed) {
            if (soundEvent == null || owner == null) {
                return 0f;
            }
            if (soundEventDictionary.TryGetValue(soundEvent, out tempInstanceDictionaryValue)) {
                if (tempInstanceDictionaryValue.instanceDictionary.TryGetValue(owner.GetInstanceID(), out tempInstance)) {
                    if (tempInstance != null) {
                        return tempInstance.GetLastPlayedAudioSourceTime(pitchSpeed);
                    }
                }
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
        public void InternalGetSpectrumData(SoundEventBase soundEvent, Transform owner, ref float[] samples, int channel, FFTWindow window, SpectrumDataFrom spectrumDataFrom) {
            if (soundEvent == null || owner == null) {
                return;
            }
            if (soundEventDictionary.TryGetValue(soundEvent, out tempInstanceDictionaryValue)) {
                if (tempInstanceDictionaryValue.instanceDictionary.TryGetValue(owner.GetInstanceID(), out tempInstance)) {
                    if (tempInstance != null) {
                        tempInstance.GetSpectrumData(ref samples, channel, window, spectrumDataFrom);
                    }
                }
            }
        }

        /// <summary>
        /// <para> Returns the max length of the <see cref="SoundEventBase">SoundEvent</see> (calculated from the longest audioClip) </para>
        /// <para> Is scaled by the pitch of the <see cref="SoundEventBase">SoundEvent</see> and <see cref="SoundContainerBase">SoundContainer</see> </para>
        /// <para> Does not take into account random, intensity or parameter pitch </para>
        /// </summary>
        /// <param name="soundEvent"> The <see cref="SoundEventBase">SoundEvent</see> get the length from </param>
        /// <returns> The max length in seconds </returns>
        public float InternalGetMaxLength(SoundEventBase soundEvent) {
            if (soundEvent == null) {
                return 0f;
            }
            if (!checkedDictionarySoundEvent.TryGetValue(soundEvent, out checkedDictionaryValueSoundEvent)) {
                checkedDictionaryValueSoundEvent = new CheckedDictionaryValueSoundEvent();
                checkedDictionarySoundEvent.Add(soundEvent, checkedDictionaryValueSoundEvent);
            }
            if (!checkedDictionaryValueSoundEvent.savedMaxChecked) {
                checkedDictionaryValueSoundEvent.savedMaxChecked = true;
                checkedDictionaryValueSoundEvent.savedMaxLength = soundEvent.internals.GetMaxLengthWithPitchAndTimeline();
            }
            return checkedDictionaryValueSoundEvent.savedMaxLength;
        }

        public bool InternalCheckTriggerOnTailLengthTooShort(SoundEventBase soundEvent) {
            if (soundEvent == null) {
                return true;
            }
            if (!checkedDictionarySoundEvent.TryGetValue(soundEvent, out checkedDictionaryValueSoundEvent)) {
                checkedDictionaryValueSoundEvent = new CheckedDictionaryValueSoundEvent();
                checkedDictionarySoundEvent.Add(soundEvent, checkedDictionaryValueSoundEvent);
            }
            if (!checkedDictionaryValueSoundEvent.savedMinLengthChecked) {
                checkedDictionaryValueSoundEvent.savedMinLengthChecked = true;
                checkedDictionaryValueSoundEvent.savedMinLength = soundEvent.internals.GetMinLengthFirstSoundContainerWithoutPitch();
                if (checkedDictionaryValueSoundEvent.savedMinLength < soundEvent.internals.data.triggerOnTailLength) {
#if UNITY_EDITOR
                    // So the problem can be fixed in runtime
                    checkedDictionaryValueSoundEvent.savedMinLengthChecked = false;
#endif
                    if (ShouldDebug.Warnings()) {
                        Debug.LogWarning($"Sonity.{nameof(NameOf.SoundEvent)} \"" + soundEvent.name + "\" Trigger On Tail: Tail Length is longer than the shortest AudioClip on the first SoundContainer.", soundEvent);
                    }
                    return true;
                }
            }
            return false;
        }

        public bool InternalCheckTriggerOnPlayIsInfiniteLoop(SoundEventBase soundEvent) {
            if (soundEvent == null) {
                return true;
            }
            if (!checkedDictionarySoundEvent.TryGetValue(soundEvent, out checkedDictionaryValueSoundEvent)) {
                checkedDictionaryValueSoundEvent = new CheckedDictionaryValueSoundEvent();
                checkedDictionarySoundEvent.Add(soundEvent, checkedDictionaryValueSoundEvent);
            }
            if (!checkedDictionaryValueSoundEvent.triggerOnPlayIsInfiniteLoopChecked) {
                checkedDictionaryValueSoundEvent.triggerOnPlayIsInfiniteLoopChecked = true;
                checkedDictionaryValueSoundEvent.triggerOnPlayIsInfiniteLoop =
                    soundEvent.internals.data.GetIfInfiniteLoop(soundEvent, out SoundEventBase infiniteInstigator, out SoundEventBase infinitePrevious, TriggerOnType.TriggerOnPlay);
                if (checkedDictionaryValueSoundEvent.triggerOnPlayIsInfiniteLoop) {
#if UNITY_EDITOR
                    // So the problem can be fixed in runtime
                    checkedDictionaryValueSoundEvent.triggerOnPlayIsInfiniteLoopChecked = false;
#endif
                    if (ShouldDebug.Warnings()) {
                        Debug.LogWarning($"Sonity.{nameof(NameOf.SoundEvent)} TriggerOnPlay: "
                            + "\"" + infiniteInstigator.name + "\" in \"" + infinitePrevious.name + "\" creates an infinite loop", infiniteInstigator);
                    }
                    return true;
                }
            }
            return checkedDictionaryValueSoundEvent.triggerOnPlayIsInfiniteLoop;
        }

        public bool InternalCheckTriggerOnStopIsInfiniteLoop(SoundEventBase soundEvent) {
            if (soundEvent == null) {
                return true;
            }
            if (!checkedDictionarySoundEvent.TryGetValue(soundEvent, out checkedDictionaryValueSoundEvent)) {
                checkedDictionaryValueSoundEvent = new CheckedDictionaryValueSoundEvent();
                checkedDictionarySoundEvent.Add(soundEvent, checkedDictionaryValueSoundEvent);
            }
            if (!checkedDictionaryValueSoundEvent.triggerOnStopIsInfiniteLoopChecked) {
                checkedDictionaryValueSoundEvent.triggerOnStopIsInfiniteLoopChecked = true;
                checkedDictionaryValueSoundEvent.triggerOnStopIsInfiniteLoop =
                    soundEvent.internals.data.GetIfInfiniteLoop(soundEvent, out SoundEventBase infiniteInstigator, out SoundEventBase infinitePrevious, TriggerOnType.TriggerOnStop);
                if (checkedDictionaryValueSoundEvent.triggerOnStopIsInfiniteLoop) {
#if UNITY_EDITOR
                    // So the problem can be fixed in runtime
                    checkedDictionaryValueSoundEvent.triggerOnStopIsInfiniteLoopChecked = false;
#endif
                    if (ShouldDebug.Warnings()) {
                        Debug.LogWarning($"Sonity.{nameof(NameOf.SoundEvent)} TriggerOnStop: "
                            + "\"" + infiniteInstigator.name + "\" in \"" + infinitePrevious.name + "\" creates an infinite loop", infiniteInstigator);
                    }
                    return true;
                }
            }
            return checkedDictionaryValueSoundEvent.triggerOnStopIsInfiniteLoop;
        }

        public bool InternalCheckSoundDataGroupIsInfiniteLoop(SoundDataGroupBase soundDataGroup) {
            if (soundDataGroup == null) {
                return true;
            }
            if (!checkedDictionarySoundDataGroup.TryGetValue(soundDataGroup, out checkedDictionaryValueSoundDataGroup)) {
                checkedDictionaryValueSoundDataGroup = new CheckedDictionaryValueSoundDataGroup();
                checkedDictionarySoundDataGroup.Add(soundDataGroup, checkedDictionaryValueSoundDataGroup);
            }
            if (!checkedDictionaryValueSoundDataGroup.isInfiniteLoopChecked) {
                checkedDictionaryValueSoundDataGroup.isInfiniteLoopChecked = true;
                checkedDictionaryValueSoundDataGroup.isInfiniteLoop = soundDataGroup.internals.GetIfInfiniteLoop(soundDataGroup, out SoundDataGroupBase infiniteInstigator, out SoundDataGroupBase infinitePrevious);
                if (checkedDictionaryValueSoundDataGroup.isInfiniteLoop) {
#if UNITY_EDITOR
                    // So the problem can be fixed in runtime
                    checkedDictionaryValueSoundDataGroup.isInfiniteLoopChecked = false;
#endif
                    if (ShouldDebug.Warnings()) {
                        Debug.LogWarning($"Sonity.{nameof(NameOf.SoundDataGroup)}: "
                            + "\"" + infiniteInstigator.name + "\" in \"" + infinitePrevious.name + "\" creates an infinite loop", infiniteInstigator);
                    }
                }
            }
            return checkedDictionaryValueSoundDataGroup.isInfiniteLoop;
        }

        public bool InternalCheckSoundMixIsInfiniteLoop(SoundMixBase soundMix) {
            if (soundMix == null) {
                return true;
            }
            if (!checkedDictionarySoundMix.TryGetValue(soundMix, out checkedDictionaryValueSoundMix)) {
                checkedDictionaryValueSoundMix = new CheckedDictionaryValueSoundMix();
                checkedDictionarySoundMix.Add(soundMix, checkedDictionaryValueSoundMix);
            }
            if (!checkedDictionaryValueSoundMix.isInfiniteLoopChecked) {
                checkedDictionaryValueSoundMix.isInfiniteLoopChecked = true;
                checkedDictionaryValueSoundMix.isInfiniteLoop = soundMix.internals.GetIfInfiniteLoop(soundMix, out SoundMixBase infiniteInstigator, out SoundMixBase infinitePrevious);
                if (checkedDictionaryValueSoundMix.isInfiniteLoop) {
#if UNITY_EDITOR
                    // So the problem can be fixed in runtime
                    checkedDictionaryValueSoundMix.isInfiniteLoopChecked = false;
#endif
                    if (ShouldDebug.Warnings()) {
                        Debug.LogWarning($"Sonity.{nameof(NameOf.SoundMix)}: "
                            + "\"" + infiniteInstigator.name + "\" in \"" + infinitePrevious.name + "\" creates an infinite loop", infiniteInstigator);
                    }
                }
            }
            return checkedDictionaryValueSoundMix.isInfiniteLoop;
        }

#if UNITY_EDITOR
        public int InternalStatisticsNumberOfPlays(SoundEventBase soundEvent, bool increment) {
            if (soundEvent == null) {
                return 0;
            }
            if (!checkedDictionarySoundEvent.TryGetValue(soundEvent, out checkedDictionaryValueSoundEvent)) {
                checkedDictionaryValueSoundEvent = new CheckedDictionaryValueSoundEvent();
                checkedDictionarySoundEvent.Add(soundEvent, checkedDictionaryValueSoundEvent);
            }
            if (increment) {
                checkedDictionaryValueSoundEvent.statisticsNumberOfPlays++;
            }
            return checkedDictionaryValueSoundEvent.statisticsNumberOfPlays;
        }
#endif

        /// <summary>
        /// <para> Returns the time since the <see cref="SoundEventBase">SoundEvent</see> was played </para>
        /// <para> Calculated using <see cref="Time.realtimeSinceStartup"/> </para>
        /// <para> Returns 0 if the <see cref="SoundEventInstance"/> is not playing </para>
        /// <para> Returns 0 if the <see cref="SoundEventBase">SoundEvent</see> or <see cref="Transform"/> is null </para>
        /// </summary>
        /// <param name="soundEvent"> The <see cref="SoundEventBase">SoundEvent</see> get the time played from </param>
        /// <param name="owner"> The owner <see cref="Transform"/> </param>
        /// <returns> Time in seconds </returns>
        public float InternalGetTimePlayed(SoundEventBase soundEvent, Transform owner) {
            if (soundEvent == null || owner == null) {
                return 0f;
            }
            if (soundEventDictionary.TryGetValue(soundEvent, out tempInstanceDictionaryValue)) {
                if (tempInstanceDictionaryValue.instanceDictionary.TryGetValue(owner.GetInstanceID(), out tempInstance)) {
                    if (tempInstance != null) {
                        return tempInstance.GetTimePlayed();
                    }
                }
            }
            return 0f;
        }

#if UNITY_EDITOR
        [NonSerialized]
        private List<SoundEventBase> soloSoundEvents = new List<SoundEventBase>();

        public bool GetSoloEnabled() {
            for (int i = soloSoundEvents.Count - 1; i >= 0; i--) {
                if (soloSoundEvents[i] != null && soloSoundEvents[i].internals.data.soloEnable) {
                    return true;
                } else {
                    soloSoundEvents.RemoveAt(i);
                }
            }
            return false;
        }

        public bool GetIsInSolo(SoundEventBase soundEvent) {
            if (soloSoundEvents.Contains(soundEvent)) {
                return true;
            }
            return false;
        }

        public void AddSolo(SoundEventBase soundEvent) {
            if (!soloSoundEvents.Contains(soundEvent)) {
                soloSoundEvents.Add(soundEvent);
            }
        }
#endif

        public string DebugInfoString(SoundEventBase soundEvent, Transform transform, Transform position, Transform owner) {
            string tempString = "";
            bool previousNull = true;

            if (soundEvent != null) {
                previousNull = false;
                tempString += " \"" + soundEvent.name + $"\" ({nameof(NameOf.SoundEvent)})";
            } else {
                previousNull = true;
            }
            if (transform != null) {
                if (!previousNull) {
                    tempString += ",";
                }
                previousNull = false;
                tempString += " \"" + transform.name + $"\" (Transform)";
            } else {
                previousNull = true;
            }
            if (position != null) {
                if (!previousNull) {
                    tempString += ",";
                }
                previousNull = false;
                tempString += " \"" + position.name + $"\" (position Transform)";
            } else {
                previousNull = true;
            }
            if (owner != null) {
                if (!previousNull) {
                    tempString += ",";
                }
                previousNull = false;
                tempString += " \"" + owner.name + $"\" (owner Transform)";
            } else {
                previousNull = true;
            }
            // Add first At
            if (tempString != "") {
                tempString = " Using" + tempString + ".";
            }
            return tempString;
        }

        /// <summary>
        /// Plays the <see cref="SoundEventBase">SoundEvent</see> at the <see cref="Transform"/> position
        /// </summary>
        /// <param name='soundEvent'>
        /// The <see cref="SoundEventBase">SoundEvent</see> to play
        /// </param>
        /// <param name='owner'>
        /// The owner <see cref="Transform"/> (can follow positon)
        /// </param>
        public void Play(SoundEventBase soundEvent, Transform owner) {
            if (owner == null) {
                if (ShouldDebug.Warnings()) {
                    Debug.LogWarning($"Sonity.{nameof(NameOf.SoundManager)}: The {nameof(Transform)} is null." + DebugInfoString(soundEvent, owner, null, null), soundEvent);
                }
            } else {
                if (soundEvent == null) {
                    if (ShouldDebug.Warnings()) {
                        Debug.LogWarning($"Sonity.{nameof(NameOf.SoundManager)}: The {nameof(NameOf.SoundEvent)} is null." + DebugInfoString(soundEvent, owner, null, null), owner);
                    }
                } else {
                    InternalPlay(soundEvent, SoundEventPlayType.Play, owner, null, null, null, null, null, null, null);
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
        public void Play(SoundEventBase soundEvent, Transform owner, SoundTagBase localSoundTag) {
            if (owner == null) {
                if (ShouldDebug.Warnings()) {
                    Debug.LogWarning($"Sonity.{nameof(NameOf.SoundManager)}: The {nameof(Transform)} is null." + DebugInfoString(soundEvent, owner, null, null), soundEvent);
                }
            } else {
                if (soundEvent == null) {
                    if (ShouldDebug.Warnings()) {
                        Debug.LogWarning($"Sonity.{nameof(NameOf.SoundManager)}: The {nameof(NameOf.SoundEvent)} is null." + DebugInfoString(soundEvent, owner, null, null), owner);
                    }
                } else {
                    InternalPlay(soundEvent, SoundEventPlayType.Play, owner, null, null, null, null, null, null, localSoundTag);
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
        public void Play(SoundEventBase soundEvent, Transform owner, params SoundParameterInternals[] soundParameters) {
            if (owner == null) {
                if (ShouldDebug.Warnings()) {
                    Debug.LogWarning($"Sonity.{nameof(NameOf.SoundManager)}: The {nameof(Transform)} is null." + DebugInfoString(soundEvent, owner, null, null), soundEvent);
                }
            } else {
                if (soundEvent == null) {
                    if (ShouldDebug.Warnings()) {
                        Debug.LogWarning($"Sonity.{nameof(NameOf.SoundManager)}: The {nameof(NameOf.SoundEvent)} is null." + DebugInfoString(soundEvent, owner, null, null), owner);
                    }
                } else {
                    InternalPlay(soundEvent, SoundEventPlayType.Play, owner, null, null, null, null, soundParameters, null, null);
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
        public void Play(SoundEventBase soundEvent, Transform owner, SoundTagBase localSoundTag, params SoundParameterInternals[] soundParameters) {
            if (owner == null) {
                if (ShouldDebug.Warnings()) {
                    Debug.LogWarning($"Sonity.{nameof(NameOf.SoundManager)}: The {nameof(Transform)} is null." + DebugInfoString(soundEvent, owner, null, null), soundEvent);
                }
            } else {
                if (soundEvent == null) {
                    if (ShouldDebug.Warnings()) {
                        Debug.LogWarning($"Sonity.{nameof(NameOf.SoundManager)}: The {nameof(NameOf.SoundEvent)} is null." + DebugInfoString(soundEvent, owner, null, null), owner);
                    }
                } else {
                    InternalPlay(soundEvent, SoundEventPlayType.Play, owner, null, null, null, null, soundParameters, null, localSoundTag);
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
        public void PlayAtPosition(SoundEventBase soundEvent, Transform owner, Transform position) {
            if (position == null) {
                if (ShouldDebug.Warnings()) {
                    Debug.LogWarning($"Sonity.{nameof(NameOf.SoundManager)}: The position {nameof(Transform)} is null." + DebugInfoString(soundEvent, null, position, owner), owner);
                }
            } else {
                if (owner == null) {
                    if (ShouldDebug.Warnings()) {
                        Debug.LogWarning($"Sonity.{nameof(NameOf.SoundManager)}: The owner {nameof(Transform)} is null." + DebugInfoString(soundEvent, null, position, owner), soundEvent);
                    }
                } else {
                    if (soundEvent == null) {
                        if (ShouldDebug.Warnings()) {
                            Debug.LogWarning($"Sonity.{nameof(NameOf.SoundManager)}: The {nameof(NameOf.SoundEvent)} is null." + DebugInfoString(soundEvent, null, position, owner), owner);
                        }
                    } else {
                        InternalPlay(soundEvent, SoundEventPlayType.PlayAtTransform, owner, null, position, null, null, null, null, null);
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
        public void PlayAtPosition(SoundEventBase soundEvent, Transform owner, Transform position, SoundTagBase localSoundTag) {
            if (position == null) {
                if (ShouldDebug.Warnings()) {
                    Debug.LogWarning($"Sonity.{nameof(NameOf.SoundManager)}: The position {nameof(Transform)} is null." + DebugInfoString(soundEvent, null, position, owner), owner);
                }
            } else {
                if (owner == null) {
                    if (ShouldDebug.Warnings()) {
                        Debug.LogWarning($"Sonity.{nameof(NameOf.SoundManager)}: The owner {nameof(Transform)} is null." + DebugInfoString(soundEvent, null, position, owner), soundEvent);
                    }
                } else {
                    if (soundEvent == null) {
                        if (ShouldDebug.Warnings()) {
                            Debug.LogWarning($"Sonity.{nameof(NameOf.SoundManager)}: The {nameof(NameOf.SoundEvent)} is null." + DebugInfoString(soundEvent, null, position, owner), owner);
                        }
                    } else {
                        InternalPlay(soundEvent, SoundEventPlayType.PlayAtTransform, owner, null, position, null, null, null, null, localSoundTag);
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
        public void PlayAtPosition(SoundEventBase soundEvent, Transform owner, Transform position, params SoundParameterInternals[] soundParameters) {
            if (position == null) {
                if (ShouldDebug.Warnings()) {
                    Debug.LogWarning($"Sonity.{nameof(NameOf.SoundManager)}: The position {nameof(Transform)} is null." + DebugInfoString(soundEvent, null, position, owner), owner);
                }
            } else {
                if (owner == null) {
                    if (ShouldDebug.Warnings()) {
                        Debug.LogWarning($"Sonity.{nameof(NameOf.SoundManager)}: The owner {nameof(Transform)} is null." + DebugInfoString(soundEvent, null, position, owner), soundEvent);
                    }
                } else {
                    if (soundEvent == null) {
                        if (ShouldDebug.Warnings()) {
                            Debug.LogWarning($"Sonity.{nameof(NameOf.SoundManager)}: The {nameof(NameOf.SoundEvent)} is null." + DebugInfoString(soundEvent, null, position, owner), owner);
                        }
                    } else {
                        InternalPlay(soundEvent, SoundEventPlayType.PlayAtTransform, owner, null, position, null, null, soundParameters, null, null);
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
        public void PlayAtPosition(SoundEventBase soundEvent, Transform owner, Transform position, SoundTagBase localSoundTag, params SoundParameterInternals[] soundParameters) {
            if (position == null) {
                if (ShouldDebug.Warnings()) {
                    Debug.LogWarning($"Sonity.{nameof(NameOf.SoundManager)}: The position {nameof(Transform)} is null." + DebugInfoString(soundEvent, null, position, owner), owner);
                }
            } else {
                if (owner == null) {
                    if (ShouldDebug.Warnings()) {
                        Debug.LogWarning($"Sonity.{nameof(NameOf.SoundManager)}: The owner {nameof(Transform)} is null." + DebugInfoString(soundEvent, null, position, owner), soundEvent);
                    }
                } else {
                    if (soundEvent == null) {
                        if (ShouldDebug.Warnings()) {
                            Debug.LogWarning($"Sonity.{nameof(NameOf.SoundManager)}: The {nameof(NameOf.SoundEvent)} is null." + DebugInfoString(soundEvent, null, position, owner), owner);
                        }
                    } else {
                        InternalPlay(soundEvent, SoundEventPlayType.PlayAtTransform, owner, null, position, null, null, soundParameters, null, localSoundTag);
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
        public void PlayAtPosition(SoundEventBase soundEvent, Transform owner, Vector3 position) {
            if (owner == null) {
                if (ShouldDebug.Warnings()) {
                    Debug.LogWarning($"Sonity.{nameof(NameOf.SoundManager)}: The {nameof(Transform)} is null." + DebugInfoString(soundEvent, null, null, owner), soundEvent);
                }
            } else {
                if (soundEvent == null) {
                    if (ShouldDebug.Warnings()) {
                        Debug.LogWarning($"Sonity.{nameof(NameOf.SoundManager)}: The {nameof(NameOf.SoundEvent)} is null." + DebugInfoString(soundEvent, null, null, owner), owner);
                    }
                } else {
                    InternalPlay(soundEvent, SoundEventPlayType.PlayAtVector, owner, position, null, null, null, null, null, null);
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
        public void PlayAtPosition(SoundEventBase soundEvent, Transform owner, Vector3 position, SoundTagBase localSoundTag) {
            if (owner == null) {
                if (ShouldDebug.Warnings()) {
                    Debug.LogWarning($"Sonity.{nameof(NameOf.SoundManager)}: The {nameof(Transform)} is null." + DebugInfoString(soundEvent, null, null, owner), soundEvent);
                }
            } else {
                if (soundEvent == null) {
                    if (ShouldDebug.Warnings()) {
                        Debug.LogWarning($"Sonity.{nameof(NameOf.SoundManager)}: The {nameof(NameOf.SoundEvent)} is null." + DebugInfoString(soundEvent, null, null, owner), owner);
                    }
                } else {
                    InternalPlay(soundEvent, SoundEventPlayType.PlayAtVector, owner, position, null, null, null, null, null, localSoundTag);
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
        public void PlayAtPosition(SoundEventBase soundEvent, Transform owner, Vector3 position, params SoundParameterInternals[] soundParameters) {
            if (owner == null) {
                if (ShouldDebug.Warnings()) {
                    Debug.LogWarning($"Sonity.{nameof(NameOf.SoundManager)}: The {nameof(Transform)} is null." + DebugInfoString(soundEvent, null, null, owner), soundEvent);
                }
            } else {
                if (soundEvent == null) {
                    if (ShouldDebug.Warnings()) {
                        Debug.LogWarning($"Sonity.{nameof(NameOf.SoundManager)}: The {nameof(NameOf.SoundEvent)} is null." + DebugInfoString(soundEvent, null, null, owner), owner);
                    }
                } else {
                    InternalPlay(soundEvent, SoundEventPlayType.PlayAtVector, owner, position, null, null, null, soundParameters, null, null);
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
        public void PlayAtPosition(SoundEventBase soundEvent, Transform owner, Vector3 position, SoundTagBase localSoundTag, params SoundParameterInternals[] soundParameters) {
            if (owner == null) {
                if (ShouldDebug.Warnings()) {
                    Debug.LogWarning($"Sonity.{nameof(NameOf.SoundManager)}: The {nameof(Transform)} is null" + DebugInfoString(soundEvent, null, null, owner), soundEvent);
                }
            } else {
                if (soundEvent == null) {
                    if (ShouldDebug.Warnings()) {
                        Debug.LogWarning($"Sonity.{nameof(NameOf.SoundManager)}: The {nameof(NameOf.SoundEvent)} is null" + DebugInfoString(soundEvent, null, null, owner), owner);
                    }
                } else {
                    InternalPlay(soundEvent, SoundEventPlayType.PlayAtVector, owner, position, null, null, null, soundParameters, null, localSoundTag);
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
        public void Stop(SoundEventBase soundEvent, Transform owner, bool allowFadeOut = true) {
            if (owner == null) {
                if (ShouldDebug.Warnings()) {
                    Debug.LogWarning($"Sonity.{nameof(NameOf.SoundManager)}: The {nameof(Transform)} is null" + DebugInfoString(soundEvent, owner, null, null), soundEvent);
                }
            } else {
                if (soundEvent == null) {
                    if (ShouldDebug.Warnings()) {
                        Debug.LogWarning($"Sonity.{nameof(NameOf.SoundManager)}: The {nameof(NameOf.SoundEvent)} is null" + DebugInfoString(soundEvent, owner, null, null), owner);
                    }
                } else {
                    InternalStop(soundEvent, owner, allowFadeOut);
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
        public void StopAtPosition(SoundEventBase soundEvent, Transform position, bool allowFadeOut = true) {
            if (position == null) {
                if (ShouldDebug.Warnings()) {
                    Debug.LogWarning($"Sonity.{nameof(NameOf.SoundManager)}: The {nameof(Transform)} is null" + DebugInfoString(soundEvent, position, null, null), soundEvent);
                }
            } else {
                if (soundEvent == null) {
                    if (ShouldDebug.Warnings()) {
                        Debug.LogWarning($"Sonity.{nameof(NameOf.SoundManager)}: The {nameof(NameOf.SoundEvent)} is null" + DebugInfoString(soundEvent, position, null, null), position);
                    }
                } else {
                    InternalStopAtPosition(soundEvent, position, allowFadeOut);
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
                    Debug.LogWarning($"Sonity.{nameof(NameOf.SoundManager)}: The {nameof(Transform)} is null" + DebugInfoString(null, owner, null, null));
                }
            } else {
                InternalStopAllAtOwner(owner, allowFadeOut);
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
        public void StopEverywhere(SoundEventBase soundEvent, bool allowFadeOut = true) {
            if (soundEvent == null) {
                if (ShouldDebug.Warnings()) {
                    Debug.LogWarning($"Sonity.{nameof(NameOf.SoundManager)}: The {nameof(NameOf.SoundEvent)} is null" + DebugInfoString(soundEvent, null, null, null));
                }
            } else {
                InternalStopEverywhere(soundEvent, allowFadeOut);
            }
        }

        /// <summary>
        /// Stops the all <see cref="SoundEventBase">SoundEvents</see>
        /// </summary>
        /// <param name='allowFadeOut'>
        /// If the <see cref="SoundEventBase">SoundEvents</see> should be allowed to fade out. Otherwise they are going to be stopped immediately
        /// </param>
        public void StopEverything(bool allowFadeOut = true) {
            InternalStopEverything(allowFadeOut);
        }

        /// <summary>
        /// Plays the <see cref="SoundEventBase">SoundEvent</see> with the 2D <see cref="Transform"/> as owner
        /// Useful to play e.g. UI or other 2D sounds without having to pass a <see cref="Transform"/>
        /// To make the sound 2D you still need to disable distance and set spatial blend to 0 in the <see cref="SoundContainerBase">SoundContainer</see>
        /// </summary>
        /// <param name='soundEvent'>
        /// The <see cref="SoundEventBase">SoundEvent</see> to play
        /// </param>
        public void Play2D(SoundEventBase soundEvent) {
            Play(soundEvent, cached2DTransform);
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
        public void Play2D(SoundEventBase soundEvent, SoundTagBase localSoundTag) {
            Play(soundEvent, cached2DTransform, localSoundTag);
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
        public void Play2D(SoundEventBase soundEvent, params SoundParameterInternals[] soundParameters) {
            Play(soundEvent, cached2DTransform, soundParameters);
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
        public void Play2D(SoundEventBase soundEvent, SoundTagBase localSoundTag, params SoundParameterInternals[] soundParameters) {
            Play(soundEvent, cached2DTransform, localSoundTag, soundParameters);
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
        public void Stop2D(SoundEventBase soundEvent, bool allowFadeOut = true) {
            Stop(soundEvent, cached2DTransform, allowFadeOut);
        }

        /// <summary>
        /// Stops all <see cref="SoundEventBase">SoundEvents</see> at the 2D <see cref="Transform"/>
        /// </summary>
        /// <param name='allowFadeOut'>
        /// If the <see cref="SoundEventBase">SoundEvent</see> should be allowed to fade out. Otherwise it is going to be stopped immediately
        /// </param>
        public void StopAllAt2D(bool allowFadeOut = true) {
            StopAllAtOwner(cached2DTransform, allowFadeOut);
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
        public void PlayMusic(SoundEventBase soundEvent, bool stopAllOtherMusic = true, bool allowFadeOut = true) {
            if (stopAllOtherMusic) {
                StopAllMusic(allowFadeOut);
            }
            Play(soundEvent, cachedMusicTransform);
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
        public void PlayMusic(SoundEventBase soundEvent, bool stopAllOtherMusic = true, bool allowFadeOut = true, params SoundParameterInternals[] soundParameters) {
            if (stopAllOtherMusic) {
                StopAllMusic(allowFadeOut);
            }
            Play(soundEvent, cachedMusicTransform, soundParameters);
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
        public void StopMusic(SoundEventBase soundEvent, bool allowFadeOut = true) {
            Stop(soundEvent, cachedMusicTransform, allowFadeOut);
        }

        /// <summary>
        /// Stops the all <see cref="SoundEventBase">SoundEvents</see> playing at the <see cref="SoundManagerBase">SoundManagers</see> music <see cref="Transform"/>
        /// </summary>
        /// <param name='allowFadeOut'>
        /// If the <see cref="SoundEventBase">SoundEvent</see> should be allowed to fade out. Otherwise it is going to be stopped immediately
        /// </param>
        public void StopAllMusic(bool allowFadeOut = true) {
            StopAllAtOwner(cachedMusicTransform, allowFadeOut);
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
        public SoundEventState GetSoundEventState(SoundEventBase soundEvent, Transform owner) {
            return InternalGetSoundEventState(soundEvent, owner);
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
        public float GetLastPlayedClipLength(SoundEventBase soundEvent, Transform owner, bool pitchSpeed) {
            return InternalGetLastPlayedClipLength(soundEvent, owner, pitchSpeed);
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
        public float GetLastPlayedClipTime(SoundEventBase soundEvent, Transform owner, bool pitchSpeed) {
            return InternalGetLastPlayedClipTime(soundEvent, owner, pitchSpeed);
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
        public void GetSpectrumData(SoundEventBase soundEvent, Transform owner, ref float[] samples, int channel, FFTWindow window, SpectrumDataFrom spectrumDataFrom) {
            InternalGetSpectrumData(soundEvent, owner, ref samples, channel, window, spectrumDataFrom);
        }

        /// <summary>
        /// <para> Returns the max length (in seconds) of the <see cref="SoundEventBase">SoundEvent</see> (calculated from the longest audioClip) </para>
        /// <para> Is scaled by the pitch of the <see cref="SoundEventBase">SoundEvent</see> and <see cref="SoundContainerBase">SoundContainer</see> </para>
        /// <para> Does not take into account random, intensity or parameter pitch </para>
        /// <para> Returns 0 if the <see cref="SoundEventBase">SoundEvent</see> is null </para>
        /// </summary>
        /// <param name="soundEvent"> The <see cref="SoundEventBase">SoundEvent</see> get the length from </param>
        /// <returns> The max length in seconds </returns>
        public float GetMaxLength(SoundEventBase soundEvent) {
            return InternalGetMaxLength(soundEvent);
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
        public float GetTimePlayed(SoundEventBase soundEvent, Transform owner) {
            return InternalGetTimePlayed(soundEvent, owner);
        }

        /// <summary>
        /// Sets the global <see cref="SoundTagBase">SoundTag</see>
        /// </summary>
        /// <param name='soundTag'>
        /// The <see cref="SoundTagBase">SoundTag</see> to change to
        /// </param>
        public void SetGlobalSoundTag(SoundTagBase soundTag) {
            settings.globalSoundTag = soundTag;
        }

        /// <summary>
        /// Returns the global <see cref="SoundTagBase">SoundTag</see>
        /// </summary>
        /// <returns> The global <see cref="SoundTagBase">SoundTag</see> </returns>
        public SoundTagBase GetGlobalSoundTag() {
            return settings.globalSoundTag;
        }

        /// <summary>
        /// Sets the global distance scale (default is a scale of 100 units)
        /// </summary>
        /// <param name='distanceScale'>
        /// The new range scale
        /// </param>
        public void SetGlobalDistanceScale(float distanceScale) {
            settings.distanceScale = Mathf.Clamp(distanceScale, 0f, Mathf.Infinity);
        }

        /// <summary>
        /// Returns the global distance scale
        /// </summary>
        public float GetGlobalDistanceScale() {
            return settings.distanceScale;
        }

        /// <summary>
        /// Set if speed of sound should be enabled
        /// </summary>
        /// <param name='speedOfSoundEnabled'>
        /// Should speed of Sound be active
        /// </param>
        public void SetSpeedOfSoundEnabled(bool speedOfSoundEnabled) {
            settings.speedOfSoundEnabled = speedOfSoundEnabled;
        }

        /// <summary>
        /// <para> Set the speed of sound scale </para>
        /// <para> The default is a multiplier of 1 (by the base value of 340 unity units per second) </para>
        /// </summary>
        /// <param name='speedOfSoundScale'>
        /// The scale of speed of Sound (default is a multipler of 1)
        /// </param>
        public void SetSpeedOfSoundScale(float speedOfSoundScale) {
            settings.speedOfSoundScale = Mathf.Clamp(speedOfSoundScale, 0f, Mathf.Infinity);
        }

        /// <summary>
        /// Returns the speed of sound scale
        /// </summary>
        public float GetSpeedOfSoundScale() {
            return settings.speedOfSoundScale;
        }

        /// <summary>
        /// Sets the <see cref="Voice"/> limit
        /// </summary>
        /// <param name='voiceLimit'>
        /// The maximum number of <see cref="Voice"/>s which can be played at the same time
        /// </param>
        public void SetVoiceLimit(int voiceLimit) {
            settings.voiceLimit = Mathf.Clamp(voiceLimit, 0, int.MaxValue);
        }

        /// <summary>
        /// Returns the <see cref="Voice"/> limit
        /// </summary>
        public int GetVoiceLimit() {
            return settings.voiceLimit;
        }

        /// <summary>
        /// Sets the <see cref="VoiceEffect"/> limit
        /// </summary>
        public void SetVoiceEffectLimit(int voiceEffectLimit) {
            settings.voiceEffectLimit = Mathf.Clamp(voiceEffectLimit, 0, int.MaxValue);
        }

        /// <summary>
        /// Returns the <see cref="VoiceEffect"/> limit
        /// </summary>
        public int GetVoiceEffectLimit() {
            return settings.voiceEffectLimit;
        }

        /// <summary>
        /// Disables/enables all the Play/PlayAtPosition functionality
        /// </summary>
        /// <param name="disablePlayingSounds"></param>
        public void SetDisablePlayingSounds(bool disablePlayingSounds) {
            settings.disablePlayingSounds = disablePlayingSounds;
        }

        /// <summary>
        /// If the Play/PlayAtPosition functionality is disabled
        /// </summary>
        public bool GetDisablePlayingSounds() {
            return settings.disablePlayingSounds;
        }
    }
}