namespace Uice.Examples
{
    [Path("aTest/Test2")]
    public class MyContext : Context
    {
        public ObservableVariable<string> Header { get; set; }
        public ObservableVariable<string> Text { get; set; }
        public ObservableVariable<MyItemContext> TestItem { get; set; }
        
        public ObservableCollection<MyItemContext> Items { get; set; }
    }

}
