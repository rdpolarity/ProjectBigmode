﻿// Created by Victor Engström
// Copyright 2023 Sonigon AB
// http://www.sonity.org/

#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;

namespace Sonity.Internal {

    public abstract class SoundDataGroupEditorBase : Editor {

        public float pixelsPerIndentLevel = 10f;

        public SoundDataGroupBase mTarget;
        public SoundDataGroupBase[] mTargets;

        public SerializedProperty internals;
        public SerializedProperty soundDataGroupChildren;
        public SerializedProperty soundEvents;

        public void OnEnable() {
            FindProperties();
        }

        public void FindProperties() {
            internals = serializedObject.FindProperty(nameof(SoundDataGroupBase.internals));
            soundDataGroupChildren = internals.FindPropertyRelative(nameof(SoundDataGroupInternals.soundDataGroupChildren));
            soundEvents = internals.FindPropertyRelative(nameof(SoundDataGroupInternals.soundEvents));
        }

        public void BeginChange() {
            serializedObject.Update();
            EditorGUI.BeginChangeCheck();
        }

        public void EndChange() {
            if (EditorGUI.EndChangeCheck()) {
                serializedObject.ApplyModifiedProperties();
            }
        }

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

        private void DragAndDropCallback<T>(T[] draggedObjects) where T : UnityEngine.Object {
            SoundEventBase[] newObjects = draggedObjects as SoundEventBase[];
            // If there are any objects of the right type dragged
            for (int i = 0; i < mTargets.Length; i++) {
                Undo.RecordObject(mTargets[i], $"Drag and Dropped {nameof(NameOf.SoundEvent)}");
                mTargets[i].internals.soundEvents = new SoundEventBase[newObjects.Length];
                for (int ii = 0; ii < newObjects.Length; ii++) {
                    mTargets[i].internals.soundEvents[ii] = newObjects[ii];
                }
                // Expands the SoundEvent array
                soundEvents.isExpanded = true;
                EditorUtility.SetDirty(mTargets[i]);
            }
        }

        public override void OnInspectorGUI() {

            mTarget = (SoundDataGroupBase)target;

            mTargets = new SoundDataGroupBase[targets.Length];
            for (int i = 0; i < targets.Length; i++) {
                mTargets[i] = (SoundDataGroupBase)targets[i];
            }

            defaultGuiColor = GUI.color;

            guiStyleBoldCenter.fontSize = 16;
            guiStyleBoldCenter.fontStyle = FontStyle.Bold;
            guiStyleBoldCenter.alignment = TextAnchor.MiddleCenter;
            if (EditorGUIUtility.isProSkin) {
                guiStyleBoldCenter.normal.textColor = EditorColorProSkin.GetDarkSkinTextColor();
            }

            EditorGUI.indentLevel = 1;

            StartBackgroundColor(Color.white);
            if (GUILayout.Button(new GUIContent($"Sonity" + EditorTrial.trialText + $" - {nameof(NameOf.SoundDataGroup)}\n{mTarget.name}", EditorTextSoundDataGroup.soundDataGroupTooltip), guiStyleBoldCenter, GUILayout.ExpandWidth(true), GUILayout.Height(40))) {
                EditorGUIUtility.PingObject(target);
            }
            StopBackgroundColor();

            EditorTrial.InfoText();

            EditorGUILayout.Separator();

            // SoundDataGroup Child Array
            StartBackgroundColor(EditorColor.GetSettings(EditorColorProSkin.GetCustomEditorBackgroundAlpha()));

            EditorGUI.indentLevel = 0;
            EditorGUILayout.LabelField(new GUIContent(EditorTextSoundDataGroup.childSoundDataGroupsLabel, EditorTextSoundDataGroup.childSoundDataGroupsTooltip), EditorStyles.boldLabel);
            
            int lowestSoundDataGroupArrayLength = int.MaxValue;
            for (int n = 0; n < mTargets.Length; n++) {
                if (lowestSoundDataGroupArrayLength > mTargets[n].internals.soundDataGroupChildren.Length) {
                    lowestSoundDataGroupArrayLength = mTargets[n].internals.soundDataGroupChildren.Length;
                }
            }
            EditorGUI.indentLevel = 1;
            EditorGuiFunction.DrawReordableArray(soundDataGroupChildren, serializedObject, lowestSoundDataGroupArrayLength, false);

            EditorGUILayout.Separator();

            // Check for Infinite Loop
            // So that if soundDataGroupChildren is resized it wont get error when checking
            if (ShouldDebug.GuiWarnings()) {
                if (Event.current.type != EventType.DragPerform) {
                    if (mTarget.internals.GetIfInfiniteLoop(mTarget, out SoundDataGroupBase infiniteInstigator, out SoundDataGroupBase infinitePrevious)) {
                        EditorGUILayout.HelpBox("\"" + infiniteInstigator.name + "\" in \"" + infinitePrevious.name + "\" creates an infinite loop", MessageType.Error);
                        EditorGUILayout.Separator();
                    }
                }
            }

            EditorGUI.indentLevel = 0;
            EditorGUILayout.LabelField(new GUIContent(EditorTextSoundDataGroup.soundEventsLabel, EditorTextSoundDataGroup.soundEventsTooltip), EditorStyles.boldLabel);

            // Sound Event Array
            int lowestSoundEventArrayLength = int.MaxValue;
            for (int n = 0; n < mTargets.Length; n++) {
                if (lowestSoundEventArrayLength > mTargets[n].internals.soundEvents.Length) {
                    lowestSoundEventArrayLength = mTargets[n].internals.soundEvents.Length;
                }
            }
            EditorGUI.indentLevel = 1;
            EditorGuiFunction.DrawReordableArray(soundEvents, serializedObject, lowestSoundEventArrayLength, false);

            // SoundEvent Drag and Drop Area
            EditorDragAndDropArea.DrawDragAndDropAreaCustomEditor<SoundEventBase>(new EditorDragAndDropArea.DragAndDropAreaInfo($"{nameof(NameOf.SoundEvent)}"), DragAndDropCallback);

            StopBackgroundColor();
            EditorGUILayout.Separator();

            EditorGUI.indentLevel = 0;
            // Transparent background so the offset will be right
            StartBackgroundColor(new Color(0f, 0f, 0f, 0f));
            EditorGUILayout.BeginHorizontal();
            // For offsetting the buttons to the right
            EditorGUILayout.LabelField(new GUIContent(""), GUILayout.Width(EditorGUIUtility.labelWidth));

            // Reset
            BeginChange();
            if (GUILayout.Button("Reset All")) {
                for (int i = 0; i < mTargets.Length; i++) {
                    Undo.RecordObject(mTargets[i], "Reset All");
                    mTargets[i].internals.soundDataGroupChildren = new SoundDataGroupBase[0];
                    mTargets[i].internals.soundEvents = new SoundEventBase[1];
                    EditorUtility.SetDirty(mTargets[i]);
                }
            }
            EndChange();
            EditorGUILayout.EndHorizontal();
            StopBackgroundColor();
        }
    }
}
#endif