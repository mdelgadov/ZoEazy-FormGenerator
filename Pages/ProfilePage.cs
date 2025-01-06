using CommunityToolkit.Maui.Markup;
using Microsoft.Maui.Layouts;
using Syncfusion.Maui.Toolkit;
using ZoEazy.Controls;
using ZoEazy.Models;
using ZoEazy.ViewModels;

namespace ZoEazy.Pages;

public class ProfilePage : BasePage
{
    private readonly ProfileVM _profileVm;
    public ProfilePage(ProfileVM vm) : base(vm)
    {
        BindingContext = _profileVm = vm;
        _profileVm.Inputs.ExternalLayouts = [PhotoLayout()];

        _autoFormView.Bind(AutoFormView.DateSelectedCommandProperty, static (ProfileVM vm) => vm.DateSelectedCommand);
        _autoFormView.Bind(AutoFormView.TimeSelectedCommandProperty, static (ProfileVM vm) => vm.TimeSelectedCommand);
        _autoFormView.Bind(AutoFormView.UnfocusedCommandProperty, static (ProfileVM vm) => vm.UnfocusedCommand);
        _autoFormView.Bind(AutoFormView.ParametersProperty, static (ProfileVM vm) => vm.Inputs);

        _scrollView.Content = _autoFormView;

        GetRequiredLabel();

        var buttonBar = ParseButtonBar(vm.ButtonBar);

        SubmitButton?.Bind(ButtonBase.CommandProperty, static (ProfileVM vm) => vm.SubmitCommand);

        _innerGrid.Add(buttonBar, 1, 2);

        //MainThread.BeginInvokeOnMainThread(() =>
        //{
        //    // use this to insert ad hoc code outside the AutoViewForm
        //});
    }
    FlexLayout PhotoLayout()
    {
        var photoLayout = new FlexLayout()
        {
            Margin = new Thickness(0, 10, 0, 0),
            JustifyContent = FlexJustify.SpaceEvenly
        };

        var pickPhotoButton = new Button
        {
            Text = ResourceForm.PickPhoto,
            StyleClass = ["FilledButton"],
            Command = _profileVm.PickPhotoCommand,
        };
        photoLayout.Children.Insert(0, pickPhotoButton);
        var capturePhotoButton = new Button
        {
            Text = ResourceForm.CapturePhoto,
            StyleClass = ["FilledButton"],
            Command = _profileVm.CapturePhotoCommand,
        };
        photoLayout.Children.Insert(1, capturePhotoButton);
        return photoLayout;
    }
}
