namespace Lax.Console.Hosting {

    public class CommandLineHostingEnvironment : ICommandLineHostingEnvironment {

        public CommandLineHostingEnvironment(
            string[] commandLineArguments,
            string currentWorkingDirectory,
            bool isInteractive) {

            CommandLineArguments = commandLineArguments;
            CurrentWorkingDirectory = currentWorkingDirectory;
            IsInteractive = isInteractive;
        }

        public string[] CommandLineArguments { get; }

        public string CurrentWorkingDirectory { get; }

        public bool IsInteractive { get; }

    }

}
