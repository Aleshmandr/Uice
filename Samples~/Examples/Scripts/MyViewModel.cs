namespace Uice.Examples
{
    [Path("aTest/Test2")]
    public class MyViewModel : ViewModel
    {
        public ObservableVariable<string> Header { get; set; } = new();
        public ObservableVariable<string> Text { get; set; } = new();
        public ObservableVariable<float> Timer { get; set; } = new();
        
        public float TimerStatic { get; set; } = new();
        public ObservableVariable<MyItemViewModel> TestItem { get; set; } = new();
        public ObservableCollection<MyItemViewModel> Items { get; set; } = new();
    }

}
