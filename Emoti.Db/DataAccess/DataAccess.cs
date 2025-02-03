using System;
using DataAccess.Context;
using DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DataAccess.DataAccess;

public class DataAccess : IDataAccess
{

    EmoticonsDbContext _emoticonsDbContext;
    ILogger<DataAccess> _logger;

    public DataAccess(EmoticonsDbContext emoticonsDbContext, ILogger<DataAccess> logger)
    {
        _emoticonsDbContext = emoticonsDbContext;
        _logger = logger;
    }

    public Task AddEmoticon(string emoticon, List<string> tags)
    {
        EmoticonModel emoticonModel = new EmoticonModel { Emoticon = emoticon };

        foreach (var tag in tags)
        {
            TagModel? tagModel = _emoticonsDbContext.Tags.Find(tag);
            if (tagModel != null)
            {
                emoticonModel.Tags.Add(tagModel);
            }
            else
            {
                emoticonModel.Tags.Add(new TagModel { Tag = tag });
            }
        }

        try 
        {
            _emoticonsDbContext.Emoticons.Add(emoticonModel);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while adding emoticon");
        }

        return _emoticonsDbContext.SaveChangesAsync();
    }

    public async Task<List<EmoticonModel>> GetEmoticons()
    {
        List<EmoticonModel> emoticons = new();

        try
        {
            emoticons = await _emoticonsDbContext.Emoticons.ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while getting emoticons");
        }
        

        return emoticons;
    }

    public async Task<List<EmoticonModel>> GetEmoticonsByTag(string tag)
    {
        List<EmoticonModel> emoticons = new();

        try
        {
            emoticons = await _emoticonsDbContext.Tags.Where(t => t.Tag.Contains(tag)).SelectMany(t => t.Emoticons).ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while getting emoticons by tag");
        }

        return emoticons;
    }
}
