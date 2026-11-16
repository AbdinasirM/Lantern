namespace Lantern.Database.Models;
public class ConversationEntity
{
    public  Guid Id {get; init;} =  Guid.NewGuid();
    public Guid UserId {get; private set;} 
    public string Title {get; private set;}
    public string SelectedModel {get; private set;} 

    public ConversationEntity(string title, string selectedModel, Guid userId)
    {
        Title = !string.IsNullOrWhiteSpace(title) ? title : throw new ArgumentException("Title can not be empty");
        SelectedModel = !string.IsNullOrWhiteSpace(selectedModel) ? selectedModel : throw new ArgumentException("Model can not be empty!");
        UserId = userId != Guid.Empty ? userId : throw new ArgumentException("User Id can not be empty");
    }
    public  bool SetModel(string modelname)
    {
        if (string.IsNullOrWhiteSpace(modelname))
        {
            return false;
        }

        SelectedModel = modelname;
        return true;
    }

     public bool SetTitle(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            return false;
        }

        Title = title;
        return true;
    }

    
}