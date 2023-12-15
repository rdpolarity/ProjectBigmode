// Created by Victor Engström
// Copyright 2023 Sonigon AB
// http://www.sonity.org/

using UnityEngine;
using System;
using Sonity.Internal;

namespace Sonity {

    /// <summary>
    /// <see cref="SoundPhysicsBase">SoundPhysics</see> is a component used for easily playing <see cref="SoundEventBase">SoundEvents</see> on physics collisions and friction.
    /// Use intensity record in the <see cref="SoundContainerBase">SoundContainer</see> for easy scaling of the velocity into a 0 to 1 range.
    /// All <see cref="SoundPhysicsBase">SoundPhysics</see> components are multi-object editable.
    /// </summary>
    [Serializable]
    [AddComponentMenu("Sonity/Sonity - Sound Physics")]
    public class SoundPhysics : SoundPhysicsBase {

    }
}