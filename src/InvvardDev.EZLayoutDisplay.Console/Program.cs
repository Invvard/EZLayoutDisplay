namespace InvvardDev.EZLayoutDisplay.Console
{
    class Program
    {
        // ReSharper disable once UnusedParameter.Local
        static void Main(string[] args)
        {
            InitializeProcess();
        }

        private static void InitializeProcess()
        {
            var process = new KeyDefinitionProcessor();
            process.RunProcess();
        }
    }
}
