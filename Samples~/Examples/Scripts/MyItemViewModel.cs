namespace Uice.Examples
{
    [Path("Test/Test2")]
    public class MyItemViewModel : ViewModel
    {
        public ObservableVariable<string> Name { get; } = new();

        public MyItemViewModel(string name)
        {
            Name.Value = name;
        }
    }
}
