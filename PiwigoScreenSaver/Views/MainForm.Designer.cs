﻿namespace PiwigoScreenSaver.Views;

partial class MainForm
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
        this.galleryPictureBox = new System.Windows.Forms.PictureBox();
        ((System.ComponentModel.ISupportInitialize)(this.galleryPictureBox)).BeginInit();
        this.SuspendLayout();
        //
        // galleryPictureBox
        //
        this.galleryPictureBox.Location = new System.Drawing.Point(0, 0);
        this.galleryPictureBox.Name = nameof(this.galleryPictureBox);
        this.galleryPictureBox.Size = new System.Drawing.Size(42, 42);
        //
        // MainForm
        //
        this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(300, 300);
        //this.Controls.Add(this.logoPictureBox);
        this.DoubleBuffered = true;
        this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
        this.Name = "MainForm";
        this.ShowInTaskbar = false;
        this.TopMost = true;
        this.BackColor = System.Drawing.Color.Black;
        this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKeyDown);
        this.Location = new System.Drawing.Point(0, 0);
        this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.OnMouseClick);
        this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.OnMouseDown);
        this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.OnMouseMove);
        this.ShowInTaskbar = true;
        this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
        this.TopMost = true;
        this.WindowState = System.Windows.Forms.FormWindowState.Normal;
        ((System.ComponentModel.ISupportInitialize)(this.galleryPictureBox)).EndInit();
        this.ResumeLayout(false);
    }

    #endregion

    private System.Windows.Forms.PictureBox galleryPictureBox;
}
