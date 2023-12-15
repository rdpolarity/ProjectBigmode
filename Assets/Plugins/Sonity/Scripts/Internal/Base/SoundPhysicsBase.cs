// Created by Victor Engström
// Copyright 2023 Sonigon AB
// http://www.sonity.org/

using UnityEngine;
using System;

namespace Sonity.Internal {

    [Serializable]
    public abstract class SoundPhysicsBase : MonoBehaviour {

        public SoundPhysicsInternals internals = new SoundPhysicsInternals();

        private void Start() {
            internals.FindComponents(gameObject);
        }

        private void Awake() {
            internals.FindComponents(gameObject);
        }

        private void OnCollisionEnter(Collision collision) {
            internals.OnCollisionEnter(collision);
        }

        private void OnCollisionEnter2D(Collision2D collision) {
            internals.OnCollisionEnter2D(collision);
        }

        private void OnCollisionStay(Collision collision) {
            internals.OnCollisionStay(collision);
        }

        private void OnCollisionStay2D(Collision2D collision) {
            internals.OnCollisionStay2D(collision);
        }

        private void OnCollisionExit(Collision collision) {
            internals.OnCollisionExit(collision);
        }

        private void OnCollisionExit2D(Collision2D collision) {
            internals.OnCollisionExit2D(collision);
        }
    }
}