using System.Text;
using ClientPlugin.Settings;
using ClientPlugin.Settings.Elements;
using ClientPlugin.Settings.Tools;
using Sandbox.Graphics.GUI;
using Shared.Config;
using VRage.Input;
using VRageMath;

namespace ClientPlugin;

public enum ExampleEnum
{
    FirstAlpha,
    SecondBeta,
    ThirdGamma,
    AndTheDelta,
    Epsilon,
}

public class Config : PluginConfig
{
    #region Options

    // TODO: Define your configuration options and their default values
    // An option added later can default differently on a new deployment than on an existing one:
    // private bool newOption = ConfigStorage.IsNewDeployment;
    private bool toggle = true;
    private int integer = 2;
    private float number = 0.1f;
    private string text = "Default Text";
    private ExampleEnum dropdown = ExampleEnum.FirstAlpha;
    private Color color = Color.Cyan;
    private Color colorWithAlpha = new Color(0.8f, 0.6f, 0.2f, 0.5f);
    private Binding keybind = new Binding(MyKeys.None);

    #endregion

    #region User interface

    // TODO: Settings dialog title
    public readonly string Title = "Config Demo";

    // TODO: Settings dialog controls, one property for each configuration option

    [Separator("Some settings")]
    [Checkbox(description: "Checkbox Tooltip")]
    public bool Toggle
    {
        get => toggle;
        set => SetValue(ref toggle, value);
    }

    [Slider(
        -1f,
        10f,
        1f,
        SliderAttribute.SliderType.Integer,
        description: "Integer Slider Tooltip, enabled by the checkbox",
        enabledBy: nameof(Toggle)
    )]
    public int Integer
    {
        get => integer;
        set => SetValue(ref integer, value);
    }

    [Slider(
        -5f,
        4.5f,
        0.5f,
        SliderAttribute.SliderType.Float,
        description: "Float Slider Tooltip",
        unit: "m/s"
    )]
    public float Number
    {
        get => number;
        set => SetValue(ref number, value);
    }

    [Textbox(description: "Textbox Tooltip")]
    public string Text
    {
        get => text;
        set => SetValue(ref text, value);
    }

    [Dropdown(description: "Dropdown Tooltip")]
    public ExampleEnum Dropdown
    {
        get => dropdown;
        set => SetValue(ref dropdown, value);
    }

    [Separator("More settings")]
    [Color(description: "RGB color")]
    public Color Color
    {
        get => color;
        set => SetValue(ref color, value);
    }

    [Color(hasAlpha: true, description: "RGBA color")]
    public Color ColorWithAlpha
    {
        get => colorWithAlpha;
        set => SetValue(ref colorWithAlpha, value);
    }

    [Keybind(description: "Keybind Tooltip - Unbind by right clicking the button")]
    public Binding Keybind
    {
        get => keybind;
        set => SetValue(ref keybind, value);
    }

    [Button(description: "Button Tooltip")]
    public void Button()
    {
        MyGuiSandbox.AddScreen(
            MyGuiSandbox.CreateMessageBox(
                MyMessageBoxStyleEnum.Info,
                buttonType: MyMessageBoxButtonsType.OK,
                messageText: new StringBuilder("You clicked me!"),
                messageCaption: new StringBuilder("Custom Button Function"),
                size: new Vector2(0.6f, 0.5f)
            )
        );
    }

    #endregion

    public static readonly Config Default = new Config();
    public static readonly Config Current = ConfigStorage.Load();
}
