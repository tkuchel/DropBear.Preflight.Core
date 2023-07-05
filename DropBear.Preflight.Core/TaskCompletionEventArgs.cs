namespace DropBear.Preflight.Core;

/// <summary>
/// Provides data for the TaskCompleted event.
/// </summary>
public class TaskCompletionEventArgs : EventArgs
{
    /// <summary>
    /// Gets the task that has completed.
    /// </summary>
    public PreflightTask Task { get; }

    /// <summary>
    /// Gets a value indicating whether the task completed successfully.
    /// </summary>
    public bool IsSuccessful { get; }

    /// <summary>
    /// Gets the exception that was thrown, if any.
    /// </summary>
    public Exception Error { get; }

    public TaskCompletionEventArgs(PreflightTask task, bool isSuccessful, Exception error)
    {
        Task = task;
        IsSuccessful = isSuccessful;
        Error = error;
    }
}
