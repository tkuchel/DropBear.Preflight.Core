namespace DropBear.Preflight.Core;

/// <summary>
///     Defines the contract for a task to be executed during the preflight phase.
/// </summary>
public interface IPreflightTask
{
    /// <summary>
    ///     Gets or sets the name of the task.
    /// </summary>
    string Name { get; set; }
    
    /// <summary>
    ///   Gets or sets the description of the task.
    /// </summary>
    string Description { get; set; }

    /// <summary>
    ///     Gets or sets the priority of the task.
    /// </summary>
    int Priority { get; set; }

    /// <summary>
    ///     Gets the list of tasks that this task depends on.
    /// </summary>
    IReadOnlyList<IPreflightTask> Dependencies { get; }

    /// <summary>
    ///     Gets a value indicating whether the task has completed.
    /// </summary>
    bool IsCompleted { get; }

    /// <summary>
    ///     Gets a value indicating whether the task has failed.
    /// </summary>
    bool HasFailed { get; }

    /// <summary>
    ///     Gets the exception that caused the task to fail, if any.
    /// </summary>
    Exception? FailureException { get; }

    /// <summary>
    ///     Executes the task.
    /// </summary>
    Task ExecuteAsync(CancellationToken cancellationToken);

    /// <summary>
    ///     Adds a dependency to this task.
    /// </summary>
    void AddDependency(IPreflightTask task);
}