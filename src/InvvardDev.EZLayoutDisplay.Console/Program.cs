namespace InvvardDev.EZLayoutDisplay.Console
{
    class Program
    {
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
