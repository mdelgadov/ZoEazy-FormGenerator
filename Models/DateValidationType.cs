namespace ZoEazy.Models;

public enum DateValidationType
{
    Date,
    
    Numeric,
    NumericNull
}

public enum DateFormatType
{
    DDMMYY,
    DDMMYYYY,
    MMDDYY,
    MMDDYYYY,
    YYMMDD,
    YYYYMMDD,
    
    DDSMMSYY,
    DDSMMSYYYY,
    MMSDDSYY,
    MMSDDSYYYY,
    YYSMMSDD,
    YYYYSMMSDD,

    DDAMMAYY,
    DDAMMAYYYY,
    MMADDAYY,
    MMADDAYYYY,
    YYAMMADD,
    YYYYAMMADD,

}