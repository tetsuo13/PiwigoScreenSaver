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

    public SettingsForm()
    {
        InitializeComponent();

        Load += new EventHandler(OnLoad);
        buttonOk.Click += new EventHandler(OnButtonOkClick);
    }

    private void OnLoad(object? sender, EventArgs e)
    {
        ((SettingsFormPresenter)Tag).Initialize();
    }

    private void OnButtonCancelClick(object sender, EventArgs e)
    {
        Close();
        DialogResult = DialogResult.Cancel;
    }

    private void OnButtonOkClick(object? sender, EventArgs e)
    {
        var valid = ((SettingsFormPresenter)Tag).ValidateSettings();

        labelFormError.Visible = !valid;

        if (!valid)
        {
            labelFormError.Text = "Please check your inputs again.";
        }
        else
        {
            ((SettingsFormPresenter)Tag).SaveSettings();
            Application.Exit();
        }
    }
}
