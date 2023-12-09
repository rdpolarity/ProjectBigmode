using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Rendering;

namespace Bigmode
{
    public class Attributes : MonoBehaviour {
        [DictionaryDrawerSettings(KeyLabel = "Attribute", ValueLabel = "Value")]
        public SerializedDictionary<string, float> attributes = new();

        public void SetAttribute(string name, float value) {
            float preValue = PreAttributeChange(name, value);
            attributes[name] = preValue;
            PostAttributeChange(name, preValue);
        }

        public float? GetAttribute(string name) {
            return attributes.TryGetValue(name, out var value) ? value : null;
        }

        public virtual float PreAttributeChange(string name, float value) {
            return value;
        }

        public virtual void PostAttributeChange(string name, float value) {
        }
    }
}