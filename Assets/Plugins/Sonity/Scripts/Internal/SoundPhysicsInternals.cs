// Created by Victor Engström
// Copyright 2023 Sonigon AB
// http://www.sonity.org/

using UnityEngine;
using System;

namespace Sonity.Internal {

    [Serializable]
    public class SoundPhysicsInternals {

        [NonSerialized]
        public Rigidbody cachedRigidbody;
        [NonSerialized]
        public Rigidbody2D cachedRigidbody2D;
        [NonSerialized]
        public Transform cachedTransform;
        [NonSerialized]
        public bool initialized = false;

        public void FindComponents(GameObject gameObject) {
            if (!initialized) {
                initialized = true;
                if (physicsDimension == PhysicsDimension._3D) {
                    cachedRigidbody = gameObject.GetComponent<Rigidbody>();
                } else if (physicsDimension == PhysicsDimension._2D) {
                    cachedRigidbody2D = gameObject.GetComponent<Rigidbody2D>();
                }
                cachedTransform = gameObject.GetComponent<Transform>();
            }
        }

        public PhysicsDimension physicsDimension = PhysicsDimension._3D;

        // Impact
        public bool impactExpand = true;
        public bool impactPlay = true;
        public SoundPhysicsPart[] impactSoundPhysicsParts = new SoundPhysicsPart[1];
        public SoundParameterIntensity impactSoundParameter = new SoundParameterIntensity(0f, UpdateMode.Once);

        // Friction
        public bool frictionExpand = true;
        public bool frictionPlay = false;
        public SoundPhysicsPart[] frictionSoundPhysicsParts = new SoundPhysicsPart[1];
        public SoundParameterIntensity frictionSoundParameter = new SoundParameterIntensity(0f, UpdateMode.Continuous);

        [NonSerialized]
        public bool frictionIsPlaying = false;

        public void OnCollisionEnter(Collision collision) {
            if (impactPlay) {
                if (physicsDimension == PhysicsDimension._3D) {
                    // Impact Play
                    for (int i = 0; i < impactSoundPhysicsParts.Length; i++) {
                        if (CheckCollisionTag(impactSoundPhysicsParts[i], collision.transform.tag)) {
                            // Set SoundParameter
                            impactSoundParameter.Intensity = collision.relativeVelocity.magnitude;
                            // Play Impact
                            for (int n = 0; n < collision.contacts.Length; n++) {
                                SoundManagerBase.Instance.Internals.PlayAtPosition(
                                    impactSoundPhysicsParts[i].soundEvent, 
                                    cachedTransform, 
                                    collision.contacts[n].point, impactSoundParameter
                                    );
                            }
                        }
                    }
                }
            }
        }

        public void OnCollisionEnter2D(Collision2D collision) {
            if (impactPlay) {
                if (physicsDimension == PhysicsDimension._2D) {
                    // Impact Play
                    for (int i = 0; i < impactSoundPhysicsParts.Length; i++) {
                        if (CheckCollisionTag(impactSoundPhysicsParts[i], collision.transform.tag)) {
                            // Set SoundParameter
                            impactSoundParameter.Intensity = collision.relativeVelocity.magnitude;
                            // Play Impact
                            for (int n = 0; n < collision.contacts.Length; n++) {
                                SoundManagerBase.Instance.Internals.PlayAtPosition(
                                    impactSoundPhysicsParts[i].soundEvent, 
                                    cachedTransform, 
                                    collision.contacts[n].point, 
                                    impactSoundParameter
                                    );
                            }
                        }
                    }
                }
            }
        }

        public void OnCollisionStay(Collision collision) {
            if (frictionPlay) {
                if (physicsDimension == PhysicsDimension._3D) {
                    // Friction Play
                    for (int i = 0; i < frictionSoundPhysicsParts.Length; i++) {
                        if (CheckCollisionTag(frictionSoundPhysicsParts[i], collision.transform.tag)) {
                            // Set SoundParameter
                            frictionSoundParameter.Intensity = cachedRigidbody.velocity.magnitude;
                            // Play friction
                            if (!frictionIsPlaying) {
                                frictionIsPlaying = true;
                                SoundManagerBase.Instance.Internals.Play(
                                    frictionSoundPhysicsParts[i].soundEvent, 
                                    cachedTransform, 
                                    frictionSoundParameter
                                    );
                            }
                        }
                    }
                }
            }
        }

        public void OnCollisionStay2D(Collision2D collision) {
            if (frictionPlay) {
                if (physicsDimension == PhysicsDimension._2D) {
                    // Friction Play
                    for (int i = 0; i < frictionSoundPhysicsParts.Length; i++) {
                        if (CheckCollisionTag(frictionSoundPhysicsParts[i], collision.transform.tag)) {
                            // Set SoundParameter
                            frictionSoundParameter.Intensity = cachedRigidbody2D.velocity.magnitude;
                            // Play friction
                            if (!frictionIsPlaying) {
                                frictionIsPlaying = true;
                                SoundManagerBase.Instance.Internals.Play(
                                    frictionSoundPhysicsParts[i].soundEvent, 
                                    cachedTransform, 
                                    frictionSoundParameter
                                    );
                            }
                        }
                    }
                }
            }
        }

        public void OnCollisionExit(Collision collision) {
            // So it will stop even if friction is disabled in the editor
            if (frictionPlay || frictionIsPlaying) {
                if (physicsDimension == PhysicsDimension._3D) {
                    // Friction Stop
                    for (int i = 0; i < frictionSoundPhysicsParts.Length; i++) {
                        if (CheckCollisionTag(frictionSoundPhysicsParts[i], collision.gameObject.tag)) {
                            // Stop Friction
                            if (frictionIsPlaying) {
                                frictionIsPlaying = false;
                                SoundManagerBase.Instance.Internals.Stop(frictionSoundPhysicsParts[i].soundEvent, cachedTransform);
                            }
                        }
                    }
                }
            }
        }

        public void OnCollisionExit2D(Collision2D collision) {
            // So it will stop even if friction is disabled in the editor
            if (frictionPlay || frictionIsPlaying) {
                if (physicsDimension == PhysicsDimension._2D) {
                    // Friction Stop
                    for (int i = 0; i < frictionSoundPhysicsParts.Length; i++) {
                        if (CheckCollisionTag(frictionSoundPhysicsParts[i], collision.gameObject.tag)) {
                            // Stop Friction
                            if (frictionIsPlaying) {
                                frictionIsPlaying = false;
                                SoundManagerBase.Instance.Internals.Stop(frictionSoundPhysicsParts[i].soundEvent, cachedTransform);
                            }
                        }
                    }
                }
            }
        }

        private bool CheckCollisionTag(SoundPhysicsPart soundEventAndTag, string tag) {
            if (soundEventAndTag.collisionTagUse) {
                for (int i = 0; i < soundEventAndTag.collisionTags.Length; i++) {
                    if (tag == soundEventAndTag.collisionTags[i]) {
                        return true;
                    }
                }
            } else {
                return true;
            }
            return false;
        }
    }
}