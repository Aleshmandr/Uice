using System;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

namespace Uice
{
    [Serializable]
    public class ResourcesViewRegistry : ViewRegistry
    {
        [SerializeField] private string resourcesPath;

        private static readonly RegistryLoadResult FailResult = new RegistryLoadResult()
        {
            Prefab = null,
            IsLoaded = false
        };

        public override async Task<RegistryLoadResult> Load(Type viewType)
        {
            var viewName = viewType.Name;
            string fullPath = Path.Combine(resourcesPath, viewName);
            var viewPrefab = Resources.Load<GameObject>(fullPath);

            if (viewPrefab == null)
            {
                return FailResult;
            }

            var prefabComponent = (Component) viewPrefab.GetComponent<IView>();
            var viewObject = Instantiate(prefabComponent);

            return new RegistryLoadResult
            {
                Prefab = (IView) viewObject,
                IsLoaded = prefabComponent != null
            };
        }
    }
}