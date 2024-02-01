namespace Uice
{
	public abstract class ToOperator<TFrom, TTo> : ProcessorOperator<TFrom, TTo>
	{
		protected IBindingProcessor bindingProcessor;
		
		protected abstract TTo Convert(TFrom value);
		
		protected override IBindingProcessor GetBindingProcessor(BindingType bindingType, BindingInfo fromBinding)
		{
			bindingProcessor = null;
			
			switch (bindingType)
			{
				case BindingType.Variable:
					bindingProcessor = new ToVariableBindingProcessor<TFrom, TTo>(fromBinding, this, Convert);
					break;
				case BindingType.Collection:
					bindingProcessor = new ToCollectionBindingProcessor<TFrom, TTo>(fromBinding, this, Convert);
					break;
				case BindingType.Command:
					bindingProcessor = new ToCommandBindingProcessor<TFrom, TTo>(fromBinding, this, Convert);
					break;
				case BindingType.Event:
					bindingProcessor = new ToEventBindingProcessor<TFrom, TTo>(fromBinding, this, Convert);
					break;
			}

			return bindingProcessor;
		}
	}
}