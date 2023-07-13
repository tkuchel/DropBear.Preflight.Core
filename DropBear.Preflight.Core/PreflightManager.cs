using System.Collections.Concurrent;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace DropBear.Preflight.Core;

/// <summary>
///     Represents the manager for executing preflight tasks.
/// </summary>
public class PreflightManager : IPreflightManager
{
    private readonly PreflightManagerConfig _config;
    private readonly ILogger<PreflightManager> _logger;
    private readonly ConcurrentBag<PreflightTask> _tasks;

    public PreflightManager(IOptions<PreflightManagerConfig> configOptions, ILogger<PreflightManager> logger)
    {
        _config = configOptions.Value;
        _logger = logger;
        _tasks = new ConcurrentBag<PreflightTask>();
    }

    /// <summary>
    ///     Occurs when a task has completed.
    /// </summary>
    public event EventHandler<TaskCompletionEventArgs>? TaskCompleted;

    /// <summary>
    ///     Occurs when a task completes or fails to show progress.
    /// </summary>
    public event EventHandler<ProgressEventArgs>? OverallProgress;

    /// <summary>
    ///     Occurs when a task has failed.
    /// </summary>
    public event EventHandler<TaskCompletionEventArgs>? TaskFailed;

    /// <summary>
    ///     Adds a task to the manager.
    /// </summary>
    /// <param name="task"></param>
    public void AddTask(PreflightTask task)
    {
        _tasks.Add(task);
    }

    /// <summary>
    ///     Starts the preflight process.
    /// </summary>
    public async Task StartAsync()
    {
        var sortedTasks = _tasks.OrderBy(t => t.Priority).ToList();
        var executingTasks = new HashSet<PreflightTask>();

        foreach (var task in sortedTasks)
            try
            {
                await ExecuteTaskWithRetriesAndDependenciesAsync(task, executingTasks, _config.RetryCount);
            }
            catch (Exception ex)
            {
                if (_config.EnableErrorLogging) _logger.LogError(ex, "Error executing task {TaskName}", task.Name);
                if (_config.StopOnFailure) throw;
            }
    }

    /// <summary>
    ///     Executes a task with retries and its dependencies.
    /// </summary>
    /// <param name="task"></param>
    /// <param name="executingTasks"></param>
    /// <param name="remainingRetries"></param>
    /// <exception cref="InvalidOperationException"></exception>
    private async Task ExecuteTaskWithRetriesAndDependenciesAsync(PreflightTask task,
        HashSet<PreflightTask> executingTasks, int remainingRetries)
    {
        if (!executingTasks.Add(task))
        {
            if (_config.EnableErrorLogging)
                _logger.LogError("Circular dependency detected for task {TaskName}", task.Name);
            throw new InvalidOperationException($"Circular dependency detected for task {task.Name}");
        }

        foreach (var dependency in task.Dependencies)
            await ExecuteTaskWithRetriesAndDependenciesAsync(dependency, executingTasks, _config.RetryCount);

        if (!task.IsCompleted)
            try
            {
                if (_config.EnableDebugLogging) _logger.LogInformation("Starting task {TaskName}", task.Name);

                using (var cts = new CancellationTokenSource(_config.TaskTimeout))
                {
                    await task.ExecuteAsync(cts.Token);
                }

                if (_config.EnableDebugLogging) _logger.LogInformation("Completed task {TaskName}", task.Name);

                TaskCompleted?.Invoke(this, new TaskCompletionEventArgs(task, task.IsCompleted, null));
            }
            catch (Exception? ex)
            {
                if (_config.EnableErrorLogging) _logger.LogError(ex, "Error executing task {TaskName}", task.Name);

                if (remainingRetries > 0)
                {
                    if (_config.EnableDebugLogging)
                        _logger.LogInformation("Retrying task {TaskName} ({RemainingRetries} retries left)", task.Name,
                            remainingRetries);
                    await ExecuteTaskWithRetriesAndDependenciesAsync(task, executingTasks, remainingRetries - 1);
                }
                else
                {
                    TaskFailed?.Invoke(this, new TaskCompletionEventArgs(task, false, ex));
                    if (_config.StopOnFailure) throw;
                }
            }

        var completedTasks = _tasks.Count(t => t.IsCompleted);
        OverallProgress?.Invoke(this,
            new ProgressEventArgs(completedTasks == 0 ? 0 : (double)completedTasks / _tasks.Count));

        executingTasks.Remove(task);
    }
}