using System;
using System.Drawing;
using System.Windows.Forms;

namespace POS_system
{
    internal sealed class ForgotPasswordForm : Form
    {
        private readonly string username;
        private string question;
        private bool hasCode;
        private int attempts;

        private string method;          // "question", "code" or "otp"
        private string otpCode;
        private DateTime otpExpires;

        private readonly Panel pnlMethod = new Panel { Location = new Point(0, 0), Size = new Size(390, 190) };
        private readonly Panel pnlVerify = new Panel { Location = new Point(0, 0), Size = new Size(390, 190), Visible = false };
        private readonly Panel pnlReset = new Panel { Location = new Point(0, 0), Size = new Size(390, 190), Visible = false };

        private readonly RadioButton rbQuestion = new RadioButton { AutoSize = true, Location = new Point(20, 60) };
        private readonly RadioButton rbCode = new RadioButton { AutoSize = true, Location = new Point(20, 95), Text = "Use my recovery code" };
        private readonly RadioButton rbOtp = new RadioButton { AutoSize = true, Location = new Point(20, 130), Text = "Send a code to my email (OTP)" };
        private readonly Label lblPrompt = new Label { AutoSize = false, Location = new Point(20, 20), Size = new Size(340, 45) };
        private readonly TextBox txtAnswer = new TextBox { Location = new Point(20, 70), Width = 340 };
        private readonly TextBox txtNew = new TextBox { Location = new Point(20, 45), Width = 340, UseSystemPasswordChar = true };
        private readonly TextBox txtConfirm = new TextBox { Location = new Point(20, 105), Width = 340, UseSystemPasswordChar = true };
        private readonly Button btnNext = new Button { Text = "Next", Size = new Size(100, 34) };
        private readonly Button btnCancel = new Button { Text = "Cancel", Size = new Size(100, 34) };

        public ForgotPasswordForm(string username)
        {
            this.username = username;

            Text = "Forgot Password";
            FormBorderStyle = FormBorderStyle.FixedDialog;
            StartPosition = FormStartPosition.CenterParent;
            MaximizeBox = false;
            MinimizeBox = false;
            ClientSize = new Size(390, 250);
            Font = new Font("Segoe UI", 10F);

            PasswordRecovery.GetRecoveryInfo(username, out question, out hasCode);

            string email = null;
            try { email = PasswordRecovery.GetEmail(username); } catch { }

            pnlMethod.Controls.Add(new Label
            {
                Text = "Choose how to verify it's you:",
                AutoSize = true,
                Location = new Point(20, 20)
            });

            rbQuestion.Text = "Answer my security question";
            rbQuestion.Enabled = question != null;
            rbCode.Enabled = hasCode;
            rbOtp.Enabled = !string.IsNullOrWhiteSpace(email);

            rbQuestion.Checked = question != null;
            rbCode.Checked = question == null && hasCode;
            rbOtp.Checked = question == null && !hasCode && rbOtp.Enabled;

            pnlMethod.Controls.Add(rbQuestion);
            pnlMethod.Controls.Add(rbCode);
            pnlMethod.Controls.Add(rbOtp);

            pnlVerify.Controls.Add(lblPrompt);
            pnlVerify.Controls.Add(txtAnswer);

            pnlReset.Controls.Add(new Label { Text = "New password (min. 4 characters)", AutoSize = true, Location = new Point(20, 20) });
            pnlReset.Controls.Add(txtNew);
            pnlReset.Controls.Add(new Label { Text = "Confirm new password", AutoSize = true, Location = new Point(20, 80) });
            pnlReset.Controls.Add(txtConfirm);

            btnNext.Location = new Point(ClientSize.Width - 230, ClientSize.Height - 50);
            btnCancel.Location = new Point(ClientSize.Width - 120, ClientSize.Height - 50);

            Controls.Add(btnNext);
            Controls.Add(btnCancel);
            Controls.Add(pnlMethod);
            Controls.Add(pnlVerify);
            Controls.Add(pnlReset);
            pnlMethod.BringToFront();

            btnCancel.Click += (s, e) => Close();
            btnNext.Click += BtnNext_Click;

            AcceptButton = btnNext;
            CancelButton = btnCancel;
        }

        private void BtnNext_Click(object sender, EventArgs e)
        {
            try
            {
                if (pnlMethod.Visible)
                {
                    if (!rbQuestion.Checked && !rbCode.Checked && !rbOtp.Checked)
                    {
                        MessageBox.Show(
                            "No recovery option is available for this account.\n" +
                            "An admin must open Admin Panel > Recovery Setup first.",
                            "Forgot Password", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    if (rbOtp.Checked)
                    {
                        method = "otp";
                        string email = PasswordRecovery.GetEmail(username);
                        otpCode = OtpHelper.Generate();
                        otpExpires = DateTime.UtcNow.AddMinutes(5);
                        OtpHelper.SendEmail(email, otpCode);
                        lblPrompt.Text = "Enter the 6-digit code sent to\n" + OtpHelper.MaskEmail(email);
                    }
                    else if (rbQuestion.Checked)
                    {
                        method = "question";
                        lblPrompt.Text = "Security question:\n" + question;
                    }
                    else
                    {
                        method = "code";
                        lblPrompt.Text = "Enter your recovery code:";
                    }

                    pnlMethod.Visible = false;
                    pnlVerify.Visible = true;
                    pnlVerify.BringToFront();
                    txtAnswer.Focus();
                }
                else if (pnlVerify.Visible)
                {
                    bool ok;
                    if (method == "otp")
                    {
                        if (DateTime.UtcNow > otpExpires)
                        {
                            MessageBox.Show("That code has expired. Close this window and request a new one.",
                                "Forgot Password", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            Close();
                            return;
                        }
                        ok = txtAnswer.Text.Trim() == otpCode;
                    }
                    else if (method == "question")
                        ok = PasswordRecovery.VerifyAnswer(username, txtAnswer.Text);
                    else
                        ok = PasswordRecovery.VerifyCode(username, txtAnswer.Text);

                    if (!ok)
                    {
                        attempts++;
                        if (attempts >= 3)
                        {
                            MessageBox.Show("Too many failed attempts.", "Forgot Password",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            Close();
                            return;
                        }
                        MessageBox.Show("Incorrect. Attempts left: " + (3 - attempts), "Forgot Password",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtAnswer.SelectAll();
                        txtAnswer.Focus();
                        return;
                    }

                    pnlVerify.Visible = false;
                    pnlReset.Visible = true;
                    pnlReset.BringToFront();
                    btnNext.Text = "Save";
                    txtNew.Focus();
                }
                else
                {
                    if (txtNew.Text.Length < 4)
                    {
                        MessageBox.Show("Password must be at least 4 characters.", "Forgot Password",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    if (txtNew.Text != txtConfirm.Text)
                    {
                        MessageBox.Show("Passwords do not match.", "Forgot Password",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    PasswordRecovery.SetPassword(username, txtNew.Text);
                    MessageBox.Show("Password changed. You can now log in.", "Forgot Password",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    DialogResult = DialogResult.OK;
                    Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Something went wrong:\n\n" + ex.Message, "Forgot Password",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}