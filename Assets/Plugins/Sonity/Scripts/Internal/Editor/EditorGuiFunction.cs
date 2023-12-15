// Created by Victor Engström
// Copyright 2023 Sonigon AB
// http://www.sonity.org/

#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;

namespace Sonity.Internal {
    public static class EditorGuiFunction {

        public static void BeginChange(SerializedObject serializedObject) {
            serializedObject.Update();
            EditorGUI.BeginChangeCheck();
        }

        public static void EndChange(SerializedObject serializedObject) {
            if (EditorGUI.EndChangeCheck()) {
                serializedObject.ApplyModifiedProperties();
            }
        }

        public static void DrawReordableArray(
            SerializedProperty arrayProperty, SerializedObject serializedObject, int lowestArrayLength, bool fieldOffsetRight,
            bool setNull = true, bool lengthCanBeZero = true, bool typeIsTag = false, int offsetSize = 0, SerializedProperty arrayPropertySecond = null
            ) {

            int oldIntentLevel = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            EditorGUILayout.BeginHorizontal();
            if (fieldOffsetRight) {

                EditorGUI.indentLevel = oldIntentLevel;

                // For offsetting the buttons to the right
                EditorGUILayout.LabelField(new GUIContent("Size"), GUILayout.Width(EditorGUIUtility.labelWidth - offsetSize));
                EditorGUI.indentLevel = 0;
                BeginChange(serializedObject);
                if (lengthCanBeZero) {
                    arrayProperty.arraySize = EditorGUILayout.IntField(arrayProperty.arraySize);
                } else {
                    arrayProperty.arraySize = Mathf.Clamp(EditorGUILayout.IntField(arrayProperty.arraySize), 1, int.MaxValue);
                }
                EndChange(serializedObject);
            } else {
                EditorGUI.indentLevel = oldIntentLevel;
                BeginChange(serializedObject);
#if UNITY_2019_1_OR_NEWER
                if (lengthCanBeZero) {
                    arrayProperty.arraySize = EditorGUILayout.IntField(arrayProperty.arraySize, GUILayout.Width(EditorGUIUtility.currentViewWidth - 136));
                } else {
                    arrayProperty.arraySize = Mathf.Clamp(EditorGUILayout.IntField(arrayProperty.arraySize, GUILayout.Width(EditorGUIUtility.currentViewWidth - 140)), 1, int.MaxValue);
                }
#else
                // Code for older
                if (lengthCanBeZero) {
                    arrayProperty.arraySize = EditorGUILayout.IntField(arrayProperty.arraySize);
                } else {
                    arrayProperty.arraySize = Mathf.Clamp(EditorGUILayout.IntField(arrayProperty.arraySize), 1, int.MaxValue);
                }
#endif
                EndChange(serializedObject);
                EditorGUI.indentLevel = 0;
                // For offsetting the field to the left
                EditorGUILayout.LabelField(new GUIContent("Size"), GUILayout.Width(EditorGUIUtility.labelWidth));
            }
            EditorGUILayout.EndHorizontal();

            for (int i = 0; i < arrayProperty.arraySize; i++) {
                // Out of bounds check
                if (i >= lowestArrayLength) {
                    break;
                }

                SerializedProperty arrayElement = arrayProperty.GetArrayElementAtIndex(i);

                EditorGUILayout.BeginHorizontal();
                if (fieldOffsetRight) {
                    // For offsetting the buttons to the right
                    EditorGUILayout.LabelField(new GUIContent(""), GUILayout.Width(EditorGUIUtility.labelWidth - offsetSize));
                }

                BeginChange(serializedObject);

#if UNITY_2019_1_OR_NEWER
                if (typeIsTag) {
                    if (fieldOffsetRight) {
                        arrayElement.stringValue = EditorGUILayout.TagField(arrayElement.stringValue);
                    } else {
                        EditorGUI.indentLevel = oldIntentLevel;
                        arrayElement.stringValue = EditorGUILayout.TagField(arrayElement.stringValue, GUILayout.Width(EditorGUIUtility.currentViewWidth - 140));
                        EditorGUI.indentLevel = 0;
                    }
                } else {
                    if (fieldOffsetRight) {
                        EditorGUILayout.PropertyField(arrayElement, new GUIContent($"", ""));
                    } else {
                        EditorGUI.indentLevel = oldIntentLevel;
                        EditorGUILayout.PropertyField(arrayElement, new GUIContent($"", ""), GUILayout.Width(EditorGUIUtility.currentViewWidth - 136));
                        EditorGUI.indentLevel = 0;
                    }
                }
#else
                // Code for older
                if (typeIsTag) {
                    if (fieldOffsetRight) {
                        arrayElement.stringValue = EditorGUILayout.TagField(arrayElement.stringValue);
                    } else {
                        EditorGUI.indentLevel = oldIntentLevel;
                        arrayElement.stringValue = EditorGUILayout.TagField(arrayElement.stringValue);
                        EditorGUI.indentLevel = 0;
                    }
                } else {
                    if (fieldOffsetRight) {
                        EditorGUILayout.PropertyField(arrayElement, new GUIContent($"", ""));
                    } else {
                        EditorGUI.indentLevel = oldIntentLevel;
                        EditorGUILayout.PropertyField(arrayElement, new GUIContent($"", ""));
                        EditorGUI.indentLevel = 0;
                    }
                }
#endif
                EndChange(serializedObject);

                int buttonWidth = 19;

                BeginChange(serializedObject);
                if (GUILayout.Button("+", GUILayout.Width(buttonWidth))) {
                    arrayProperty.InsertArrayElementAtIndex(i);
                    // Used by timeline data
                    if (arrayPropertySecond != null) {
                        arrayPropertySecond.InsertArrayElementAtIndex(i);
                    }
                }
                EndChange(serializedObject);
                BeginChange(serializedObject);
                if (GUILayout.Button("-", GUILayout.Width(buttonWidth))) {
                    // Set to null so it will delete instantly
                    if (setNull) {
                        arrayElement.objectReferenceValue = null;
                    }
                    if (lengthCanBeZero) {
                        if (arrayProperty.arraySize > 0) {
                            arrayProperty.DeleteArrayElementAtIndex(i);
                            // Used by timeline data
                            if (arrayPropertySecond != null) {
                                arrayPropertySecond.DeleteArrayElementAtIndex(i);
                            }
                        }
                    } else {
                        if (arrayProperty.arraySize > 1) {
                            arrayProperty.DeleteArrayElementAtIndex(i);
                            // Used by timeline data
                            if (arrayPropertySecond != null) {
                                arrayPropertySecond.DeleteArrayElementAtIndex(i);
                            }
                        }
                    }
                }
                EndChange(serializedObject);
                BeginChange(serializedObject);
                if (GUILayout.Button("↑", GUILayout.Width(buttonWidth))) {
                    arrayProperty.MoveArrayElement(i, Mathf.Clamp(i - 1, 0, arrayProperty.arraySize));
                    // Used by timeline data
                    if (arrayPropertySecond != null) {
                        arrayPropertySecond.MoveArrayElement(i, Mathf.Clamp(i - 1, 0, arrayProperty.arraySize));
                    }
                }
                EndChange(serializedObject);
                BeginChange(serializedObject);
                if (GUILayout.Button("↓", GUILayout.Width(buttonWidth))) {
                    arrayProperty.MoveArrayElement(i, Mathf.Clamp(i + 1, 0, arrayProperty.arraySize));
                    // Used by timeline data
                    if (arrayPropertySecond != null) {
                        arrayPropertySecond.MoveArrayElement(i, Mathf.Clamp(i + 1, 0, arrayProperty.arraySize));
                    }
                }
                EndChange(serializedObject);
                EditorGUILayout.EndHorizontal();
            }

            EditorGUI.indentLevel = oldIntentLevel;
        }

        public static void DrawFoldout(SerializedProperty expanded, string label, string tooltip = "", int indentLevel = 0, bool expandedBool = false, bool smallFont = false) {

            GUIStyle toggleStyle = new GUIStyle(EditorStyles.foldout);

            if (!smallFont) {
                toggleStyle.fontSize = 16;
            }
            toggleStyle.fontStyle = FontStyle.Bold;

            if (smallFont) {
                toggleStyle.margin = new RectOffset(indentLevel * 22, 0, 3, 5);
            } else {
                toggleStyle.margin = new RectOffset(indentLevel * 20, 0, 0, 3);
            }

            // Draw Toggle
            if (expandedBool) {
                expanded.isExpanded = GUILayout.Toggle(expanded.isExpanded, new GUIContent(label, tooltip), toggleStyle);
            } else {
                expanded.boolValue = GUILayout.Toggle(expanded.boolValue, new GUIContent(label, tooltip), toggleStyle);
            }
        }

        public static void DrawFoldoutTitle(string label, string tooltip = "", int indentLevel = 0, bool smallFont = false) {

            GUIStyle toggleStyle = new GUIStyle();

            if (EditorGUIUtility.isProSkin) {
                toggleStyle.normal.textColor = EditorColorProSkin.GetDarkSkinTextColor();
            }

            if (!smallFont) {
                toggleStyle.fontSize = 16;
            }
            toggleStyle.fontStyle = FontStyle.Bold;

            if (smallFont) {
                toggleStyle.margin = new RectOffset(indentLevel * 22, 0, 3, 5);
            } else {
                toggleStyle.margin = new RectOffset(indentLevel * 20, 0, 0, 3);
            }

            GUILayout.Label(new GUIContent(label, tooltip), toggleStyle);
        }

        public static void DrawScriptableObjectTitle(string label) {

            GUIStyle toggleStyle = new GUIStyle();

            if (EditorGUIUtility.isProSkin) {
                toggleStyle.normal.textColor = EditorColorProSkin.GetDarkSkinTextColor();
            }

            toggleStyle.fontSize = 16;
            toggleStyle.fontStyle = FontStyle.Bold;

            toggleStyle.margin = new RectOffset(0, 0, 0, 3);

            GUILayout.Label(new GUIContent(label), toggleStyle);
        }

        private static void StartBackgroundColor(Color color) {
            Color defaultGuiColor = GUI.color;
            GUI.color = color;
            EditorGUILayout.BeginVertical("Button");
            GUI.color = defaultGuiColor;
        }

        private static void StopBackgroundColor() {
            EditorGUILayout.EndVertical();
        }

        public static void DrawLayoutObjectTitle(string title, string tooltip = "", bool drawBackground = true) {

            GUIStyle guiStyleBoldCenter = new GUIStyle();

            guiStyleBoldCenter.fontStyle = FontStyle.Bold;
            guiStyleBoldCenter.alignment = TextAnchor.MiddleCenter;
            guiStyleBoldCenter.fontSize = 16;
            if (EditorGUIUtility.isProSkin) {
                guiStyleBoldCenter.normal.textColor = EditorColorProSkin.GetDarkSkinTextColor();
            }

            if (drawBackground) {
                StartBackgroundColor(Color.white);
            }
            GUILayout.Box(new GUIContent(title, tooltip), guiStyleBoldCenter, GUILayout.ExpandWidth(true), GUILayout.Height(25));
            if (drawBackground) {
                StopBackgroundColor();
            }
        }
    }
}
#endif