namespace InvvardDev.EZLayoutDisplay.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            InitializeProcess();
        }

        private static async void InitializeProcess()
        {
            var process = new KeyDefinitionProcessor();
            await process.StartProcess();
        }
    }
}
