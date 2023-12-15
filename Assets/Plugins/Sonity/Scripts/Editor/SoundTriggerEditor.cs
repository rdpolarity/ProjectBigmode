// Created by Victor Engstr�m
// Copyright 2023 Sonigon AB
// http://www.sonity.org/

#if UNITY_EDITOR

using UnityEditor;

namespace Sonity.Internal {

    [CustomEditor(typeof(SoundTrigger))]
    [CanEditMultipleObjects]
    public class SoundTriggerEditor : SoundTriggerEditorBase {

    }
}
#endif