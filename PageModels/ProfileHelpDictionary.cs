namespace ZoEazy.PageModels;
public class ProfileHelpDictionary : HelpDictionary
{
    public ProfileHelpDictionary()
    {
        helpMessages.Add("Acceptance", "Must Be True");
        helpMessages.Add("AppointmentTime","Between 2.00 and 4.00 am, demonic hours only");
        helpMessages.Add("BirthDate", "Birthdates between 0 and 10 years old.");
        helpMessages.Add("AppointmentDate", "In the next natural year.");
    }
}