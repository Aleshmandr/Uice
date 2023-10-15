using System;
using System.Threading.Tasks;
using UnityEngine;

namespace Uice
{
    public abstract class ViewRegistry : MonoBehaviour
    {
        public abstract Task<RegistryLoadResult> Load(Type viewType);
    }
}