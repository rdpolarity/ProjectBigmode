// Created by Victor Engström
// Copyright 2023 Sonigon AB
// http://www.sonity.org/

#if UNITY_EDITOR

namespace Sonity.Internal {

    public static class EditorTimeSinceStartup {
        // Used for preview in the editor
        public static float time = 0f;
    }
}
#endif