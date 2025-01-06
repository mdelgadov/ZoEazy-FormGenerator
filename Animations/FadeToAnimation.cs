using CommunityToolkit.Maui.Animations;

namespace ZoEazy.Animations;
public enum FadeDirection
{
    Up,
    Down
}

public class FadeToAnimation : BaseAnimation
{
    public static readonly BindableProperty OpacityProperty =
        BindableProperty.Create(nameof(Opacity), typeof(double), typeof(FadeToAnimation), 0.0d,
            propertyChanged: (bindable, oldValue, newValue) =>
                ((FadeToAnimation)bindable).Opacity = (double)newValue);

    public double Opacity
    {
        get => (double)GetValue(OpacityProperty);
        set => SetValue(OpacityProperty, value);
    }

    public override Task Animate(VisualElement view, CancellationToken token)
    {
        view.Dispatcher.Dispatch(() => view.FadeTo(Opacity, 200, Easing.Linear));
         return Task.CompletedTask;
    }

}

public class FadeInAnimation : BaseAnimation
{
    public static readonly BindableProperty DirectionProperty =
        BindableProperty.Create(nameof(Direction), typeof(FadeDirection), typeof(FadeInAnimation), FadeDirection.Up,
            propertyChanged: (bindable, oldValue, newValue) =>
                ((FadeInAnimation)bindable).Direction = (FadeDirection)newValue);

    public FadeDirection Direction
    {
        get => (FadeDirection)GetValue(DirectionProperty);
        set => SetValue(DirectionProperty, value);
    }

    public override Task Animate(VisualElement view, CancellationToken token)
    {

        view.Dispatcher.Dispatch(() => view.Animate("FadeIn", FadeIn(view), 16, Length));

        return Task.CompletedTask;
    }

    

    internal Animation FadeIn(VisualElement view)
    {
        var animation = new Animation();

        animation.WithConcurrent(f => view.Opacity = f, 0, 1, this.Easing);

        animation.WithConcurrent(
            f => view.TranslationY = f,
            view.TranslationY + (Direction == FadeDirection.Up ? 50 : -50), view.TranslationY,
            this.Easing);

        return animation;
    }
}


public class FadeOutAnimation : BaseAnimation
{

    public static readonly BindableProperty DirectionProperty =
        BindableProperty.Create(nameof(Direction), typeof(FadeDirection), typeof(FadeOutAnimation), FadeDirection.Up,
            propertyChanged: (bindable, oldValue, newValue) =>
                ((FadeOutAnimation)bindable).Direction = (FadeDirection)newValue);

    public FadeDirection Direction
    {
        get => (FadeDirection)GetValue(DirectionProperty);
        set => SetValue(DirectionProperty, value);
    }

    public override Task Animate(VisualElement view, CancellationToken token)
    {

        view.Dispatcher.Dispatch(() => view.Animate("FadeOut", FadeOut(view), 16, Length));

        return Task.CompletedTask;
    }



    internal Animation FadeOut(VisualElement view)
    {
        var animation = new Animation();

        animation.WithConcurrent(f => view.Opacity = f, 1, 0, this.Easing);

        animation.WithConcurrent(
            f => view.TranslationY = f,
            view.TranslationY + (Direction == FadeDirection.Up ? 50 : -50), view.TranslationY,
            this.Easing);

        return animation;
    }
}


public class FadeToTranslateAnimation : BaseAnimation
{
    public enum OpacityDirection
    {
        Transparent,
        Visible
    }

    private static readonly BindableProperty DirectionProperty =
        BindableProperty.Create(nameof(Direction), typeof(OpacityDirection), typeof(FadeToTranslateAnimation), OpacityDirection.Visible,
            propertyChanged: (bindable, oldValue, newValue) =>
                ((FadeToTranslateAnimation)bindable).Direction = (OpacityDirection)newValue);

    public OpacityDirection Direction
    {
        get => (OpacityDirection)GetValue(DirectionProperty);
        set => SetValue(DirectionProperty, value);
    }

    private static readonly BindableProperty PositionProperty =
        BindableProperty.Create(nameof(Position), typeof(int), typeof(FadeToTranslateAnimation), 0,
            propertyChanged: (bindable, oldValue, newValue) =>
                ((FadeToTranslateAnimation)bindable).Position =(int)newValue);

    public int Position
    {
        get => (int)GetValue(PositionProperty);
        set => SetValue(PositionProperty, value);
    }
    
    public override Task Animate(VisualElement view, CancellationToken token)
    {
        return Task.WhenAll(
            view.FadeTo((int)Direction, 1000, easing: Easing.CubicInOut),
            view.TranslateTo(Position, 0, 500, easing: Easing.CubicOut)
        );
    }
}

//public class FadeOutAnimation : BaseAnimation
//{
//    public enum FadeDirection
//    {
//        Up,
//        Down
//    }

//    public FadeDirection Direction
//    {
//        get => (FadeDirection)GetValue(0);

//    }

//    public override Task Animate(VisualElement view, CancellationToken token)
//    {


//        view.Dispatcher.Dispatch(() => view.Animate("FadeOut", FadeOut(view), 16, 100u));

//        return Task.CompletedTask;
//    }



//    internal Animation FadeOut(VisualElement view)
//    {
//        var animation = new Animation();

//        animation.WithConcurrent(
//            f => view.Opacity = f,
//            1, 0);

//        animation.WithConcurrent(
//            f => view.TranslationY = f,
//            view.TranslationY, view.TranslationY + (Direction == FadeDirection.Up ? 50 : -50));

//        return animation;
//    }
//}


public class FadeOutBehavior : Behavior<ContentView>
{
    protected override void OnAttachedTo(ContentView bindable)
    {
        base.OnAttachedTo(bindable);
        // Add logic to fade out the ContentView
        bindable.FadeTo(0, 10000); // Fade out over 1 second
    }

    protected override void OnDetachingFrom(ContentView bindable)
    {
        base.OnDetachingFrom(bindable);
        // Add logic if needed when detaching the behavior
    }
}

public class FadeInBehavior : Behavior<ContentView>
{
    protected override void OnAttachedTo(ContentView bindable)
    {
        base.OnAttachedTo(bindable);
        // Add logic to fade In the ContentView
        bindable.FadeTo(1, 1000); // Fade In over 1 second
    }

    protected override void OnDetachingFrom(ContentView bindable)
    {
        base.OnDetachingFrom(bindable);
        // Add logic if needed when detaching the behavior
    }
}
