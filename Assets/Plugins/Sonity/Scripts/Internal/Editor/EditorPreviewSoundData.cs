// Created by Victor Engstr�m
// Copyright 2023 Sonigon AB
// http://www.sonity.org/

#if UNITY_EDITOR

using System.Collections.Generic;

namespace Sonity.Internal {

    public class EditorPreviewSoundData {

        public SoundContainerBase soundContainer;
        public List<SoundEventModifier> soundEventModifierList = new List<SoundEventModifier>();
        public SoundEventTimelineData timelineSoundContainerData = new SoundEventTimelineData();
        public SoundEventBase soundEvent;
    }
}
#endif