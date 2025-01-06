namespace ZoEazy.Models;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public class EditorAttribute(
    string sourceMethod,
    bool readOnly = false,
    int height = 250,
    int maxHeight = 250,
    EditorAutoSizeOption autoSize = EditorAutoSizeOption.TextChanges,
    
    string onEditorChanged = "OnEditorChanged",
    string onEditorCompleted = "OnEditorCompleted",
    bool isSpellCheckEnabled = true,
    bool isTextPredictionEnabled = true
) : MetadataValidationAttribute
{
    public readonly string SourceMethod = sourceMethod;
    public readonly bool ReadOnly = readOnly;
    public readonly int Height = height;
    public readonly int MaxHeight = maxHeight;
    public readonly EditorAutoSizeOption AutoSize = autoSize;
    public string OnEditorChanged = onEditorChanged;
    public string OnEditorCompleted = onEditorCompleted;
    public readonly bool IsSpellCheckEnabled = isSpellCheckEnabled;
    public readonly bool IsTextPredictionEnabled = isTextPredictionEnabled;
    
}