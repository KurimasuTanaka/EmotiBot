


class EmoticonList
{
    string name { get; set; }
    List<EmoticonList>? emoticonLists { get; set; } = null;
    List<string>? emoticons { get; set; } = null;

    public EmoticonList(string name, List<EmoticonList> emoticonLists)
    {
        this.name = name;
        this.emoticonLists = emoticonLists;
    }

    public EmoticonList(string name,List<string> emoticons)
    {
        this.name = name;
        this.emoticons = emoticons;
    }

    public List<string> GetEmoticons()
    {
        if(emoticonLists is null && emoticons is not null)
        {
            return emoticons.Select(e => name + "-" + e).ToList();
        } else if(emoticonLists is not null && emoticons is null)
        {
            return emoticonLists.SelectMany(e => e.GetEmoticons().Select(em => name + "-" + em)).ToList();
        } else
        {
            return new List<string>();
        }
    }

}