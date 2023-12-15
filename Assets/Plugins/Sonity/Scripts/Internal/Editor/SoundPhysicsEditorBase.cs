// Created by Victor Engström
// Copyright 2023 Sonigon AB
// http://www.sonity.org/

#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;

namespace Sonity.Internal {

    public abstract class SoundPhysicsEditorBase : Editor {

        public SoundPhysicsBase mTarget;
        public SoundPhysicsBase[] mTargets;

        public SerializedProperty internals;
        public SerializedProperty physicsDimensions;
        public SerializedProperty impactSoundPhysicsParts;
        public SerializedProperty frictionSoundPhysicsParts;

        private void OnEnable() {
            FindProperties();
        }

        private void FindProperties() {
            internals = serializedObject.FindProperty(nameof(SoundPhysicsBase.internals));
            physicsDimensions = internals.FindPropertyRelative(nameof(SoundPhysicsBase.internals.physicsDimension));
            impactSoundPhysicsParts = internals.FindPropertyRelative(nameof(SoundPhysicsBase.internals.impactSoundPhysicsParts));
            frictionSoundPhysicsParts = internals.FindPropertyRelative(nameof(SoundPhysicsBase.internals.frictionSoundPhysicsParts));
        }

        private GUIStyle guiStyleBoldCenter = new GUIStyle();
        private Color defaultGuiColor;

        private void BeginChange() {
            serializedObject.Update();
            EditorGUI.BeginChangeCheck();
        }

        private void EndChange() {
            if (EditorGUI.EndChangeCheck()) {
                serializedObject.ApplyModifiedProperties();
            }
        }

        private void StartBackgroundColor(Color color) {
            GUI.color = color;
            EditorGUILayout.BeginVertical("Button");
            GUI.color = defaultGuiColor;
        }

        private void StopBackgroundColor() {
            EditorGUILayout.EndVertical();
        }

        public override void OnInspectorGUI() {

            mTarget = (SoundPhysicsBase)target;

            mTargets = new SoundPhysicsBase[targets.Length];
            for (int i = 0; i < targets.Length; i++) {
                mTargets[i] = (SoundPhysicsBase)targets[i];
            }

            defaultGuiColor = GUI.color;

            EditorGUI.indentLevel = 0;

            guiStyleBoldCenter.fontStyle = FontStyle.Bold;
            guiStyleBoldCenter.alignment = TextAnchor.MiddleCenter;
            if (EditorGUIUtility.isProSkin) {
                guiStyleBoldCenter.normal.textColor = EditorColorProSkin.GetDarkSkinTextColor();
            }

            EditorGuiFunction.DrawLayoutObjectTitle($"Sonity" + EditorTrial.trialText + $" - {nameof(NameOf.SoundPhysics)}", EditorTextSoundPhysics.soundPhysicsTooltip);

            EditorTrial.InfoText();

            EditorGUILayout.Separator();

            if (ShouldDebug.GuiWarnings()) {
                if (physicsDimensions.enumValueIndex == (int)PhysicsDimension._3D) {
                    // 3D
                    if (mTarget.gameObject.GetComponent<Rigidbody>() == null) {
                        EditorGUILayout.HelpBox(EditorTextSoundPhysics.warningRigidbody3D, MessageType.Warning);
                        EditorGUILayout.Separator();
                    }
                } else if (physicsDimensions.enumValueIndex == (int)PhysicsDimension._2D) {
                    // 2D
                    if (mTarget.gameObject.GetComponent<Rigidbody2D>() == null) {
                        EditorGUILayout.HelpBox(EditorTextSoundPhysics.warningRigidbody2D, MessageType.Warning);
                        EditorGUILayout.Separator();
                    }
                }
            }

            BeginChange();
            EditorGUILayout.PropertyField(physicsDimensions, new GUIContent(EditorTextSoundPhysics.physicsDimensionLabel, EditorTextSoundPhysics.physicsDimensionTooltip));
            EndChange();

            // Impact
            EditorGUILayout.Separator();
            SerializedProperty impactExpand = internals.FindPropertyRelative(nameof(SoundPhysicsInternals.impactExpand));
            BeginChange();
            EditorGuiFunction.DrawFoldout(impactExpand, EditorTextSoundPhysics.impactHeaderLabel, EditorTextSoundPhysics.impactHeaderTooltip);
            EndChange();

            if (impactExpand.boolValue) {

                // Transparent background so the offset will be right
                StartBackgroundColor(new Color(0f, 0f, 0f, 0f));
                SerializedProperty impactPlay = internals.FindPropertyRelative(nameof(SoundPhysicsInternals.impactPlay));
                BeginChange();
                EditorGUILayout.PropertyField(impactPlay, new GUIContent("Play Impact"));
                EndChange();
                StopBackgroundColor();

                if (impactPlay.boolValue) {
                    for (int i = 0; i < impactSoundPhysicsParts.arraySize; i++) {
                        SerializedProperty part = impactSoundPhysicsParts.GetArrayElementAtIndex(i);

                        StartBackgroundColor(EditorColor.ChangeHue(EditorColor.GetSettings(EditorColorProSkin.GetCustomEditorBackgroundAlpha()), i * -0.075f));
                        SerializedProperty soundEvent = part.FindPropertyRelative(nameof(SoundPhysicsPart.soundEvent));
                        BeginChange();
                        EditorGUILayout.PropertyField(soundEvent, new GUIContent(EditorTextSoundPhysics.impactSoundEventLabel, EditorTextSoundPhysics.impactSoundEventTooltip));
                        EndChange();
                        SerializedProperty collisionTagUse = part.FindPropertyRelative(nameof(SoundPhysicsPart.collisionTagUse));
                        BeginChange();
                        EditorGUILayout.PropertyField(collisionTagUse, new GUIContent(EditorTextSoundPhysics.impactCollisionTagLabel, EditorTextSoundPhysics.impactCollisionTagTooltip));
                        EndChange();

                        if (collisionTagUse.boolValue) {
                            SerializedProperty collisionTags = part.FindPropertyRelative(nameof(SoundPhysicsPart.collisionTags));
                            for (int ii = 0; ii < collisionTags.arraySize; ii++) {
                                EditorGUILayout.BeginHorizontal();
                                EditorGUILayout.BeginHorizontal();
                                // For offsetting the buttons to the right
                                EditorGUILayout.LabelField(new GUIContent(""), GUILayout.Width(EditorGUIUtility.labelWidth));
                                SerializedProperty tag = collisionTags.GetArrayElementAtIndex(ii);
                                BeginChange();
                                tag.stringValue = EditorGUILayout.TagField(tag.stringValue);
                                EndChange();
                                BeginChange();
                                if (GUILayout.Button("+")) {
                                    collisionTags.InsertArrayElementAtIndex(ii);
                                }
                                EndChange();
                                BeginChange();
                                if (GUILayout.Button("-")) {
                                    if (collisionTags.arraySize > 1) {
                                        collisionTags.DeleteArrayElementAtIndex(ii);
                                    }
                                }
                                EndChange();
                                BeginChange();
                                if (GUILayout.Button("↑")) {
                                    collisionTags.MoveArrayElement(ii, Mathf.Clamp(ii - 1, 0, collisionTags.arraySize));
                                }
                                EndChange();
                                BeginChange();
                                if (GUILayout.Button("↓")) {
                                    collisionTags.MoveArrayElement(ii, Mathf.Clamp(ii + 1, 0, collisionTags.arraySize));
                                }
                                EndChange();
                                EditorGUILayout.EndHorizontal();
                                EditorGUILayout.EndHorizontal();
                            }
                        }
                        StopBackgroundColor();
                        EditorGUILayout.Separator();
                    }

                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.BeginHorizontal();
                    // For offsetting the buttons to the right
                    EditorGUILayout.LabelField(new GUIContent(""), GUILayout.Width(EditorGUIUtility.labelWidth));
                    BeginChange();
                    if (GUILayout.Button("+")) {
                        impactSoundPhysicsParts.arraySize++;
                    }
                    EndChange();
                    BeginChange();
                    if (GUILayout.Button("-")) {
                        if (impactSoundPhysicsParts.arraySize > 1) {
                            impactSoundPhysicsParts.arraySize--;
                        }
                    }
                    EndChange();
                    BeginChange();
                    if (GUILayout.Button("Reset")) {
                        for (int i = 0; i < mTargets.Length; i++) {
                            Undo.RecordObject(mTargets[i], $"Reset {nameof(NameOf.SoundPhysics)}");
                            mTargets[i].internals.impactSoundPhysicsParts = new SoundPhysicsPart[1];
                            EditorUtility.SetDirty(mTargets[i]);
                        }
                    }
                    EndChange();
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.Separator();
                }
            }

            // Friction
            EditorGUILayout.Separator();
            SerializedProperty frictionExpand = internals.FindPropertyRelative(nameof(SoundPhysicsInternals.frictionExpand));
            BeginChange();
            EditorGuiFunction.DrawFoldout(frictionExpand, EditorTextSoundPhysics.frictionHeaderLabel, EditorTextSoundPhysics.frictionHeaderTooltip);
            EndChange();

            if (frictionExpand.boolValue) {

                // Transparent background so the offset will be right
                StartBackgroundColor(new Color(0f, 0f, 0f, 0f));
                SerializedProperty frictionPlay = internals.FindPropertyRelative(nameof(SoundPhysicsInternals.frictionPlay));
                BeginChange();
                EditorGUILayout.PropertyField(frictionPlay, new GUIContent("Play Friction"));
                EndChange();
                StopBackgroundColor();

                if (frictionPlay.boolValue) {
                    for (int i = 0; i < frictionSoundPhysicsParts.arraySize; i++) {
                        SerializedProperty part = frictionSoundPhysicsParts.GetArrayElementAtIndex(i);

                        StartBackgroundColor(EditorColor.ChangeHue(EditorColor.GetSettings(EditorColorProSkin.GetCustomEditorBackgroundAlpha()), i * -0.075f));
                        SerializedProperty soundEvent = part.FindPropertyRelative(nameof(SoundPhysicsPart.soundEvent));
                        BeginChange();
                        EditorGUILayout.PropertyField(soundEvent, new GUIContent(EditorTextSoundPhysics.frictionSoundEventLabel, EditorTextSoundPhysics.frictionSoundEventTooltip));
                        EndChange();
                        SerializedProperty collisionTagUse = part.FindPropertyRelative(nameof(SoundPhysicsPart.collisionTagUse));
                        BeginChange();
                        EditorGUILayout.PropertyField(collisionTagUse, new GUIContent(EditorTextSoundPhysics.frictionCollisionTagLabel, EditorTextSoundPhysics.frictionCollisionTagTooltip));
                        EndChange();

                        if (collisionTagUse.boolValue) {
                            SerializedProperty collisionTags = part.FindPropertyRelative(nameof(SoundPhysicsPart.collisionTags));
                            for (int ii = 0; ii < collisionTags.arraySize; ii++) {
                                EditorGUILayout.BeginHorizontal();
                                EditorGUILayout.BeginHorizontal();
                                // For offsetting the buttons to the right
                                EditorGUILayout.LabelField(new GUIContent(""), GUILayout.Width(EditorGUIUtility.labelWidth));
                                SerializedProperty tag = collisionTags.GetArrayElementAtIndex(ii);
                                BeginChange();
                                tag.stringValue = EditorGUILayout.TagField(tag.stringValue);
                                EndChange();
                                BeginChange();
                                if (GUILayout.Button("+")) {
                                    collisionTags.InsertArrayElementAtIndex(ii);
                                }
                                EndChange();
                                BeginChange();
                                if (GUILayout.Button("-")) {
                                    if (collisionTags.arraySize > 1) {
                                        collisionTags.DeleteArrayElementAtIndex(ii);
                                    }
                                }
                                EndChange();
                                BeginChange();
                                if (GUILayout.Button("↑")) {
                                    collisionTags.MoveArrayElement(ii, Mathf.Clamp(ii - 1, 0, collisionTags.arraySize));
                                }
                                EndChange();
                                BeginChange();
                                if (GUILayout.Button("↓")) {
                                    collisionTags.MoveArrayElement(ii, Mathf.Clamp(ii + 1, 0, collisionTags.arraySize));
                                }
                                EndChange();
                                EditorGUILayout.EndHorizontal();
                                EditorGUILayout.EndHorizontal();
                            }
                        }
                        StopBackgroundColor();
                        EditorGUILayout.Separator();
                    }

                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.BeginHorizontal();
                    // For offsetting the buttons to the right
                    EditorGUILayout.LabelField(new GUIContent(""), GUILayout.Width(EditorGUIUtility.labelWidth));
                    BeginChange();
                    if (GUILayout.Button("+")) {
                        frictionSoundPhysicsParts.arraySize++;
                    }
                    EndChange();
                    BeginChange();
                    if (GUILayout.Button("-")) {
                        if (frictionSoundPhysicsParts.arraySize > 1) {
                            frictionSoundPhysicsParts.arraySize--;
                        }
                    }
                    EndChange();
                    BeginChange();
                    if (GUILayout.Button("Reset")) {
                        for (int i = 0; i < mTargets.Length; i++) {
                            Undo.RecordObject(mTargets[i], $"Reset {nameof(NameOf.SoundPhysics)}");
                            mTargets[i].internals.frictionSoundPhysicsParts = new SoundPhysicsPart[1];
                            EditorUtility.SetDirty(mTargets[i]);
                        }
                    }
                    EndChange();
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.Separator();
                }
            }

            // Transparent background so the offset will be right
            StartBackgroundColor(new Color(0f, 0f, 0f, 0f));
            EditorGUILayout.BeginHorizontal();
            // For offsetting the buttons to the right
            EditorGUILayout.LabelField(new GUIContent(""), GUILayout.Width(EditorGUIUtility.labelWidth));
            
            // Reset Settings
            BeginChange();
            if (GUILayout.Button("Reset Settings")) {
                for (int i = 0; i < mTargets.Length; i++) {
                    Undo.RecordObject(mTargets[i], "Reset Settings");
                    // Impact
                    mTargets[i].internals.impactExpand = true;
                    mTargets[i].internals.impactPlay = true;
                    
                    // Friction
                    mTargets[i].internals.frictionExpand = true;
                    mTargets[i].internals.frictionPlay = false;
                    
                    EditorUtility.SetDirty(mTargets[i]);
                }
            }
            EndChange();

            // Reset All
            BeginChange();
            if (GUILayout.Button("Reset All")) {
                for (int i = 0; i < mTargets.Length; i++) {
                    Undo.RecordObject(mTargets[i], "Reset All");
                    // Impact
                    mTargets[i].internals.impactExpand = true;
                    mTargets[i].internals.impactPlay = true;
                    mTargets[i].internals.impactSoundPhysicsParts = new SoundPhysicsPart[1];
                    mTargets[i].internals.impactSoundParameter = new SoundParameterIntensity(0f, UpdateMode.Once);

                    // Friction
                    mTargets[i].internals.frictionExpand = true;
                    mTargets[i].internals.frictionPlay = false;
                    mTargets[i].internals.frictionSoundPhysicsParts = new SoundPhysicsPart[1];
                    mTargets[i].internals.frictionSoundParameter = new SoundParameterIntensity(0f, UpdateMode.Continuous);
                    mTargets[i].internals.frictionIsPlaying = false;
                    
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