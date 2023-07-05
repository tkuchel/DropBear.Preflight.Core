namespace DropBear.Preflight.Core;

/// <summary>
/// Represents a task to be executed during the preflight phase.
/// </summary>
public abstract class PreflightTask
{
    /// <summary>
    /// Gets or sets the priority of the task.
    /// </summary>
    public int Priority { get; set; } = 0;

    /// <summary>
    /// Gets the list of tasks that this task depends on.
    /// </summary>
    public List<PreflightTask> Dependencies { get; set; } = new List<PreflightTask>();

    /// <summary>
    /// Gets a value indicating whether the task has completed.
    /// </summary>
    public bool IsCompleted { get; private set; } = false;

    /// <summary>
    /// Executes the task.
    /// </summary>
    public abstract Task ExecuteAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Marks the task as completed.
    /// </summary>
    protected void MarkAsCompleted()
    {
        IsCompleted = true;
    }
}

