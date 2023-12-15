// Created by Victor Engstr�m
// Copyright 2023 Sonigon AB
// http://www.sonity.org/

using UnityEngine;
using System;
using Sonity.Internal;

namespace Sonity {

    /// <summary>
    /// <see cref="SoundContainerBase">SoundContainers</see> are the building blocks of Sonity.
    /// They contain <see cref="AudioClip"/>s and options of how the sound should be played.
    /// All <see cref="SoundContainerBase">SoundContainers</see> are multi-object editable.
    /// </summary
    [Serializable]
    [CreateAssetMenu(fileName = "_SC", menuName = "Sonity/SoundContainer", order = 1)]
    public class SoundContainer : SoundContainerBase {

    }
}