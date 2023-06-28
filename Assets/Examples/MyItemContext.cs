namespace Uice.Examples
{
    public class MyItemContext : Context
    {
        public ObservableVariable<string> Name { get; }

        public MyItemContext(string name)
        {
            Name = name;
        }
    }
}
