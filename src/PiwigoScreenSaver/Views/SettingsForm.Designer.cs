namespace PiwigoScreenSaver.Views;

partial class SettingsForm
{
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        this.labelHeader = new System.Windows.Forms.Label();
        this.labelUrl = new System.Windows.Forms.Label();
        this.labelUsername = new System.Windows.Forms.Label();
        this.labelPassword = new System.Windows.Forms.Label();
        this.labelFieldset = new System.Windows.Forms.Label();
        this.labelFormError = new System.Windows.Forms.Label();
        this.textBoxPassword = new System.Windows.Forms.TextBox();
        this.textBoxUsername = new System.Windows.Forms.TextBox();
        this.textBoxUrl = new System.Windows.Forms.TextBox();
        this.buttonOk = new System.Windows.Forms.Button();
        this.buttonCancel = new System.Windows.Forms.Button();
        this.SuspendLayout();
        // 
        // labelHeader
        // 
        this.labelHeader.AutoSize = true;
        this.labelFieldset.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
        this.labelHeader.Location = new System.Drawing.Point(38, 50);
        this.labelHeader.Name = nameof(this.labelHeader);
        this.labelHeader.Size = new System.Drawing.Size(883, 33);
        this.labelHeader.TabIndex = 0;
        this.labelHeader.Text = "Please specify the URL and login details to your Piwigo installation.";
        // 
        // labelUrl
        // 
        this.labelUrl.AutoSize = true;
        this.labelUrl.Location = new System.Drawing.Point(96, 216);
        this.labelUrl.Name = nameof(this.labelUrl);
        this.labelUrl.Size = new System.Drawing.Size(73, 33);
        this.labelUrl.TabIndex = 2;
        this.labelUrl.Text = "URL";
        // 
        // labelUsername
        // 
        this.labelUsername.AutoSize = true;
        this.labelUsername.Location = new System.Drawing.Point(96, 293);
        this.labelUsername.Name = nameof(this.labelUsername);
        this.labelUsername.Size = new System.Drawing.Size(150, 33);
        this.labelUsername.TabIndex = 4;
        this.labelUsername.Text = "Username";
        // 
        // labelPassword
        // 
        this.labelPassword.AutoSize = true;
        this.labelPassword.Location = new System.Drawing.Point(96, 372);
        this.labelPassword.Name = nameof(this.labelPassword);
        this.labelPassword.Size = new System.Drawing.Size(151, 33);
        this.labelPassword.TabIndex = 6;
        this.labelPassword.Text = "Password:";
        // 
        // textBoxPassword
        // 
        this.textBoxPassword.Location = new System.Drawing.Point(403, 367);
        this.textBoxPassword.Name = nameof(this.textBoxPassword);
        this.textBoxPassword.PasswordChar = '*';
        this.textBoxPassword.Size = new System.Drawing.Size(561, 40);
        this.textBoxPassword.TabIndex = 7;
        // 
        // textBoxUsername
        // 
        this.textBoxUsername.Location = new System.Drawing.Point(403, 290);
        this.textBoxUsername.Name = nameof(this.textBoxUsername);
        this.textBoxUsername.Size = new System.Drawing.Size(561, 40);
        this.textBoxUsername.TabIndex = 5;
        // 
        // textBoxUrl
        // 
        this.textBoxUrl.Location = new System.Drawing.Point(403, 213);
        this.textBoxUrl.Name = nameof(this.textBoxUrl);
        this.textBoxUrl.Size = new System.Drawing.Size(561, 40);
        this.textBoxUrl.TabIndex = 3;
        // 
        // labelFieldset
        // 
        this.labelFieldset.AutoSize = true;
        this.labelFieldset.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
        this.labelFieldset.Location = new System.Drawing.Point(44, 120);
        this.labelFieldset.Name = nameof(this.labelFieldset);
        this.labelFieldset.Size = new System.Drawing.Size(248, 33);
        this.labelFieldset.TabIndex = 1;
        this.labelFieldset.Text = "General Settings";
        //
        // labelFormError
        //
        this.labelFormError.AutoSize = true;
        this.labelFormError.ForeColor = System.Drawing.Color.IndianRed;
        this.labelFormError.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
        this.labelFormError.Location = new System.Drawing.Point(403, 170);
        this.labelFormError.Name = nameof(this.labelFormError);
        this.labelFormError.Size = new System.Drawing.Size(248, 33);
        this.labelFormError.TabIndex = 99;
        this.labelFormError.Text = "Error";
        this.labelFormError.Visible = false;
        // 
        // buttonOk
        // 
        this.buttonOk.Location = new System.Drawing.Point(560, 504);
        this.buttonOk.Name = nameof(this.buttonOk);
        this.buttonOk.Size = new System.Drawing.Size(181, 57);
        this.buttonOk.TabIndex = 8;
        this.buttonOk.Text = "OK";
        this.buttonOk.UseVisualStyleBackColor = true;
        this.buttonOk.Click += new System.EventHandler(this.OnButtonOkClick);
        // 
        // buttonCancel
        // 
        this.buttonCancel.Location = new System.Drawing.Point(783, 504);
        this.buttonCancel.Name = nameof(this.buttonCancel);
        this.buttonCancel.Size = new System.Drawing.Size(181, 57);
        this.buttonCancel.TabIndex = 9;
        this.buttonCancel.Text = "Cancel";
        this.buttonCancel.UseVisualStyleBackColor = true;
        this.buttonCancel.Click += new System.EventHandler(this.OnButtonCancelClick);
        // 
        // Form1
        // 
        this.AcceptButton = this.buttonOk;
        this.AutoScaleDimensions = new System.Drawing.SizeF(17F, 33F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(1039, 625);
        this.MinimizeBox = false;
        this.MaximizeBox = false;
        this.Controls.Add(this.buttonCancel);
        this.Controls.Add(this.buttonOk);
        this.Controls.Add(this.textBoxUrl);
        this.Controls.Add(this.textBoxUsername);
        this.Controls.Add(this.textBoxPassword);
        this.Controls.Add(this.labelFormError);
        this.Controls.Add(this.labelFieldset);
        this.Controls.Add(this.labelPassword);
        this.Controls.Add(this.labelUsername);
        this.Controls.Add(this.labelUrl);
        this.Controls.Add(this.labelHeader);
        this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
        this.Name = "Form1";
        this.ShowIcon = false;
        this.ShowInTaskbar = false;
        this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
        this.Text = "Piwigo Screen Saver Settings";
        this.ResumeLayout(false);
        this.PerformLayout();
    }

    #endregion

    private System.Windows.Forms.Label labelHeader;
    private System.Windows.Forms.Label labelUrl;
    private System.Windows.Forms.Label labelUsername;
    private System.Windows.Forms.Label labelPassword;
    private System.Windows.Forms.Label labelFieldset;
    private System.Windows.Forms.Label labelFormError;
    private System.Windows.Forms.TextBox textBoxPassword;
    private System.Windows.Forms.TextBox textBoxUsername;
    private System.Windows.Forms.TextBox textBoxUrl;
    private System.Windows.Forms.Button buttonOk;
    private System.Windows.Forms.Button buttonCancel;
}
