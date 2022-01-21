using Microsoft.Xaml.Behaviors;
using System.Windows;
using System.Windows.Controls;

namespace MyToDo.Extensions;

public class PasswordExtensions
{

    public static string GetPassword(DependencyObject obj)
    {
        return (string)obj.GetValue(PasswordyProperty);
    }

    public static void SetPassword(DependencyObject obj, string value)
    {
        obj.SetValue(PasswordyProperty, value);
    }


    public static readonly DependencyProperty PasswordyProperty =
        DependencyProperty.RegisterAttached("Password", typeof(string), typeof(PasswordExtensions), new PropertyMetadata(string.Empty, OnPasswordPropertyChanged));

    private static void OnPasswordPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var pwdBox = d as PasswordBox;
        string password = (string)e.NewValue;

        if (pwdBox != null && pwdBox.Password != password)
        {
            pwdBox.Password = password;
        }
    }
}

public class PasswordBehavior: Behavior<PasswordBox>
{
    protected override void OnAttached()
    {
        base.OnAttached();
        AssociatedObject.PasswordChanged += AssociatedObject_PasswordChanged;
    }

    private void AssociatedObject_PasswordChanged(object sender, RoutedEventArgs e)
    {
        PasswordBox pwdBox = sender as PasswordBox;
        string password = PasswordExtensions.GetPassword(pwdBox);

        if (pwdBox != null && pwdBox.Password != password)
        {
            PasswordExtensions.SetPassword(pwdBox, pwdBox.Password);
        }
    }

    protected override void OnDetaching()
    {
        AssociatedObject.PasswordChanged -= AssociatedObject_PasswordChanged;
    }
}