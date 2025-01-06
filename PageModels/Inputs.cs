using ZoEazy.Models;

namespace ZoEazy.PageModels;

public class Inputs(FormContent formContent, string? config = null)
{
    public FormContent FormContent { get; init; } = formContent;
    public Layout[] ExternalLayouts { get; set; } = [];
    public string? Config { get; init; } = config;
   
}