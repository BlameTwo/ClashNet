using System.Windows.Input;
using System.Windows;

namespace ClashNet.Styles.Propertys;

public class ItemsEx
{
    public static readonly DependencyProperty CommandProperty =
        DependencyProperty.RegisterAttached("Command", typeof(ICommand), typeof(ItemsEx), new PropertyMetadata(null));



    public static ICommand GetCommand(DependencyObject obj)
    {
        return (ICommand)obj.GetValue(CommandProperty);
    }

    public static void SetCommand(DependencyObject obj, ICommand value)
    {
        obj.SetValue(CommandProperty, value);
    }

    public static object GetCommandParameter(DependencyObject obj)
    {
        return (object)obj.GetValue(CommandParameterProperty);
    }

    public static void SetCommandParameter(DependencyObject obj, object value)
    {
        obj.SetValue(CommandParameterProperty, value);
    }

    public static readonly DependencyProperty CommandParameterProperty =
        DependencyProperty.RegisterAttached("CommandParameter", typeof(object), typeof(ItemsEx), new PropertyMetadata(null));




}
