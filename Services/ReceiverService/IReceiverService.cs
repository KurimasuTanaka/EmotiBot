namespace StudyBot.Abstract;

public interface IReceiverService
{
    Task ReceiveAsync(CancellationToken cancellationToken);
}