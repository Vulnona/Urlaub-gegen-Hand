using System.Diagnostics;


namespace UGHApi.Services
{
    public class PythonScriptRunner : IHostedService, IDisposable
    {
        private readonly ILogger<PythonScriptRunner> _logger;
        private Timer _timer;
        private Process _chromeDriverProcess;

        public PythonScriptRunner(ILogger<PythonScriptRunner> logger)
        {
            _logger = logger;
        }
        #region start-facebook-crawling
        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("PythonScriptRunner is starting.");
            _timer = new Timer(RunScript, null, TimeSpan.Zero, TimeSpan.FromMinutes(2));
            return Task.CompletedTask;
        }
        private void RunScript(object state)
        {
            try
            {
                _logger.LogInformation("Starting ChromeDriver...");
                _chromeDriverProcess = new Process();
                _chromeDriverProcess.StartInfo.FileName = @"/home/azureuser/migration-fix/chromedriver.exe";
                _chromeDriverProcess.StartInfo.UseShellExecute = false;
                _chromeDriverProcess.StartInfo.RedirectStandardOutput = true;
                _chromeDriverProcess.StartInfo.RedirectStandardError = true;
                _chromeDriverProcess.Start();
                _logger.LogInformation("Running Python script...");
                ProcessStartInfo start = new ProcessStartInfo();
                start.FileName = "python";
                start.Arguments = @"/home/azureuser/migration-fix/facebookcrawler.py";
                start.UseShellExecute = false;
                start.RedirectStandardOutput = true;
                start.RedirectStandardError = true;
                using (Process process = Process.Start(start))
                {
                    process.WaitForExit();
                    string result = process.StandardOutput.ReadToEnd();
                    string error = process.StandardError.ReadToEnd();
                    _logger.LogInformation(result);
                    if (!string.IsNullOrEmpty(error))
                    {
                        _logger.LogError(error);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error running Python script");
            }
            finally
            {
                _logger.LogInformation("Stopping ChromeDriver...");
                if (_chromeDriverProcess != null && !_chromeDriverProcess.HasExited)
                {
                    _chromeDriverProcess.Kill();
                    _chromeDriverProcess.Dispose();
                }
            }
        }
        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("PythonScriptRunner is stopping.");
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }
        public void Dispose()
        {
            _timer?.Dispose();
            if (_chromeDriverProcess != null)
            {
                if (!_chromeDriverProcess.HasExited)
                {
                    _chromeDriverProcess.Kill();
                }
                _chromeDriverProcess.Dispose();
            }
        }
        #endregion
    }
}
