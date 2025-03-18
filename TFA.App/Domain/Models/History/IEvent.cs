namespace TFA.App.Domain.Models.History;

public interface IEvent
{
    Guid Id { get; set; }
    DateTime EventDate { get; set; }
}