using System.Diagnostics;

namespace Frontend.WebUI.Extensions.HttpConfig
{
  public class TimingHandler : DelegatingHandler
  {
    private readonly ILogger<TimingHandler> _logger;

    public TimingHandler(ILogger<TimingHandler> logger)
    {
      _logger = logger;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
      var sw = Stopwatch.StartNew();

      _logger.LogInformation("Starting request");

      var response = await base.SendAsync(request, cancellationToken);
      if (response.IsSuccessStatusCode)
      {
        _logger.LogInformation($"Finished success request in {sw.ElapsedMilliseconds}ms");
      }
      else
      {
        _logger.LogError($"Finished failed request in {sw.ElapsedMilliseconds}ms");
      }

      return response;

    }
  }

}
