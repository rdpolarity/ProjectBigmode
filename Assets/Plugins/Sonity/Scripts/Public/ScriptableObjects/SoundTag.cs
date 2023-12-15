﻿// Created by Victor Engström
// Copyright 2023 Sonigon AB
// http://www.sonity.org/

using UnityEngine;
using System;
using Sonity.Internal;

namespace Sonity {

    /// <summary>
    /// <see cref="SoundTagBase">SoundTag</see> objects are passed to modify how a <see cref="SoundEventBase">SoundEvent</see> should be played.
    /// You can either pass them when playing a <see cref="SoundEventBase">SoundEvent</see> for setting the local <see cref="SoundTagBase">SoundTag</see>.
    /// Or you can set the global <see cref="SoundTagBase">SoundTag</see> in the <see cref="SoundManagerBase">SoundManager</see>.
    /// This is useful for e.g; weapon reverb zones.
    /// You can set the <see cref="SoundTagBase">SoundTag</see> corresponding to the acoustic space which the listener is in.
    /// And when you play the <see cref="SoundEventBase">SoundEvent</see>, your gun reflection layers can correspond to the acoustic space.
    /// </summary>
    [Serializable]
    [CreateAssetMenu(fileName = "_TAG", menuName = "Sonity/SoundTag", order = 5)]
    public class SoundTag : SoundTagBase {

    }
}