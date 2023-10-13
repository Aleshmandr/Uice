using System;
using System.Collections.Generic;
using System.Reflection;

namespace Uice
{
	public class ContextComponentInfo
	{
		public string Id => Component.Id;
		public ContextComponent Component { get; }

		private readonly Dictionary<string, object> properties;

		public ContextComponentInfo(ContextComponent component)
		{
			properties = new Dictionary<string, object>();
			Component = component;

			Component.ContextChanged += OnContextChanged;
		}

		public object GetProperty(string name)
		{
			if (properties.TryGetValue(name, out var result) == false && Component.Context != null)
			{
				Type contextType = Component.Context.GetType();
				PropertyInfo propertyInfo = contextType.GetProperty(name);

				if (propertyInfo != null)
				{
					result = propertyInfo.GetValue(Component.Context);

					if (result != null)
					{
						properties[name] = result;
					}
				}
			}

			return result;
		}

		private void OnContextChanged(IContextProvider<IContext> source, IContext lastContext, IContext newContext)
		{
			properties.Clear();
		}
	}
}
