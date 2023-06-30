namespace Uice.Examples
{
    [Path("Test2")]
    public class MySecondContext : Context
    {
        public ObservableVariable<string> Header { get; set; }
        public ObservableVariable<string> Text { get; set; }
    }
}
