# ZoEazy Form Generator
[ZoEazy](https://github.com/mdelgadov/ZoEazy-FormGenerator/)

## Credits

This software uses the following open source packages:

- [Net9](https://dotnet.microsoft.com/en-us/download/dotnet/9.0)
- [Net Maui9]
- [Net Toolkit MVVM](https://github.com/CommunityToolkit/dotnet)
- [Maui Toolkit](https://github.com/CommunityToolkit/Maui)
- [Syncfusion Maui Toolkit](https://help.syncfusion.com/maui-toolkit/introduction/overview)
- [Maui Markup](https://github.com/CommunityToolkit/Maui.Markup)

Inspired by [UraniumUI](https://github.com/enisn/UraniumUI), but have to create my own to make it work with this stack. Some parts are still used like extensions to parse the elements.

## Support
Contact me at mdelgado@verticalviral.com

## Instructions
The current template creates a form based in the Profile prefix. Excuse the wimsical tones, Developer is a loney occupation.
All fields are defined by ValidationAttributes in the ProfileFields.Cs
All validation messages are in the ProfileMessages
The helper messages are in the ProfileHelpDictionary
No Xaml, only Code and Markup extensions

Little bug:
The Syncfusion toolkit has issues with the color of the Hint in Darkmode. Even the default Syncfusion template has the same problem.



The promises of Code and Markup are: Reuse, Testable, Maintainable, Refactorable, Streamlines code.



