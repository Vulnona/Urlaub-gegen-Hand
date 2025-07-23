using System.Diagnostics;
using Microsoft.Extensions.Options;
using UGHApi.Models;

public class TemplateSettings
{
    public string SuccessTemplate { get; set; }
    public string FailedTemplate { get; set; }
    public string ChromeDriverPath { get; set; }
}

namespace UGHApi.Services
{
    public class PythonScriptRunner : IHostedService, IDisposable
    {
        private readonly ILogger<PythonScriptRunner> _logger;
        private Timer _timer;
        private Process _chromeDriverProcess;
        private readonly TemplateSettings _templateSettings;

        public PythonScriptRunner(
            ILogger<PythonScriptRunner> logger,
            IOptions<TemplateSettings> templateSettings
        )
        {
            _logger = logger;
            _templateSettings = templateSettings.Value;
        }

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

                string chromeDriverPath =
                    Environment.GetEnvironmentVariable("ChromeDriverPath")
                    ?? _templateSettings.ChromeDriverPath;
                if (string.IsNullOrEmpty(chromeDriverPath))
                {
                    _logger.LogError("ChromeDriver path is not set.");
                    return;
                }

                _chromeDriverProcess = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = chromeDriverPath,
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                    },
                };
                _chromeDriverProcess.Start();

                _logger.LogInformation("Running Python script...");

                string pythonScriptPath =
                    Environment.GetEnvironmentVariable("FacebookCrawlerScriptPath")
                    ?? _templateSettings.FacebookCrawlerScriptPath;
                if (string.IsNullOrEmpty(pythonScriptPath))
                {
                    _logger.LogError("Python script path is not set.");
                    return;
                }

                ProcessStartInfo start = new ProcessStartInfo
                {
                    FileName = "python3",
                    Arguments = pythonScriptPath,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                };

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
                _logger.LogError(ex, "Error running Python script!");
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
    }
}
