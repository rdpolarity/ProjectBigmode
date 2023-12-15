// Created by Victor Engstr�m
// Copyright 2023 Sonigon AB
// http://www.sonity.org/

#if UNITY_EDITOR

namespace Sonity.Internal {

    public static class EditorPath {

        public static string GetFileNameWithoutExtension(string path) {
            // Remove path
            int slashIndex = path.LastIndexOf("/");
            if (slashIndex > 0) {
                path = path.Substring(slashIndex + 1, path.Length - slashIndex - 1);
            }
            // Remove extension
            int dotIndex = path.LastIndexOf(".");
            if (slashIndex > 0) {
                path = path.Substring(0, dotIndex);
            }
            return path;
        }

        public static string GetFileName(string path) {
            // Remove path
            int slashIndex = path.LastIndexOf("/");
            if (slashIndex > 0) {
                path = path.Substring(slashIndex + 1, path.Length - slashIndex - 1);
            }
            return path;
        }
    }
}
#endif