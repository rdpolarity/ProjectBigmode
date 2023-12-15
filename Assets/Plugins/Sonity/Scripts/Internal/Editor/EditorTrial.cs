// Created by Victor Engstr�m
// Copyright 2023 Sonigon AB
// http://www.sonity.org/

#if UNITY_EDITOR

using UnityEngine;

namespace Sonity.Internal {

    public static class EditorTrial {
        public static string trialText = "";
        public static string trialTooltip = "";
        public static bool isTrial = false;

        public static void InfoText() {
            if (isTrial) {
                GUIStyle guiStyleTrial = new GUIStyle();
                guiStyleTrial.fontSize = 16;
                guiStyleTrial.fontStyle = FontStyle.Bold;
                guiStyleTrial.alignment = TextAnchor.MiddleCenter;
                guiStyleTrial.normal.textColor = EditorColorProSkin.GetTextOrange();
#if UNITY_2019_1_OR_NEWER
                if (GUILayout.Button(new GUIContent($"Free Trial - Buy the full version to get sound in builds",
                    "This free trial is for testing only.\nIt doesn't output sounds in build\nPlease buy the full version to get sound in builds and to support developement.")
                    , guiStyleTrial, GUILayout.ExpandWidth(true), GUILayout.Height(40))
                    ) {
                    Application.OpenURL("https://assetstore.unity.com/packages/tools/audio/sonity-audio-middleware-229857");
                }
#else
                // Code for older
                if (GUILayout.Button(new GUIContent($"Free Trial - Buy the full version to get sounds in build",
                    "This free trial is for testing only.\nIt doesn't output sounds in build\nPlease buy the full version to get sound in builds and to support developement.")
                    , guiStyleTrial, GUILayout.ExpandWidth(true), GUILayout.Height(40), GUILayout.Width(EditorGUIUtility.currentViewWidth - 55))
                    ) {
                    Application.OpenURL("https://assetstore.unity.com/packages/tools/audio/sonity-audio-middleware-229857");
                }
#endif
            }
        }
    }
}
#endif