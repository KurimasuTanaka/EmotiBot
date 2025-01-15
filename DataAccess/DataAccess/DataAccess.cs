using System;
using DataAccess.Context;
using DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.DataAccess;

public class DataAccess : IDataAccess
{

    EmoticonsDbContext _emoticonsDbContext;

    public DataAccess(EmoticonsDbContext emoticonsDbContext)
    {
        _emoticonsDbContext = emoticonsDbContext;
    }

    public Task AddEmoticon(string emoticon, List<string> tags)
    {
        _emoticonsDbContext.Emoticons.Add(new EmoticonModel { Emoticon = emoticon, Tags = tags.Select(tag => new TagModel { Tag = tag }).ToList() });
    
        return _emoticonsDbContext.SaveChangesAsync();
    }

    public Task<List<EmoticonModel>> GetEmoticons()
    {
        return _emoticonsDbContext.Emoticons.ToListAsync();
    }

    public async Task<List<EmoticonModel>> GetEmoticonsByTag(string tag)
    {
        return await _emoticonsDbContext.Tags.Where(t => t.Tag.Contains(tag)).SelectMany(t => t.Emoticons).ToListAsync();
    }
}
