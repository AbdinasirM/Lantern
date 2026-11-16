namespace Lantern.Database.Models;

public class UserEntity
{
    public Guid Id {get; private set;} = Guid.NewGuid();
    public  string Name {get; private set;}

    public UserEntity(string name)
    {
        Name = !string.IsNullOrWhiteSpace(name) ? name : throw new ArgumentException("User Name can not be empty");
    }

    public bool SetName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return false;
        }
        Name = name.Trim();
        return true;
    }

}