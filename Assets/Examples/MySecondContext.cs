namespace Uice.Examples
{
    public class MySecondContext : Context
    {
        public ObservableVariable<string> Header { get; set; }
        public ObservableVariable<string> Text { get; set; }
    }
}
