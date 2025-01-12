
using Services.EmoticonsService;
using Telegram.Bot.Types.InlineQueryResults;

public class Emoticon{

    static int idCounter = 0;

    public string tag { get; set; }
    public string emoticon { get; set; }

    public int id { get; set; }
    public Emoticon(string tag, string emoticon)
    {
        this.id = idCounter++;
        this.tag = tag;
        this.emoticon = emoticon;
    }
}
class EmoticonsSerivice : IEmoticonsSerivice
{
    private List<Emoticon> emoticonList = new List<Emoticon>();
    public EmoticonsSerivice ()
    {
        emoticonList.Add(new Emoticon("doubt", "(* ^ ω ^)"));
        emoticonList.Add(new Emoticon("doubt", "(¬_¬)"));
        emoticonList.Add(new Emoticon("doubt", "(￢‿￢ )"));

        emoticonList.Add(new Emoticon("confustion", "(￣ω￣;)"));
        emoticonList.Add(new Emoticon("confustion", "(•ิ_•ิ)?"));
        emoticonList.Add(new Emoticon("confustion", "ლ(ಠ_ಠ ლ)"));

        emoticonList.Add(new Emoticon("indifferent", "┐(￣ヘ￣)┌"));
        emoticonList.Add(new Emoticon("indifferent", "┐(︶▽︶)┌"));
        emoticonList.Add(new Emoticon("indifferent", "(￢_￢)"));

        emoticonList.Add(new Emoticon("sadness", "(つ﹏<)･ﾟ｡"));
        emoticonList.Add(new Emoticon("sadness", ".｡･ﾟﾟ･(＞_＜)･ﾟﾟ･｡."));
        emoticonList.Add(new Emoticon("sadness", "(╥﹏╥)"));

        emoticonList.Add(new Emoticon("anger", "(＃`Д´)"));
        emoticonList.Add(new Emoticon("anger", "٩(╬ʘ益ʘ╬)۶"));
        emoticonList.Add(new Emoticon("anger", "ヽ( `д´*)ノ"));

        emoticonList.Add(new Emoticon("love", "( ´ ▽ ` ).｡ｏ♡"));
        emoticonList.Add(new Emoticon("love", "♡ (⇀ 3 ↼)"));
        emoticonList.Add(new Emoticon("love", "(ﾉ´ з `)ノ"));
        emoticonList.Add(new Emoticon("love", "(♥ω♥*)"));

        emoticonList.Add(new Emoticon("love", "(⌒‿⌒)"));
        emoticonList.Add(new Emoticon("joy", "٩(◕‿◕｡)۶"));
        emoticonList.Add(new Emoticon("joy", "(* ^ ω ^)"));
    }

    public List<Emoticon> GetEmoticons(string search = "")
    {
        if(search == "")
        {
            return emoticonList;
        } else return emoticonList.Select(e => e).Where(e => e.tag.Contains(search)).ToList();
    }
}