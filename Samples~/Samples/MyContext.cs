namespace Uice.Examples
{
    [Path("aTest/Test2")]
    public class MyContext : Context
    {
        public ObservableVariable<string> Header { get; set; } = new();
        public ObservableVariable<string> Text { get; set; } = new();
        public ObservableVariable<float> Timer { get; set; } = new();
        
        public float TimerStatic { get; set; } = new();
        public ObservableVariable<MyItemContext> TestItem { get; set; } = new();
        public ObservableCollection<MyItemContext> Items { get; set; } = new();
    }

}
