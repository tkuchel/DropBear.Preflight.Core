namespace DropBear.Preflight.Core;

public abstract class PreflightTask
{
    // The priority of the task
    public int Priority { get; set; } = 0;

    // The list of tasks that this task depends on
    public List<PreflightTask> Dependencies { get; set; } = new List<PreflightTask>();

    // A flag indicating whether the task has completed
    public bool IsCompleted { get; private set; } = false;

    // The method to execute the task
    public abstract Task ExecuteAsync(CancellationToken cancellationToken);

    // A method to mark the task as completed
    protected void MarkAsCompleted()
    {
        IsCompleted = true;
    }
}
