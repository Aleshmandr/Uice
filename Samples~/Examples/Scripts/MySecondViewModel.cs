namespace Uice.Examples
{
    [Path("Test2")]
    public class MySecondViewModel : ViewModel
    {
        public ObservableVariable<string> Header { get; set; } = new();
        public ObservableVariable<string> Text { get; set; } = new();
    }
}
