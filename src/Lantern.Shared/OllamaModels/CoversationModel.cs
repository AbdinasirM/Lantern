namespace Lantern.Shared;
public class ConversationModel
{
    public  Guid Id {get; init;} =  Guid.NewGuid();
    public string Title {get; private set;}
    public string SelectedModel {get; private set;} 

    public ConversationModel(string title, string selectedModel)
    {
        Title = !string.IsNullOrWhiteSpace(title) ? title : throw new ArgumentException("Title can not be empty");
        SelectedModel = !string.IsNullOrWhiteSpace(selectedModel) ? selectedModel : throw new ArgumentException("Model can not be empty!");
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