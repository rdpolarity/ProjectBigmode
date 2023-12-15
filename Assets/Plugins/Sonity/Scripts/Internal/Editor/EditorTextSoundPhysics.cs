// Created by Victor Engström
// Copyright 2023 Sonigon AB
// http://www.sonity.org/

#if UNITY_EDITOR

using UnityEngine;

namespace Sonity.Internal {

    public class EditorTextSoundPhysics {

        public static string soundPhysicsTooltip =
            $"{nameof(NameOf.SoundPhysics)} is a component used for easily playing {nameof(NameOf.SoundEvent)}s on physics collisions and friction" + "\n" +
            "\n" +
            $"A {nameof(Rigidbody)} or {nameof(Rigidbody2D)} is required on this object." + "\n" +
            "\n" +
            $"One or several {nameof(Collider)} or {nameof(Collider2D)} should be placed on this object or its children." + "\n" +
            "\n" +
            $"Use intensity record in the {nameof(NameOf.SoundEvent)} for easy scaling of the velocity into a 0 to 1 range." + "\n" +
            "\n" +
            $"All {nameof(NameOf.SoundPhysics)} components are multi-object editable." + EditorTrial.trialTooltip;

        public static string physicsDimensionLabel = $"Collision Type";
        public static string physicsDimensionTooltip = 
            $"If 3D is selected a {nameof(Rigidbody)} and {nameof(Collider)} is required." + "\n" +
            "\n" +
            $"If 2D is selected a {nameof(Rigidbody2D)} and {nameof(Collider2D)} is required." + EditorTrial.trialTooltip;

        public static string warningRigidbody3D = $"{nameof(Rigidbody)} required";
        public static string warningRigidbody2D = $"{nameof(Rigidbody2D)} required";

        public static string impactHeaderLabel = $"Impact";
        public static string impactHeaderTooltip = $"Impacts are played per contact point." + EditorTrial.trialTooltip;

        public static string impactSoundEventLabel = $"{nameof(NameOf.SoundEvent)}";
        public static string impactSoundEventTooltip = $"The {nameof(NameOf.SoundEvent)} which is played on impact." + EditorTrial.trialTooltip;

        public static string impactCollisionTagLabel = $"Collision Tag";
        public static string impactCollisionTagTooltip = $"If enabled the {nameof(NameOf.SoundEvent)}s will only play on matching tags." + EditorTrial.trialTooltip;

        public static string frictionHeaderLabel = $"Friction";
        public static string frictionHeaderTooltip = $"Friction is played when touching another object." + EditorTrial.trialTooltip;

        public static string frictionSoundEventLabel = $"{nameof(NameOf.SoundEvent)}";
        public static string frictionSoundEventTooltip = $"The {nameof(NameOf.SoundEvent)} which is played on friction." + EditorTrial.trialTooltip;

        public static string frictionCollisionTagLabel = $"Collision Tag";
        public static string frictionCollisionTagTooltip = $"If enabled the {nameof(NameOf.SoundEvent)}s will only play on matching tags." + EditorTrial.trialTooltip;
    }
}
#endif