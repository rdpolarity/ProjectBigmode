using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Dropdown))]
public class SampleSwitcher : MonoBehaviour
{
    [SerializeField] private Transform samplesContainer = null;

    private List<Transform> samples = null;

    private int currentIndex;
    
    private void Start()
    {
        samples = new List<Transform>();
        
        var dropdown = GetComponent<Dropdown>();

        var options = new List<string>();
        foreach (Transform sample in samplesContainer)
        {
            samples.Add(sample);
            options.Add(sample.name);
        }
        
        dropdown.AddOptions(options);
        
        dropdown.onValueChanged.AddListener(index =>
        {
            samples[currentIndex].gameObject.SetActive(false);
            samples[index].gameObject.SetActive(true);

            currentIndex = index;
        });
    }
}
