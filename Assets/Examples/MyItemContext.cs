namespace Uice.Examples
{
    [Path("Test/Test2")]
    public class MyItemContext : Context
    {
        public ObservableVariable<string> Name { get; } = new();

        public MyItemContext(string name)
        {
            Name.Value = name;
        }
    }
}
