using System;
using System.ComponentModel.DataAnnotations;

namespace DataAccess.Models;

public class EmoticonModel
{

    [Key]
    public string Emoticon { get; set; } = "";
    public List<TagModel> Tags { get; set; } = new List<TagModel>();
}
