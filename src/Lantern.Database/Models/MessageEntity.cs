namespace Lantern.Database.Models;

using Lantern.Database.Enums;
public class MessageEntity
{
    public Guid Id { get; private set; }
    public Guid ConversationId { get; private set; } = Guid.NewGuid();

    public Role Role { get; private set; }
    public string Content { get; private set; }

    public DateTime CreatedAtUtc { get; private set; } = DateTime.UtcNow;

    public MessageEntity(Guid conversationId, Role role, string content)
    {
        ConversationId = conversationId != Guid.Empty ? conversationId : throw new ArgumentException("Conversation Id can not be empty");
        Role = Enum.IsDefined(role) ? role : throw new ArgumentException("Role doesnt exist");
        Content = !string.IsNullOrWhiteSpace(content) ? content : throw new ArgumentException("message can not be empty");


    }


}