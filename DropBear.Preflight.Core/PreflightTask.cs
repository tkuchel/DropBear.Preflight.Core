namespace DropBear.Preflight.Core;

/// <summary>
///     Represents a task to be executed during the preflight phase.
/// </summary>
public abstract class PreflightTask
{
    private readonly List<PreflightTask> _dependencies = new();
    private readonly object _lock = new();
    private volatile bool _hasFailed;
    private volatile bool _isCompleted;

    /// <summary>
    ///     Gets or sets the name of the task.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the priority of the task.
    /// </summary>
    public int Priority { get; set; } = 0;

    /// <summary>
    ///     Gets the list of tasks that this task depends on.
    /// </summary>
    public IReadOnlyList<PreflightTask> Dependencies => _dependencies;

    /// <summary>
    ///     Gets a value indicating whether the task has completed.
    /// </summary>
    public bool IsCompleted => _isCompleted;

    /// <summary>
    ///     Gets a value indicating whether the task has failed.
    /// </summary>
    public bool HasFailed => _hasFailed;

    /// <summary>
    ///     Gets the exception that caused the task to fail, if any.
    /// </summary>
    public Exception? FailureException { get; private set; }

    /// <summary>
    ///     Executes the task.
    /// </summary>
    public abstract Task ExecuteAsync(CancellationToken cancellationToken);

    /// <summary>
    ///     Adds a dependency to this task.
    /// </summary>
    public void AddDependency(PreflightTask task)
    {
        _dependencies.Add(task);
    }

    /// <summary>
    ///     Marks the task as completed.
    /// </summary>
    protected void MarkAsCompleted()
    {
        lock (_lock)
        {
            _isCompleted = true;
        }
    }

    /// <summary>
    ///     Marks the task as failed and stores the exception that caused the failure.
    /// </summary>
    /// <param name="ex">The exception that caused the task to fail.</param>
    protected void MarkAsFailed(Exception ex)
    {
        lock (_lock)
        {
            _hasFailed = true;
            FailureException = ex;
        }
    }
}