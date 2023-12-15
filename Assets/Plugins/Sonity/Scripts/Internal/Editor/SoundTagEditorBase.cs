﻿// Created by Victor Engström
// Copyright 2023 Sonigon AB
// http://www.sonity.org/

#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;

namespace Sonity.Internal {

    public abstract class SoundTagEditorBase : Editor {

        public SoundTagBase mTarget;
        public SoundTagBase[] mTargets;

        public float pixelsPerIndentLevel = 10f;

        public Color defaultGuiColor;
        public GUIStyle guiStyleBoldCenter = new GUIStyle();

        public void StartBackgroundColor(Color color) {
            GUI.color = color;
            EditorGUILayout.BeginVertical("Button");
            GUI.color = defaultGuiColor;
        }

        public void StopBackgroundColor() {
            EditorGUILayout.EndVertical();
        }

        public override void OnInspectorGUI() {

            mTarget = (SoundTagBase)target;

            mTargets = new SoundTagBase[targets.Length];
            for (int i = 0; i < targets.Length; i++) {
                mTargets[i] = (SoundTagBase)targets[i];
            }

            defaultGuiColor = GUI.color;

            guiStyleBoldCenter.fontSize = 16;
            guiStyleBoldCenter.fontStyle = FontStyle.Bold;
            guiStyleBoldCenter.alignment = TextAnchor.MiddleCenter;
            if (EditorGUIUtility.isProSkin) {
                guiStyleBoldCenter.normal.textColor = EditorColorProSkin.GetDarkSkinTextColor();
            }

            EditorGUI.indentLevel = 0;

            StartBackgroundColor(Color.white);
            if (GUILayout.Button(new GUIContent($"Sonity" + EditorTrial.trialText + $" - {nameof(NameOf.SoundTag)}\n{mTarget.name}", EditorTextSoundTag.soundTagTooltip), guiStyleBoldCenter, GUILayout.ExpandWidth(true), GUILayout.Height(40))) {
                EditorGUIUtility.PingObject(target);
            }
            StopBackgroundColor();

            EditorTrial.InfoText();
        }
    }
}
#endif