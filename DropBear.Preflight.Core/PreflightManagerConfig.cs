namespace DropBear.Preflight.Core;

public class PreflightManagerConfig
{
    // Should the manager stop executing tasks if a task fails?
    public bool StopOnFailure { get; set; } = false;

    // The timeout for each task
    public TimeSpan TaskTimeout { get; set; } = TimeSpan.FromMinutes(1);

    // The maximum number of tasks that can run concurrently
    public int MaxConcurrentTasks { get; set; } = 1;

    // The number of times to retry a task if it fails
    public int RetryCount { get; set; } = 0;
}
