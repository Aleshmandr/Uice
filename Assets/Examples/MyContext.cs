namespace Uice.Examples
{
    public class MyContext : Context
    {
        public ObservableVariable<string> Header { get; set; } = new();
    }
}
