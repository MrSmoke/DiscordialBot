namespace DiscordialBot.Core.Internal;

internal class BotLifetime : IDisposable
{
    private readonly CancellationTokenSource _cts;
    private readonly ManualResetEventSlim _resetEvent;

    private bool _disposed;
    private bool _exitedGracefully;

    public BotLifetime(CancellationTokenSource cts, ManualResetEventSlim resetEventSlim)
    {
        _cts = cts;
        _resetEvent = resetEventSlim;

        AppDomain.CurrentDomain.ProcessExit += ProcessExit;
        Console.CancelKeyPress += CancelKeyPress;
    }

    public void SetExitedGracefully()
    {
        _exitedGracefully = true;
    }

    public void Dispose()
    {
        if (_disposed)
            return;

        _disposed = true;
        AppDomain.CurrentDomain.ProcessExit -= ProcessExit;
        Console.CancelKeyPress -= CancelKeyPress;
    }

    private void CancelKeyPress(object? sender, ConsoleCancelEventArgs eventArgs)
    {
        Shutdown();

        // Don't terminate the process immediately, wait for the Main thread to exit gracefully.
        eventArgs.Cancel = true;
    }

    private void ProcessExit(object? sender, EventArgs e)
    {
        Shutdown();

        if (_exitedGracefully)
        {
            // On Linux if the shutdown is triggered by SIGTERM then that's signaled with the 143 exit code.
            // Suppress that since we shut down gracefully. https://github.com/dotnet/aspnetcore/issues/6526
            Environment.ExitCode = 0;
        }
    }

    private void Shutdown()
    {
        if (!_cts.IsCancellationRequested)
        {
            Console.WriteLine("Shutting down...");
            _cts.Cancel();
        }

        // Wait on the given reset event
        _resetEvent.Wait();
    }
}
