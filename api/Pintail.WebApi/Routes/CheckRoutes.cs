using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using Pintail.BusinessLogic.Checks;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Pintail.WebApi.Controllers;

public class CheckRoutes : IRouteBundle {
  public void RegisterRoutes(WebApplication app) {
    var group = app.MapGroup("/api/checks").WithTags("Checks");

    group.MapGet("", GetChecksRoute);
  }

  public async Task GetChecksRoute(HttpContext context, IMediator mediator, IHostApplicationLifetime lifetime, int count = 200) {
    var tokenSource = new CancellationTokenSource();
    var cancellationToken = tokenSource.Token;

    if (context.WebSockets.IsWebSocketRequest) {
      using var webSocket = await context.WebSockets.AcceptWebSocketAsync();

      var interval = TimeSpan.FromSeconds(30);
      var buffer = new byte[1024 * 4];
      var receiveResult = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), cancellationToken);

      var timer = new Timer(async _ => {
        var checks = await mediator.Send(new GetChecks.Query());
        var json = JsonSerializer.Serialize(checks);
        var bytes = Encoding.UTF8.GetBytes(json);
        if (webSocket.State == WebSocketState.Aborted) {
          return;
        }

        await webSocket.SendAsync(new ArraySegment<byte>(bytes), receiveResult.MessageType, receiveResult.EndOfMessage, cancellationToken);
      }, null, TimeSpan.Zero, interval);

      try {
        receiveResult = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), lifetime.ApplicationStopping);
      } catch (Exception) {
        await tokenSource.CancelAsync();
        tokenSource.Dispose();
      }

      await timer.DisposeAsync();
      if (webSocket.State == WebSocketState.Open) {
        await webSocket.CloseAsync(receiveResult?.CloseStatus ?? WebSocketCloseStatus.NormalClosure, receiveResult?.CloseStatusDescription ?? "Terminated", CancellationToken.None);
      }
    }
  }
}