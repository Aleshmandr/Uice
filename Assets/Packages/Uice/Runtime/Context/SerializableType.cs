using System;
using UnityEngine;

namespace Uice
{
    [Serializable]
    public class SerializableType : ISerializationCallbackReceiver
    {
        [SerializeField] private string typeReference;
        private Type type;
        
        public Type Type
        {
            get => type;

            set
            {
                type = value;
                typeReference = GetTypeReference(value);
            }
        }

        public SerializableType() { }

        public SerializableType(Type type)
        {
            Type = type;
        }

        public SerializableType(string assemblyQualifiedTypeName) : this(Type.GetType(assemblyQualifiedTypeName)) { }

        public void OnBeforeSerialize() { }

        public void OnAfterDeserialize()
        {
            if (string.IsNullOrEmpty(typeReference))
            {
                type = null;
            } else
            {
                type = Type.GetType(typeReference);

                if (type == null)
                {
                    Debug.LogError($"Serialized type \"({typeReference})\" is not a valid Type.");
                }
            }
        }

        private static string GetTypeReference(Type type)
        {
            string result = string.Empty;

            if (type != null)
            {
                result = type.AssemblyQualifiedName;
            }

            return result;
        }
    }
}
