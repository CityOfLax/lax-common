namespace Lax.Console.Hosting {

    public interface ICommandLineHostingEnvironment {

        string[] CommandLineArguments { get; }

        string CurrentWorkingDirectory { get; }

        bool IsInteractive { get; }

    }

}
