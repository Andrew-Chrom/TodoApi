namespace TodoApi.Command.Refresh;

public record RefreshCommand(string RefreshToken, CancellationToken cancellationToken);
