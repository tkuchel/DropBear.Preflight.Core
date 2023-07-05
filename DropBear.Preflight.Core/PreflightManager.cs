using Microsoft.Extensions.Logging;

namespace DropBear.Preflight.Core;

public class PreflightManager
{
    // A list of tasks to be executed
    private readonly List<PreflightTask> _tasks;

    // Configuration options
    private readonly PreflightManagerConfig _config;

    // Logger
    private readonly ILogger<PreflightManager> _logger;

    // Event for task completion
    public event EventHandler<TaskCompletionEventArgs>? TaskCompleted;

    // Event for overall progress
    public event EventHandler<ProgressEventArgs>? OverallProgress;

    public PreflightManager(PreflightManagerConfig config, ILogger<PreflightManager> logger)
    {
        this._config = config;
        this._logger = logger;
        _tasks = new List<PreflightTask>();
    }

    // Other methods...

    // Method to start executing tasks
    public async Task StartAsync()
    {
        // Sort tasks based on priority
        _tasks.Sort((t1, t2) => t1.Priority.CompareTo(t2.Priority));

        var completedTasks = 0;

        foreach (var task in _tasks.Where(task => task.Dependencies.All(d => d.IsCompleted)))
        {
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

                // Update the number of completed tasks and raise the OverallProgress event
                completedTasks++;
                OverallProgress?.Invoke(this, new ProgressEventArgs((double)completedTasks / _tasks.Count));
            }
            catch (Exception ex)
            {
                // Log the error
                _logger.LogError(ex, $"Error executing task {task.GetType().Name}");

                // Handle task failure based on the configuration
                if (_config.StopOnFailure)
                {
                    throw;
                }
                else
                {
                    // Raise the TaskCompleted event with the exception
                    TaskCompleted?.Invoke(this, new TaskCompletionEventArgs(task, false, ex));
                }
            }
        }
    }
}
