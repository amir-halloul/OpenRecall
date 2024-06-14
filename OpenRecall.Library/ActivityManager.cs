using OpenRecall.Library.Models;
using OpenRecall.Library.Utilities;

public class ActivitySnapshotEventArgs : EventArgs
{
    public ActivitySnapshot Snapshot { get; set; }
    public int SnapshotIndex { get; set; }
    public int SnapshotThreshold { get; set; }
}

public class ActivityEventArgs : EventArgs
{
    public Activity Activity { get; set; }
}

public class ActivityManager
{
    public event EventHandler<ActivitySnapshotEventArgs> ActivitySnapshotTaken;
    public event EventHandler<ActivityEventArgs> ActivityCreated;

    private readonly ScreenshotUtility _screenshotUtility = new ScreenshotUtility();
    private readonly ActiveWindowUtility _activeWindowUtility = new ActiveWindowUtility();
    private readonly AiUtility _aiUtility;
    private readonly int _snapshotInterval;
    private readonly int _snapshotThreshold;

    private CancellationTokenSource _cancellationTokenSource;

    public ActivityManager(AiUtility aiUtility, int snapshotInterval, int snapshotThreshold)
    {
        _aiUtility = aiUtility;
        _snapshotInterval = snapshotInterval;
        _snapshotThreshold = snapshotThreshold;
    }

    public void Start()
    {
        if (_cancellationTokenSource != null)
        {
            throw new InvalidOperationException("ActivityManager is already running.");
        }

        _cancellationTokenSource = new CancellationTokenSource();
        var cancellationToken = _cancellationTokenSource.Token;

        Task.Run(() => CaptureActivityLoopAsync(cancellationToken), cancellationToken);
    }

    public void Stop()
    {
        _cancellationTokenSource?.Cancel();
        _cancellationTokenSource = null;
    }

    private async Task CaptureActivityLoopAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            var activity = new Activity
            {
                Snapshots = new List<ActivitySnapshot>(),
                StartTime = DateTime.Now,
                EndTime = DateTime.Now,
            };

            int snapshotIndex = 0;

            while (snapshotIndex < _snapshotThreshold && !cancellationToken.IsCancellationRequested)
            {
                var screenshot = _screenshotUtility.TakeScreenshot();
                var activeWindow = _activeWindowUtility.GetActiveWindowTitle();
                var activitySnapshot = new ActivitySnapshot
                {
                    ActiveWindowTitle = activeWindow,
                    Screenshot = screenshot,
                    Timestamp = DateTime.Now
                };

                activity.Snapshots.Add(activitySnapshot);
                activity.EndTime = DateTime.Now;
                snapshotIndex++;

                // Raising the ActivitySnapshotTaken event
                ActivitySnapshotTaken?.Invoke(this, new ActivitySnapshotEventArgs
                {
                    Snapshot = activitySnapshot,
                    SnapshotIndex = snapshotIndex,
                    SnapshotThreshold = _snapshotThreshold
                });

                
                await Task.Delay(_snapshotInterval, cancellationToken);
            }

            if (!cancellationToken.IsCancellationRequested)
            {
                var description = await _aiUtility.SummarizeActivityAsync(activity);
                activity.Description = description;

                // Raising the ActivityCreated event
                ActivityCreated?.Invoke(this, new ActivityEventArgs { Activity = activity });
            }
        }
    }
}