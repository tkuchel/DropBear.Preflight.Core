namespace DropBear.Preflight.Core;

public class TaskCompletionEventArgs : EventArgs
{
    // The task that has completed
    public PreflightTask Task { get; }

    // A flag indicating whether the task completed successfully
    public bool IsSuccessful { get; }

    // The exception that was thrown, if any
    public Exception? Error { get; }

    public TaskCompletionEventArgs(PreflightTask task, bool isSuccessful, Exception? error)
    {
        Task = task;
        IsSuccessful = isSuccessful;
        Error = error;
    }
}
