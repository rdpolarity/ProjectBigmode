// Created by Victor Engstr�m
// Copyright 2023 Sonigon AB
// http://www.sonity.org/

#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

namespace Sonity.Internal {

    public static class EditorShortcutsPreview {

        // Shortcut key modifiers
        // % -> Ctrl on Windows, Linux, CMD on MacOS
        // ^ -> Ctrl on Windows, Linux, MacOS
        // # -> Shift
        // & -> Alt

        /// <summary>
        /// Previews the selected <see cref="SoundEventBase">SoundEvent</see> or <see cref="SoundContainerBase">SoundContainer</see>.
        /// </summary>
        [MenuItem("Tools/Sonity/Sound Preview - Play ^Q")]
        private static void SoundPreviewPlay() {
            playTime = Time.realtimeSinceStartup;
            playPressed = true;
            previewRepaint = ScriptableObject.CreateInstance<EditorShortcutsPreviewRepaint>();
            previewRepaint.DoRepaint();
        }

        /// <summary>
        /// Stops any playing preview of <see cref="SoundEventBase">SoundEvents</see> and <see cref="SoundContainerBase">SoundContainers</see>, press two times to skip fade out.
        /// </summary>
        [MenuItem("Tools/Sonity/Sound Preview - Stop ^W")]
        private static void SoundPreviewStop() {
            stopTime = Time.realtimeSinceStartup;
            stopPressed = true;
            previewRepaint = ScriptableObject.CreateInstance<EditorShortcutsPreviewRepaint>();
            previewRepaint.DoRepaint();
        }

        private class EditorShortcutsPreviewRepaint : Editor {
            // So that the inspectors of the objects will refresh
            public void DoRepaint() {
                Repaint();
            }
        }

        private static EditorShortcutsPreviewRepaint previewRepaint;

        private static float playTime = 0f;
        private static bool playPressed = false;

        private static float stopTime = 0f;
        private static bool stopPressed = false;

        // There was about 0.004 difference in test
        private static float timeAllowedDifference = 0.1f;

        public static bool GetPlayIsPressed() {
            if (playPressed && playTime > 0f) {
                playPressed = false;
                if (Time.realtimeSinceStartup + timeAllowedDifference >= playTime && Time.realtimeSinceStartup - timeAllowedDifference <= playTime) {
                    playTime = 0f;
                    return true;
                }
            }
            return false;
        }

        public static bool GetStopIsPressed() {
            if (stopPressed && stopTime > 0f) {
                stopPressed = false;
                if (Time.realtimeSinceStartup + timeAllowedDifference >= stopTime && Time.realtimeSinceStartup - timeAllowedDifference <= stopTime) {
                    stopTime = 0f;
                    return true;
                }
            }
            return false;
        }
    }
}
#endif