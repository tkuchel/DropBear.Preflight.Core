using Microsoft.Extensions.Logging;

namespace DropBear.Preflight.Core;
/// <summary>
/// Manages the execution of preflight tasks.
/// </summary>
public class PreflightManager
{
    // Configuration options
    private readonly PreflightManagerConfig _config;

    // Logger
    private readonly ILogger<PreflightManager> _logger;

    // A list of tasks to be executed
    private readonly List<PreflightTask> _tasks;

    public PreflightManager(PreflightManagerConfig config, ILogger<PreflightManager> logger)
    {
        _config = config;
        _logger = logger;
        _tasks = new List<PreflightTask>();
    }

    /// <summary>
    /// Occurs when a task has completed.
    /// </summary>
    public event EventHandler<TaskCompletionEventArgs>? TaskCompleted;

    /// <summary>
    /// Occurs when the overall progress has changed.
    /// </summary>
    public event EventHandler<ProgressEventArgs>? OverallProgress;

    /// <summary>
    /// Adds a task tothe list of tasks to be executed.
    /// </summary>
    public void AddTask(PreflightTask task)
    {
        _tasks.Add(task);
    }

    /// <summary>
    /// Starts the execution of tasks.
    /// </summary>
    public async Task StartAsync()
    {
        // Sort tasks based on priority
        _tasks.Sort((t1, t2) => t1.Priority.CompareTo(t2.Priority));

        var executingTasks = new HashSet<PreflightTask>();

        foreach (var task in _tasks)
            try
            {
                await ExecuteTaskWithDependenciesAsync(task, executingTasks);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error executing task {task.GetType().Name}");
                if (_config.StopOnFailure) throw;
            }
    }

    /// <summary>
    /// Executes a task and its dependencies.
    /// </summary>
    private async Task ExecuteTaskWithDependenciesAsync(PreflightTask task, HashSet<PreflightTask> executingTasks)
    {
        // Check for circular dependencies
        if (!executingTasks.Add(task))
        {
            _logger.LogError($"Circular dependency detected for task {task.GetType().Name}");
            throw new InvalidOperationException($"Circular dependency detected for task {task.GetType().Name}");
        }

        // Execute all dependencies first
        foreach (var dependency in task.Dependencies)
            await ExecuteTaskWithDependenciesAsync(dependency, executingTasks);

        // Only execute the task if it hasn't been completed yet
        if (!task.IsCompleted)
            try
            {
                // Log the start of the task
                _logger.LogInformation($"Starting task {task.GetType().Name}");

                // Execute the task with a cancellation token and timeout
                using (var cts = new CancellationTokenSource(_config.TaskTimeout))
                {
                    await task.ExecuteAsync(cts.Token);
                }

                // Log the completion of the task
                _logger.LogInformation($"Completed task {task.GetType().Name}");

                // Raise the TaskCompleted event
                TaskCompleted?.Invoke(this, new TaskCompletionEventArgs(task, true, null));
            }
            catch (Exception ex)
            {
                // Log the error
                _logger.LogError(ex, $"Error executing task {task.GetType().Name}");

                // Handle task failure based on the configuration
                if (_config.StopOnFailure)
                    throw;
                // Raise the TaskCompleted event with the exception
                TaskCompleted?.Invoke(this, new TaskCompletionEventArgs(task, false, ex));
            }

        // Calculate the overall progress and raise the OverallProgress event
        var completedTasks = _tasks.Count(t => t.IsCompleted);
        OverallProgress?.Invoke(this, new ProgressEventArgs((double)completedTasks / _tasks.Count));

        // Remove the task from the executing tasks
        executingTasks.Remove(task);
    }
}

