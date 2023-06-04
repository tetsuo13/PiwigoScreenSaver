using PiwigoScreenSaver.Presenters;
using System;
using System.Windows.Forms;

namespace PiwigoScreenSaver.Views;

public partial class SettingsForm : Form, ISettingsFormView
{
    public string Url
    {
        get { return textBoxUrl.Text; }
        set { textBoxUrl.Text = value; }
    }

    public string Username
    {
        get { return textBoxUsername.Text; }
        set { textBoxUsername.Text = value; }
    }

    public string Password
    {
        get { return textBoxPassword.Text; }
        set { textBoxPassword.Text = value; }
    }

    private readonly ISettingsFormPresenter _presenter;

    public SettingsForm(ISettingsFormPresenter presenter)
    {
        _presenter = presenter ?? throw new ArgumentNullException(nameof(presenter));

        InitializeComponent();

        Load += new EventHandler(OnLoad);
        buttonOk.Click += new EventHandler(OnButtonOkClick);
    }

    private void OnLoad(object? sender, EventArgs e)
    {
        _presenter.Initialize();
        Url = _presenter.Url;
        Username = _presenter.Username;
        Password = _presenter.Password;
    }

    private void OnButtonCancelClick(object sender, EventArgs e)
    {
        Close();
        DialogResult = DialogResult.Cancel;
    }

    private void OnButtonOkClick(object? sender, EventArgs e)
    {
        _presenter.Url = Url;
        _presenter.Username = Username;
        _presenter.Password = Password;

        var valid = _presenter.ValidateSettings();

        labelFormError.Visible = !valid;

        if (!valid)
        {
            labelFormError.Text = "Please check your inputs again.";
        }
        else
        {
            _presenter.SaveSettings();
            Application.Exit();
        }
    }
}
