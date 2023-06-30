namespace Uice.Examples
{
    [Path("Test/Test2")]
    public class MyItemContext : Context
    {
        public ObservableVariable<string> Name { get; }

        public MyItemContext(string name)
        {
            Name = name;
        }
    }
}
