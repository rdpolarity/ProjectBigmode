using System.Collections.Generic;
using UnityEngine.AddressableAssets;

namespace Bigmode
{
    public static class Assets
    {
        static Assets()
        {
            Addressables.InitializeAsync().WaitForCompletion();
        }
        
        public static List<T> GetAll<T>(string key)
        {
            var found = new List<T>();
            
            foreach (var locator in Addressables.ResourceLocators)
            {
                if (!locator.Locate(key, typeof(T), out var locations)) 
                    continue;

                foreach (var location in locations)
                {
                    found.Add(Addressables.LoadAssetAsync<T>(location).WaitForCompletion());
                }
            }
            
            return found;
        }
    }
}
