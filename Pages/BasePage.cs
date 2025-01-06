using CommunityToolkit.Maui.Markup;
using Syncfusion.Maui.Toolkit.Buttons;
using ZoEazy.Animations;
using ZoEazy.Controls;
using ZoEazy.Models;
using ZoEazy.Resources;
using ZoEazy.ViewModels;
using ActivityIndicator = Microsoft.Maui.Controls.ActivityIndicator;

namespace ZoEazy.Pages;

public class BasePage : ContentPage
{
    protected enum RowEnum { Form }
    protected enum ColumnEnum { Form = 1 }

    private readonly int _translationPhone = 300;
    private readonly int _translationDesktop = 300;
    private readonly int _translation;

    protected readonly ActivityIndicator ActivityIndicator = new ActivityIndicator();

    protected AutoFormView _autoFormView;
    protected SfButton? SubmitButton;

    protected VerticalStackLayout? _content { get; set; }
    protected Border? _border { get; set; }

    protected Grid? _innerGrid { get; set; }
    private Grid? _outerGrid { get; set; }

    protected Grid? _footer { get; set; }
    private BaseVM Vm { get; set; }
    protected ScrollView _scrollView;

    protected BasePage(BaseVM vm)
    {
        Vm = vm;
        _translation = App.IsDesktop ? _translationDesktop : _translationPhone;
        _autoFormView = [];
        _scrollView = new ScrollView();
        InitializeComponents();
    }

    private void InitializeComponents()
    {
        this.Style = StyleResource.GetStyle(App.IsDesktop ? "SpacerDesktop" : "SpacerPhone")!;

        _scrollView = new ScrollView();
        _innerGrid = new Grid
        {
            RowDefinitions = Rows.Define(GridLength.Auto, GridLength.Star, GridLength.Auto),
            ColumnDefinitions = Columns.Define(GridLength.Star, Stars(App.IsPhone ? 10 : 4), GridLength.Star),
            HorizontalOptions = LayoutOptions.Fill,
            VerticalOptions = LayoutOptions.Fill,
        }.Row(1); //inside the row 1 (covering all space) of outerGrid

        _innerGrid.Add(_scrollView, 1, 1);

       // _innerGrid.Add(_refreshButton, 1, 1);

        _outerGrid = new Grid
        {
            RowDefinitions = Rows.Define(0, GridLength.Star),
            ColumnDefinitions = Columns.Define(GridLength.Star),
            HorizontalOptions = LayoutOptions.Fill,
            VerticalOptions = LayoutOptions.Fill
        };

        _outerGrid.Add(_innerGrid, 0, 1);

        Content = new Border
        {
            StyleClass = ["SurfaceContainer", "Rounded", "Elevation2"],
            Content = _outerGrid
        };

    }
    protected Layout ParseButtonBar(ButtonBarData[] buttons)
    {
        Layout layout = buttons.Length == 1 && App.IsPhone
            ? new Grid()
            : new HorizontalStackLayout();

        layout.Children.Clear();

        foreach (var data in buttons)
        {
            var button = GetSfButton(data);
            layout.Children.Add(button);
        }

        if (App.IsPhone)
            layout.Top();
        else
            layout.Column(1).End();

        return layout;

        SfButton GetSfButton(ButtonBarData button)
        {

            var styles = new List<string>();
            ;
            var response = new SfButton
            {
                Text = button.Text,
            };
            if (!button.IsSubmit)
                styles.Add("Complement");
            else
            {
                if (App.IsDesktop)
                    styles.Add("Desktop");
                else if (buttons.Length == 1)
                    styles.Add("PhoneOneButton");

                SubmitButton = response;
            }

            response.StyleClass = styles.ToArray();

            return response;
        }
    }

    protected void GetRequiredLabel()
    {
        if (Vm.HasRequired) return;
        var required = new HorizontalStackLayout
        {
            HorizontalOptions = LayoutOptions.Start,
            Margin = new Thickness(0, 20, 0, 0),
            VerticalOptions = LayoutOptions.Center,
            Children =
            {
                StandardImages.Required(false, false),
                new Label
                {
                    Text = ResourceForm.HasRequired,
                    Style = StyleResource.GetStyle("HyperLinkText")!,
                }
            }
        }.Column(0).Row(1);
       // _footer.Add(required);
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        this.TranslationX = _translation;
        this.Opacity = 0.5;

        var animation = new FadeToTranslateAnimation
        {
            Direction = FadeToTranslateAnimation.OpacityDirection.Visible,
            Position = 0
        };

        await animation.Animate(this, CancellationToken.None);
        this.TranslationX = 0;
        this.Opacity = 1;
    }

}



public class OldBasePage : ContentPage
{
    protected enum RowEnum { Form }
    protected enum ColumnEnum { Form = 1 }

    private readonly int _translationPhone = 300;
    private readonly int _translationDesktop = 300;
    private readonly int _translation;

    protected readonly ActivityIndicator _activityIndicator;

    protected AutoFormView _autoFormView;
    protected SfButton? SubmitButton;

    protected VerticalStackLayout _content { get; set; }
    protected Border _border { get; set; }

    protected Grid _outerGrid { get; set; }
    protected Grid _innerGrid { get; set; }

    protected Grid _footer { get; set; }
    protected BaseVM _vm { get; set; }
    protected OldBasePage(BaseVM vm)
    {
        _vm = vm;
        _translation = App.IsDesktop ? _translationDesktop : _translationPhone;
        _autoFormView = new AutoFormView();

        _outerGrid = new Grid
        {
            RowDefinitions = Rows.Define(0, GridLength.Star),
            HorizontalOptions = LayoutOptions.Fill,
            VerticalOptions = LayoutOptions.Fill
        };

        var colDefinitions = Stars(App.IsPhone ? 10 : 4);


        _innerGrid = new Grid
        {
            RowDefinitions = Rows.Define(GridLength.Star),
            ColumnDefinitions = Columns.Define(GridLength.Star, colDefinitions, GridLength.Star),
            HorizontalOptions = LayoutOptions.Fill,
            VerticalOptions = LayoutOptions.Fill,
        }.Row(1); //inside the row 1 (covering all space) of outerGrid

        _content = new VerticalStackLayout
        {
            StyleClass = [App.IsDesktop ? "SpacerDesktop" : "SpacerPhone"],
            Children = { }
        };

        _border = new Border
        {
            StyleClass = new[] { "SurfaceContainer", "Rounded", "Elevation2" },

        };

        _border.Content = _outerGrid;
        _content.Add(_border);

        var anchor = new HorizontalStackLayout()
        {
            ZIndex = 1010,
            VerticalOptions = LayoutOptions.End,
            HorizontalOptions = LayoutOptions.Center
        }.Row(RowEnum.Form).Column(ColumnEnum.Form);

        _footer = new Grid();


        if (App.IsPhone)
        {
            _footer.ColumnDefinitions = Columns.Define(GridLength.Star);
            _footer.RowDefinitions = Rows.Define(GridLength.Star, GridLength.Star, GridLength.Star);
        }
        else
        {
            _footer.ColumnDefinitions = Columns.Define(GridLength.Auto, GridLength.Star);
            _footer.RowDefinitions = Rows.Define(GridLength.Star);
        }

        _activityIndicator = new ActivityIndicator().Row(0).RowSpan(2).Column(1);

        anchor.Add(_footer);
        _innerGrid.Add(anchor);
        _activityIndicator.Bind(IsVisibleProperty, nameof(vm.IsBusy));
        _activityIndicator.Bind(IsEnabledProperty, nameof(vm.IsBusy));

        _outerGrid.Add(_activityIndicator);
        _outerGrid.Add(_innerGrid);

        Content = _content;
    }

    protected Layout ParseButtonBar(ButtonBarData[] buttons)
    {
        Layout layout = buttons.Length == 1 && App.IsPhone
            ? new Grid()
            : new HorizontalStackLayout();

        layout.Children.Clear();

        foreach (var data in buttons)
        {
            var button = GetSfButton(data);
            layout.Children.Add(button);
        }

        if (App.IsPhone)
            layout.Top();
        else
            layout.Column(1).End();

        return layout;

        SfButton GetSfButton(ButtonBarData button)
        {

            var styles = new List<string>();
            ;
            var response = new SfButton
            {
                Text = button.Text,
            };
            if (!button.IsSubmit)
                styles.Add("Complement");
            else
            {
                if (App.IsDesktop)
                    styles.Add("Desktop");
                else if (buttons.Length == 1)
                    styles.Add("PhoneOneButton");

                SubmitButton = response;
            }

            response.StyleClass = styles.ToArray();

            return response;
        }
    }

    protected void GetRequiredLabel()
    {
        if (_vm.HasRequired) return;
        var required = new HorizontalStackLayout
        {
            HorizontalOptions = LayoutOptions.Start,
            Margin = new Thickness(0, 20, 0, 0),
            VerticalOptions = LayoutOptions.Center,
            Children =
            {
                StandardImages.Required(false, false),
                new Label
                {
                    Text = ResourceForm.HasRequired,
                    Style = StyleResource.GetStyle("HyperLinkText")!,
                }
            }
        }.Column(0).Row(1);
        _footer.Add(required);
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        this.TranslationX = _translation;
        this.Opacity = 0.5;

        var animation = new FadeToTranslateAnimation
        {
            Direction = FadeToTranslateAnimation.OpacityDirection.Visible,
            Position = 0
        };

        await animation.Animate(this, CancellationToken.None);
        this.TranslationX = 0;
        this.Opacity = 1;
    }
}